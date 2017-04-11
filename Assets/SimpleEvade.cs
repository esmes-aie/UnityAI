﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleEvade : MonoBehaviour {

    public GameObject target, target2;
    private Rigidbody rb;
    public float speed = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var tpos = target.transform.position + target.GetComponent<Rigidbody>().velocity;
        // (target position + target velocity)
        var dir = -(tpos - transform.position).normalized;

        var desiredVelocity = dir * speed;

        var force = desiredVelocity - rb.velocity;


        var tpos2 = target2.transform.position + target2.GetComponent<Rigidbody>().velocity;
        // (target position + target velocity)
        var dir2 = -(tpos2 - transform.position).normalized;

        var desiredVelocity2 = dir2 * speed;

        var force2 = desiredVelocity2 - rb.velocity;

     
        rb.AddForce((force + force2).normalized * speed);


        Vector3 head = rb.velocity;
        head.y = 0;

        transform.LookAt(transform.position + head, Vector3.up);
    }
}