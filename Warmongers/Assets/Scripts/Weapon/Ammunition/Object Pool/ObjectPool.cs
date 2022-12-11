using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject Prefab;
    private List<PoolableObject> AvaliableObjects;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        AvaliableObjects = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        GameObject poolObject = new GameObject(Prefab.name + " Pool");
        poolObject.gameObject.tag = "Object Pool";
        pool.CreateObjects(poolObject.transform, Size);

        return pool;
    }

    private void CreateObjects(Transform parent, int Size)
    {
        for (int i = 0; i < Size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false);
        }
    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        AvaliableObjects.Add(poolableObject);
    }

    public PoolableObject GetObject()
    {
        if (AvaliableObjects.Count > 0)
        {
            PoolableObject instance = AvaliableObjects[0];
            AvaliableObjects.RemoveAt(0);

            instance.gameObject.SetActive(true);

            return instance;
        }
        else
        {
            return null;
        }
    }
}
