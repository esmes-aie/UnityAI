using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

    private GraphProject.Graph<Transform> graph;
    private GraphProject.Solver<Transform> solver;

    public List<Transform> start, end;

    public Transform pathStart, pathEnd;

    public List<Transform> path;

    public int gridSize = 4;
    public float gridSpace = 1;
    public bool genGridKeep = false;
    public bool generateGrid = true;



    private static float diff(Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    }

    void InitializeGraph()
    {
        graph = new GraphProject.Graph<Transform>();
        solver = new GraphProject.Solver<Transform>();
        solver.graph = graph;

        HashSet<Transform> set = new HashSet<Transform>();
        set.UnionWith(start);
        set.UnionWith(end);

        foreach (var trans in set)
            if (trans != null)
                graph.AddNode(trans);

        for (int i = 0; i < start.Count; ++i)
            if (start[i] != null && end[i] != null)
                graph.AddEdge(start[i], end[i], diff,0.0001f, diff(start[i], end[i]));
    }

    // Use this for initialization
    void Start ()
    {
        InitializeGraph();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
            if(t != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(t.position, .1f);
            }


        for (int i = 0; i < start.Count; ++i)
            if(start[i] != null && end[i] != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(start[i].position,
                                  end[i].position);
            }

        foreach(var t in path)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(t.position, .2f);
        }
    }



    // Called by the editor when a serialized field is modified
    private void OnValidate()
    {
        // check gate
        if (!generateGrid) return;
        generateGrid = false;

        if(!genGridKeep)
        { 
            foreach (Transform child in transform)
                    UnityEditor.EditorApplication.delayCall += () =>
                                        { DestroyImmediate(child.gameObject); };

            // clear out my lists of references to destroyed objects
            start.Clear();
            end.Clear();

            // total transforms we need
            Transform[] t_array = new Transform[gridSize * gridSize];

            // generate nodes and apply offsets
            for (int i = 0; i < gridSize; ++i)
            {
                for (int j = 0; j < gridSize; ++j)
                {
                    var t = new GameObject(GetInstanceID() + "_gwp_" + (i * gridSize + j));
                    t.transform.parent = transform;
                    t.transform.localPosition = new Vector3((i + 0.5f - gridSize / 2.0f) * gridSpace,
                                                            (j + 0.5f - gridSize / 2.0f) * gridSpace,
                                                             0);
                    t_array[i * gridSize + j] = t.transform;
                }
            }

            // generate connections
            for(int n = 0; n < gridSize*gridSize; ++n)
            {
                // form an edge with the node above me
                if(((n+1)%gridSize) !=  0)
                {
                    start.Add(t_array[n]);
                    end.Add(t_array[n+1]);
                }
                // form an edge with the node to the right of me
                if(n+gridSize < gridSize * gridSize)
                {
                    start.Add(t_array[n]);
                    end.Add(t_array[n + gridSize]);
                }
            }
        }
        genGridKeep = true;

        if(pathStart != null && pathEnd != null)
        {
            InitializeGraph();
            solver.init(pathStart, pathEnd, diff, 100.0f);
            while (solver.step());
            path = solver.solution;
        }
    }
}
