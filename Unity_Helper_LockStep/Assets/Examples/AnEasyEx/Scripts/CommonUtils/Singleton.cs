using System;

public abstract class Singleton<T> : IDisposable where T : class, new()
{
    private static T _instance;

    public static T Instance 
    { 
        get 
        {
            _instance ??= new T();
            return _instance; 
        } 
    }

    public virtual void Dispose()
    {
        OnDispose();
        _instance = null;
    }

    protected virtual void OnDispose() { }
}
