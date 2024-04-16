using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numEnemies = 16;
    [SerializeField] private float formationPeriod = 5f;
    [SerializeField] private float formationTransitionDuration = 1f;

    private List<EnemyStats> _enemies;
    private float _formationTimer;
    private int _currentFormationIndex;
    private Vector3[] _formationPositions;
    private bool _isRecycle = true;

    public bool IsRecycle
    {
        get => _isRecycle;
        set => _isRecycle = value;
    }

    private void Awake()
    {
        _enemies = new List<EnemyStats>();
        SpawnEnemies();
        SetFormation(0);
    }

    private void Update()
    {
        if (_isRecycle)
        {
            _formationTimer += Time.deltaTime;
            if (_formationTimer >= formationPeriod)
            {
                _formationTimer = 0f;
                StartCoroutine(TransitionFormation());
            }
        }
        else
        {
            SetFormation(3);
        }
    }
    
    private void SpawnEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            _enemies.Add(enemyObj.GetComponent<EnemyStats>());
        }
    }
    
    private void SetFormation(int index)
    {
        _currentFormationIndex = index;
        _formationPositions = GetFormationPositions(index);

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] != null) // Kiểm tra nếu kẻ địch không phải là null
            {
                _enemies[i].SetPosition(_formationPositions[i]);
            }
        }
    }
    
    private IEnumerator TransitionFormation()
    {
        _currentFormationIndex = (_currentFormationIndex + 1) % 4; // Chạy đén 3 thì dừng
        
        _isRecycle = true;
        
        Vector3[] newPositions = GetFormationPositions(_currentFormationIndex);

        float elapsedTime = 0f;
        while (elapsedTime < formationTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / formationTransitionDuration);

            for (int i = 0; i < numEnemies; i++)
            {
                _enemies[i].SetPosition(Vector3.Lerp(_formationPositions[i], newPositions[i], t));
            }
            yield return null;
        }

        _formationPositions = newPositions;
        if (_currentFormationIndex == 3)
        {
            _isRecycle = false;
        }
    }
    
    public void OnEnemyDestroyed(EnemyStats enemy)
    {
        // Tìm và xóa kẻ địch khỏi mảng _enemies
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == enemy)
            {
                _enemies[i] = null; // Xóa tham chiếu
                break;
            }
        }
    }
    
    private Vector3[] GetFormationPositions(int index)
    {
        Vector3[] positions = new Vector3[numEnemies];
        float spacing = 1.5f; // Khoảng cách mỗi enemy
        int indexPos;
        
        switch (index)
        {
            case 0: // Hình vuông 
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        positions[i * 4 + j] = new Vector3(transform.position.x + (i * spacing - 2.2f),
                            transform.position.y - (j * spacing), 0);
                    }
                }
                break;
    
            case 1: // Hình tam giác cân
                indexPos = 0;
                positions[indexPos++] = new Vector3(transform.position.x, transform.position.y, 0); // Đỉnh của tam giác
                // Tạo viền tam giác cân
                float horizontalSpacing = 1.0f;
                for (int i = 1; i < 5; i++)
                {
                    if (i < 4)
                    {
                        // Hàng 2, 3, 4 với 2 kẻ địch và khoảng cách tăng dần
                        positions[indexPos++] = new Vector3(transform.position.x - horizontalSpacing, transform.position.y -
                            (i * spacing), 0);
                        positions[indexPos++] = new Vector3(transform.position.x + horizontalSpacing, transform.position.y -
                            (i * spacing), 0);
                        horizontalSpacing += 1.2f; // Tăng khoảng cách ngang cho hàng tiếp theo
                    }
                    else if(i == 4)
                    {
                        // Hàng thứ 5 với 9 kẻ địch
                        for (int j = -4; j <= 4; j++)
                        {
                            positions[indexPos++] = new Vector3(transform.position.x + j * 1f,
                                transform.position.y - i * spacing, 0);
                        }
                    }
                }
                break;
    
            case 2: // Hình con thoi
                indexPos = 0;
                // Hàng 1 và 5 có 1 kẻ địch
                positions[indexPos++] = new Vector3(transform.position.x, transform.position.y, 0); // Hàng 1
                positions[indexPos++] = new Vector3(transform.position.x, transform.position.y - 4 * spacing, 0); // Hàng 5

                // Hàng 2 và 4 có 4 kẻ địch
                for (int i = -3; i <= 0; i += 2)
                {
                    for (int j = -1; j <= 2; j++)
                    {
                        positions[indexPos++] = new Vector3(transform.position.x + j * spacing - 0.75f, transform.position.y + i * spacing, 0);
                    }
                }

                // Hàng 3 có 6 kẻ địch
                for (int j = -2; j <= 3; j++)
                {
                    positions[indexPos++] = new Vector3(transform.position.x + j * spacing - 0.75f, transform.position.y - 2f * spacing,0);
                }
                break;
            case 3: // Hình con thoi
                indexPos = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 1)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (indexPos < numEnemies)
                            {
                                positions[indexPos] = new Vector3(transform.position.x + (j * spacing * 6f) - 4.5f, transform.position.y -
                                    (i * spacing), 0);
                                indexPos++; // Tăng indexPos sau khi gán giá trị
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (indexPos < numEnemies)
                            {
                                positions[indexPos] = new Vector3(transform.position.x + (j * spacing) - 4.5f, transform.position.y -
                                    (i * spacing), 0);
                                indexPos++; // Tăng indexPos sau khi gán giá trị
                            }
                        }
                    }
                }
                break;
        }
    
        return positions;
    }
}
