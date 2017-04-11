using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWander : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1;
    public float radius = 1;
    public float jitter = 1;
    public float distance = 1;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 target = Vector3.zero;
        
        // Get initial wander target
        target = Random.insideUnitCircle.normalized * radius;
  
        // Apply Jitter
        target = (Vector2)target + Random.insideUnitCircle * jitter;
        target = target.normalized * radius;

        // correct the 2D parts to 3D
        target.z = target.y;
        target += transform.position;

        // Apply Distance
        target += transform.forward * distance;

        // use seek
        var dir = (target - transform.position).normalized;
        var desiredVelocity = dir * speed;
        var force = (desiredVelocity - rb.velocity);
        rb.AddForce(force);

        // TODO, use torque later.
        transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}