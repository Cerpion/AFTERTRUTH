using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public  class ServiceLocator 
{
    public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());
    private static ServiceLocator _instance;

    private readonly Dictionary<Type, object> _services;
    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void RegisterServices<T>(T services)
    {
        var type = typeof(T);
        Assert.IsFalse(_services.ContainsKey(type), $"Servicio {type} esta registrado");

        _services.Add(type, services);
    }
    public void UnregisterService<T>()
    {
        var type = typeof(T);
        Assert.IsTrue(_services.ContainsKey(type),
                       $"Service {type} is not registered");

        _services.Remove(type);
    }
    public T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception($"Servicio{type} no se encontro");
        }

        return (T)service;
    }


    public bool Contains<T>()
    {
        var type = typeof(T);
        return _services.ContainsKey(type);
    }
}
