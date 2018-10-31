using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongAI : MonoBehaviour {
    public GameObject ball;
    private Rigidbody rb;

    public float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = (rb.transform.forward * (ball.transform.position.z - transform.position.z) * speed).normalized;
        //transform.position = new Vector3(transform.position.x, 0, ball.transform.position.z);
    }
}
