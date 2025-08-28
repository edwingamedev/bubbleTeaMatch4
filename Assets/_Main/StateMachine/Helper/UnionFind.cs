using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    /// <summary>
    /// Disjoint Set Union (DSU) algorithm
    /// </summary>
    public class UnionFind<T>
    {
        private readonly Dictionary<T, T> parent = new();
        private readonly Dictionary<T, int> rank = new();

        /// <summary>
        /// Union two bubbles into the same set.
        /// </summary>
        public void Union(T a, T b)
        {
            T rootA = Find(a);
            T rootB = Find(b);

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
        
        /// <summary>
        /// Find representative (with path compression).
        /// </summary>
        public T Find(T node)
        {
            // Lazy registration
            Add(node);

            if (!parent[node].Equals(node))
            {
                // Path compression
                parent[node] = Find(parent[node]); 
            }

            return parent[node];
        }

        /// <summary>
        /// Adds a bubble to the Union-Find structure if not already present.
        /// </summary>
        private void Add(T node)
        {
            if (!parent.TryAdd(node, node))
            {
                return;
            }

            // self parent, rank = 0 for new set
            rank[node] = 0; 
        }
        
        public bool Connected(T a, T b)
        {
            return Find(a).Equals(Find(b));
        }
    }
}