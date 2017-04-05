using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGraphWalker : MonoBehaviour {

    public GraphManager gm;

    private List<Vector3> pathToWalk;

    public Transform target;
	// Use this for initialization
	void Start ()
    {
        if(target != null)
            pathToWalk = gm.FindPathBetween(transform, target);
	}

    // Update is called once per frame
    void Update ()
    {
        if (pathToWalk.Count == 0) return;
        // What direction do we want to walk in?
        Vector3 dir = (pathToWalk[0] - transform.position).normalized;

        transform.position += dir * Time.deltaTime;

        if (Vector3.Distance(pathToWalk[0], transform.position) < .1f)
            pathToWalk.RemoveAt(0);
    }

    private void OnValidate()
    {
        if (target != null && gm != null)
        { 
            pathToWalk = gm.FindPathBetween(transform, target);
        }
        else pathToWalk = null;
    }

    private void OnDrawGizmos()
    {
        if(pathToWalk != null)
            for(int i = 0; i < pathToWalk.Count; ++i)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(pathToWalk[i], .3f);

                if(i < pathToWalk.Count-1)
                    Gizmos.DrawLine(pathToWalk[i], pathToWalk[i + 1]);
            }
    }
}
