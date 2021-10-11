using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class PoolProvider
    {
        public Pooling CreateBubblePool(IPool pool, string name, int amount)
        {
            var go = new GameObject();
            go.name = name;
            var p = new Pooling(go.transform);
            p.CreatePool(pool, amount);

            return p;
        }
    }
}