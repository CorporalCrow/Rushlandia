using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject parent;
    private PoolableObject prefab;
    private int size;
    private List<PoolableObject> avaliableObjects;

    private ObjectPool(PoolableObject prefab, int size)
    {
        this.prefab = prefab;
        avaliableObjects = new List<PoolableObject>(size);
    }

    public static ObjectPool CreateInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);

        pool.parent = new GameObject(prefab.name + " Pool");
        pool.parent.tag = "Object Pool";
        pool.CreateObjects();

        return pool;
    }

    private void CreateObjects()
    {
        for (int i = 0; i < size; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
        poolableObject.Parent = this;
        poolableObject.gameObject.SetActive(false);
    }

    public PoolableObject GetObject()
    {
        if (avaliableObjects.Count == 0)
        {
            CreateObject();
        }

        PoolableObject instance = avaliableObjects[0];
        avaliableObjects.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        avaliableObjects.Add(poolableObject);
    }
}
