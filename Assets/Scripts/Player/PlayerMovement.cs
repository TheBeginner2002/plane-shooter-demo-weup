using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        transform.Translate( speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -8.8f, 0), 0);

        if (transform.position.x <= -6.7f)
        {
            transform.position = new Vector3(6.7f, transform.position.y, 0);
        }
        else if (transform.position.x >= 6.7f)
        {
            transform.position = new Vector3(-6.7f, transform.position.y, 0);
        }
    }
}
