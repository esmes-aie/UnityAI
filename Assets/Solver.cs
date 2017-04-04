using System;
using System.Collections.Generic;
using System.Text;

namespace GraphProject
{
    class Solver<T>
    {
		class Meta
		{
            public enum VisitState {undiscovered, frontier, explored};
            public VisitState state;
            public Graph<T>.Node prev;

            public uint depth;
            public float g;

            public Meta()
            {
                state = VisitState.undiscovered;
            }
		};

        public Graph<T> graph;

        private Meta[] metadata;
        private Queue<Graph<T>.Node> frontier;

        private T start, goal;
        private float threshold;
        private Graph<T>.FindDelegate searcher;
        private Graph<T>.Node goalNode;

        public List<T> solution
        {
            get
            {
                List<T> retval = new List<T>();
                retval.Add(goal);
                var n = goalNode;
				while(n != null)
                {
                    retval.Add(n.data);
                    n = metadata[n.uid].prev;
                }
                retval.Add(start);
                retval.Reverse();
                return retval;
            }
        }

        // Cleanup, setup and start our search.
        public void init(T a_start, T a_goal, Graph<T>.FindDelegate a_searcher, float a_search_threshold = 0.0001f)
        {
            start = a_start;
            goal = a_goal;
            searcher = a_searcher;
            threshold = a_search_threshold;
			goalNode = graph.FindNode(goal, searcher, threshold);

            metadata = new Meta[graph.nodes.Count];
            frontier = new Queue<Graph<T>.Node>();

            var snode = graph.FindNode(start, searcher, threshold);
            for(int i = 0; i < metadata.Length; ++i)
                metadata[i] = new Meta();

            metadata[snode.uid].state = Meta.VisitState.frontier;
            frontier.Enqueue(snode);
        }

		public bool step()
        {
            var current = frontier.Dequeue();

            metadata[current.uid].state = Meta.VisitState.explored;

			// stop if we've reached the goal.
			if(current.uid == goalNode.uid)
            {
                return false;
            }
            
			foreach(var e in current.edges)
            {
                float g = e.weight + metadata[current.uid].g;
                uint  d = 1 + metadata[current.uid].depth;

                if (metadata[e.end.uid].state == Meta.VisitState.undiscovered)
				{
		            frontier.Enqueue(e.end);
	                metadata[e.end.uid].state = Meta.VisitState.frontier;
                }
                if (metadata[e.end.uid].state == Meta.VisitState.frontier)
                {
                    if(g < metadata[e.end.uid].g || metadata[e.end.uid].prev == null)
                    {
                        metadata[e.end.uid].prev = current;
                        metadata[e.end.uid].g = g;
                        metadata[e.end.uid].depth = d;
                    }
                }
            }
            return frontier.Count != 0;
        }
	}
}
