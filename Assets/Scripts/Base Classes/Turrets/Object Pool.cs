using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] GameObject objectToPool;
    [SerializeField] int amountToPool;

    #endregion
    
    public List<GameObject> pooledObjects;
    
    void Start()
    {
        SetObjectToPool();
    }

    public void SetObjectToPool()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            //tmp.transform.SetParent(transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    void OnDestroy()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            Destroy(pooledObjects[i]);
        }
    }
}
