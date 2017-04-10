using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGraphWalker : MonoBehaviour {

    public GraphManager gm;

    private List<Vector3> pathToWalk;

    public List<Transform> targets;
    private GameObject target;
    int currentTarget = 0;
    bool useRandom;
	// Use this for initialization
	void Start ()
    {
        target = new GameObject();
        pathToWalk = new List<Vector3>();
	}

    // Update is called once per frame
    void Update ()
    {
        if (pathToWalk.Count == 0)
        {
            if(!useRandom)
            {
                useRandom = true;
                currentTarget++;
                currentTarget %= targets.Count;
                pathToWalk = gm.FindPathBetween(transform, targets[currentTarget]);
            }
            else
            {
                do
                {
                    target.transform.position = 
                        new Vector3(Random.Range(-4,4),Random.Range(-4, 4), 0);
                }
                while (Physics.CheckSphere(target.transform.position, .2f));
                useRandom = false;
                pathToWalk = gm.FindPathBetween(transform, target.transform);
            }
        }
        
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
            pathToWalk = gm.FindPathBetween(transform, targets[0]);
        }
        else pathToWalk = null;
    }

    private void OnDrawGizmos()
    {
        if(pathToWalk != null)
        {
            Gizmos.DrawLine(transform.position, pathToWalk[0]);

            for (int i = 0; i < pathToWalk.Count; ++i)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(pathToWalk[i], .3f);
               
                if (i < pathToWalk.Count-1)
                    Gizmos.DrawLine(pathToWalk[i], pathToWalk[i + 1]);
            }
        }
    }
}
