using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool : Singleton<ObjectPool>
{
    private const int SinglePoolMaxCount = 1000;
    private readonly Dictionary<Type, Queue<object>> _pool = new();

    protected override void OnDispose()
    {
        _pool.Clear();
    }

    public T Get<T>() where T : new()
    {
        if (!_pool.TryGetValue(typeof(T), out var queue))
        {
            return Activator.CreateInstance<T>();
        }

        return queue.Count == 0 ? Activator.CreateInstance<T>() : (T)queue.Dequeue();
    }

    public object Get(Type type)
    {
        if (!_pool.TryGetValue(type, out var queue))
        {
            return Activator.CreateInstance(type);
        }

        return queue.Count == 0 ? Activator.CreateInstance(type) : queue.Dequeue();
    }

    public void Recycle(ref object obj)
    {
        var type = obj.GetType();
        if (!_pool.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _pool.Add(type, queue);
        }

        if (queue.Count > SinglePoolMaxCount)
        {
            obj = null;
            return;
        }

        queue.Enqueue(obj);
        obj = null;
    }

    public void InitCapacity<T>(int count)
    {
        var type = typeof(T);
        if (!_pool.TryGetValue(typeof(T), out var queue))
        {
            queue = new Queue<object>();
            _pool.Add(type, queue);
        }

        var initCount = Math.Min(count, SinglePoolMaxCount);

        if (queue.Count >= initCount)
        {
            return;
        }

        for (var i = 0; i < initCount - queue.Count; i++)
        {
            queue.Enqueue(Activator.CreateInstance<T>());
        }
    }
}