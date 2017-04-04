using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGraphWalker : MonoBehaviour {

    public GraphManager gm;

    private List<Transform> pathToWalk;

	// Use this for initialization
	void Start ()
    {
        pathToWalk = gm.path;
	}

    // Update is called once per frame
    void Update ()
    {
        if (pathToWalk.Count == 0) return;
        // What direction do we want to walk in?
        Vector3 dir = (pathToWalk[0].position - transform.position).normalized;


        transform.position += dir * Time.deltaTime;

        if (Vector3.Distance(pathToWalk[0].position, transform.position) < .1f)
            pathToWalk.RemoveAt(0);

    }
}
