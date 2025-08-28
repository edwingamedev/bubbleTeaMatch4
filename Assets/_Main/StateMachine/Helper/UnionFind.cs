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
        /// Union two nodes into the same set.
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

            // parent is not itself
            if (!parent[node].Equals(node))
            {
                // Path compression
                parent[node] = Find(parent[node]); 
            }

            return parent[node];
        }

        /// <summary>
        /// Adds a node to the Union-Find structure if not already present.
        /// </summary>
        private void Add(T node)
        {
            if (parent.ContainsKey(node))
            {
                return;
            }

            parent[node] = node;  // self parent
            rank[node] = 0;       // rank = 0 for new set
        }
        
        public bool Connected(T a, T b)
        {
            return Find(a).Equals(Find(b));
        }
    }
}