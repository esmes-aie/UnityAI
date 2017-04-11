﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFlee : MonoBehaviour {
    
    Rigidbody rb;
    public Transform target;
    public float speed = 1;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        var dir = -(target.position - transform.position).normalized;

        var desiredVelocity = dir * speed;

        var force = desiredVelocity - rb.velocity;

        rb.AddForce(force);

        Vector3 head = rb.velocity;
        head.y = 0;

        // will constrain x/y rotation
        transform.LookAt(transform.position + head, Vector3.up);
    }
}
