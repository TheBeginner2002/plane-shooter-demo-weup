using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private int point = 0;
    
    private float _timeFire;

    public int Point
    {
        get => point;
        set => point = value;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > _timeFire)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        _timeFire = Time.time + fireRate;
        
        Instantiate(playerBullet, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
    }

    public void AddPoint()
    {
        point++;
    }
}
