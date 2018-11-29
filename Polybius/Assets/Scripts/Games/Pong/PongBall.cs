using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongBall : MonoBehaviour
{
    private Rigidbody rb;

    private bool isPaused = true;
    private bool stateChanged = false;
    private Vector3 currentVelocity;

    public CustomTrackableHandler trackHandler; // used for vuforia

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
        if (manager.gameOver)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (trackHandler.isTracked)
        {
            if (isPaused)
            {
                isPaused = false;
                stateChanged = true;
            }
        }
        else
        {
            isPaused = true;
            stateChanged = false;
        }

        if (!isPaused)
        {
            if (stateChanged)
            {
                Debug.Log("Changing velocity back!");
                rb.velocity = currentVelocity;
                stateChanged = false;
            }
            else
            {
                currentVelocity = rb.velocity;
            }

            // just to make sure the ball doesn't get stuck
            if (Mathf.Abs(rb.velocity.x) < 2)
            {
                float xVal = Random.Range(4, 8) * (Random.Range(0, 2) * 2 - 1); // first part gives you a random num from 4 to 8 and second part is either positive or negative
                Vector3 newVel = rb.velocity;
                newVel.x = xVal;
                rb.velocity = newVel;
            }

            rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            Debug.Log("Changing velocity to zero!");
            rb.velocity = Vector3.zero;

            Debug.Log(rb.velocity);
        }
    }

    public void ResetBall()
    {
        //rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, 0);
        Vector3 newVel = new Vector3((Random.Range(0, 2) * 2 - 1) * speed, 0, (Random.Range(0, 2) * 2 - 1) * speed);
        rb.velocity = newVel;
        currentVelocity = newVel;
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
