using System;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private int damage = 1;

    private SpawnManager _spawnManager;
    
    private void Awake()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }
    
    private void Update()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * bulletSpeed));

        if (transform.position.y >= 10.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            if (enemy != null && !_spawnManager.IsRecycle)
            {
                enemy.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
