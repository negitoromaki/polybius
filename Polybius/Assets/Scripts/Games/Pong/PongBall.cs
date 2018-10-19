using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongBall : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 5f;
    public PongManager manager;

    private void Start()
    {
        // initial start
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Yo where's the rigidbody!");
        }

        ResetBall();
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    public void ResetBall()
    {
        //rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector3((Random.Range(0, 2) * 2 - 1) * speed, 0, (Random.Range(0, 2) * 2 - 1) * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector3 vel = new Vector3(rb.velocity.x, 0, (rb.velocity.z / 2) + (collision.collider.attachedRigidbody.velocity.z / 3));
            rb.velocity = vel;
        }
        if (collision.collider.CompareTag("LeftWall"))
        {
            // score AI
            manager.ScoreRight();
        }
        else if (collision.collider.CompareTag("RightWall"))
        {
            // score player
            manager.ScoreLeft();
        }
    }
}
