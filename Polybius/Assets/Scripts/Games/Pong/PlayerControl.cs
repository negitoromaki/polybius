using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical"); // TODO: change this to allow buttons instead
        rb.velocity = new Vector3(0, 0, v) * speed;    
    }

    public void Move()
    {
        
    }
}
