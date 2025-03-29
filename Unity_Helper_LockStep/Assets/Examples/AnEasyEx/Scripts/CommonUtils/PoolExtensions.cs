using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class PoolObjectExtension
    {
        public static void RecycleToPool(this object obj)
        {
            ObjectPool.Instance.Recycle(ref obj);
        }

        public static void RecycleListToPool<T>(this List<T> list)
        {
            foreach (var obj in list)
            {
                obj.RecycleToPool();
            }
            list.Clear();
            list.RecycleToPool();
        }
    }
}