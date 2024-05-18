using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    // Cache
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

    #region Resource Load
    public T Load<T>(string path) where T : Object
    {
        if (_resources.TryGetValue(path, out Object resource))
        {
            return resource as T;
        }

        T r = Resources.Load<T>(path);
        _resources.Add(path, r);

        return r;
    }

    public GameObject Instantiate(string path, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = Object.Instantiate(prefab, parent);

        go.name = prefab.name;
        return go;
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (Managers.Pool.Push(go))
            return;

        Object.Destroy(go);
    }

    #endregion
 
}
