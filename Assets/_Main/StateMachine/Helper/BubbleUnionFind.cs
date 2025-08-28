using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    /// <summary>
    /// Disjoint Set Union (DSU) algorithm
    /// </summary>
    public class BubbleUnionFind
    {
        private readonly Dictionary<Bubble, Bubble> parent = new();
        private readonly Dictionary<Bubble, int> rank = new();

        /// <summary>
        /// Find representative (with path compression).
        /// </summary>
        public Bubble Find(Bubble bubble)
        {
            // Lazy registration
            Add(bubble);

            if (!parent[bubble].Equals(bubble))
            {
                // Path compression
                parent[bubble] = Find(parent[bubble]); 
            }

            return parent[bubble];
        }

        /// <summary>
        /// Adds a bubble to the Union-Find structure if not already present.
        /// </summary>
        private void Add(Bubble bubble)
        {
            if (!parent.TryAdd(bubble, bubble))
            {
                return;
            }

            // self parent, rank = 0 for new set
            rank[bubble] = 0; 
        }
        
        /// <summary>
        /// Union two bubbles into the same set.
        /// </summary>
        public void Union(Bubble a, Bubble b)
        {
            Bubble rootA = Find(a);
            Bubble rootB = Find(b);

            if (rootA.Equals(rootB))
            {
                // already connected
                return;
            }

            // Union by rank
            if (rank[rootA] < rank[rootB])
            {
                parent[rootA] = rootB;
                return;
            }

            if (rank[rootA] > rank[rootB])
            {
                parent[rootB] = rootA;
                return;
            }

            parent[rootB] = rootA;
            rank[rootA]++;
        }
        
        public bool Connected(Bubble a, Bubble b)
        {
            return Find(a).Equals(Find(b));
        }
    }
}