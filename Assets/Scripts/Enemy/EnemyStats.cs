using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int health = 5;
    
    private SpawnManager _spawnManager;
    private PlayerManager _player;

    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }
    
    public void SetPosition(Vector3 newPosition)
    {
        if (this.gameObject == null)
        {
            return;
        }
        transform.position = newPosition;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!_spawnManager.IsRecycle)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                _player.AddPoint();
                _spawnManager.OnEnemyDestroyed(this);
                Destroy(this.gameObject);
            }
        }
    }
}
