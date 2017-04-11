using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// need the rigidbody of my target

[RequireComponent(typeof(Rigidbody))]
public class SimplePursue: MonoBehaviour {
    
    Rigidbody rb;
    public GameObject target;
    public float speed = 1;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // modify how much we consider the velocity
        // the further we are away, the more we want to consider it.
        var t =  Vector3.Distance(target.transform.position, transform.position)/
                                  target.GetComponent<Rigidbody>().velocity.magnitude;

        var tpos = target.transform.position + t * target.GetComponent<Rigidbody>().velocity;
        // (target position + target velocity)
        var dir = (tpos - transform.position).normalized;

        var desiredVelocity = dir * speed;

        var force = desiredVelocity - rb.velocity;

        rb.AddForce(force);

        Vector3 head = rb.velocity;
        head.y = 0;

        transform.LookAt(transform.position + head, Vector3.up);
    }
}