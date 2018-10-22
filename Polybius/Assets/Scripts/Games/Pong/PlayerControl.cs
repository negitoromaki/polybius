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

    public void ResetVel()
    {
        rb.velocity = Vector3.zero;
    }

    public void MoveLeft()
    {
        rb.velocity = new Vector3(0, 0, 1) * speed;
    }

    public void MoveRight()
    {
        rb.velocity = new Vector3(0, 0, -1) * speed;
    }
}
