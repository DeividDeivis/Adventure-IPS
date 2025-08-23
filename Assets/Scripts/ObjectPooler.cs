using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{    
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;


    #region Singleton
    static private ObjectPooler instance;
    static public ObjectPooler Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        #region Fill Dictionary
        foreach (Pool pool in Pools)
        {
            var poolContainer = transform.Find(pool.Tag);

            if (!poolContainer)
            {
                poolContainer = new GameObject(pool.Tag).transform;
                poolContainer.SetParent(transform, true);
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, poolContainer.transform);
                obj.GetComponent<IPoolable>().PoolTag = pool.Tag;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag, objectPool);
        }
        #endregion

    }
    #endregion

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No existe pool con el tag: " + tag);
            return null;
        }
        if (PoolDictionary[tag].Count <= 0)
        {
            //Debug.LogWarning("No hay " + tag + " disponibles");
            foreach (Pool pool in Pools)
            {
                var poolContainer = transform.Find(pool.Tag);

                if (!poolContainer)
                {
                    poolContainer = new GameObject(pool.Tag).transform;
                    poolContainer.SetParent(transform, true);
                }
                if (pool.Tag == tag)
                {
                    GameObject toSpawn = Instantiate(pool.Prefab, poolContainer);
                    toSpawn.GetComponent<IPoolable>().PoolTag = pool.Tag;
                    toSpawn.SetActive(false);
                    PoolDictionary[tag].Enqueue(toSpawn);
                    Debug.Log("se creo un nuevo prefab");
                    break;
                }
            }
        }
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }

    public GameObject SpawnFromPoolToParent(Transform parent, string tag)
    {
        if(tag == "")
        {
            return null;
        }
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No existe pool con el tag: " + tag);
            return null;
        }
        if (PoolDictionary[tag].Count <= 0)
        {
            //Debug.LogWarning("No hay " + tag + " disponibles");
            foreach (Pool pool in Pools)
            {
                var poolContainer = transform.Find(pool.Tag);

                if (!poolContainer)
                {
                    poolContainer = new GameObject(pool.Tag).transform;
                    poolContainer.SetParent(transform, true);
                }
                if (pool.Tag == tag)
                {
                    GameObject toSpawn = Instantiate(pool.Prefab, poolContainer);
                    toSpawn.SetActive(false);
                    PoolDictionary[tag].Enqueue(toSpawn);
                    Debug.Log("se creo un nuevo prefab");
                    break;
                }
            }
        }
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(parent, false);

        return objectToSpawn;
    }

    public void Enqueue(string tag, GameObject toEnqueue)
    {
        if (!PoolDictionary[tag].Contains(toEnqueue))
        {
            var poolContainer = transform.Find(tag);
            if (!poolContainer)
            {
                poolContainer = new GameObject(tag).transform;
                poolContainer.SetParent(transform, true);
            }
            toEnqueue.transform.SetParent(poolContainer, false);
            toEnqueue.SetActive(false);
            PoolDictionary[tag].Enqueue(toEnqueue);
        }
    }

    /// <summary>
    /// Deactivates all elements of a certain type
    /// </summary>
    /// <param name="tag">Tag that indicates the kind of element that will be deactivated</param>
    public void DeactivateAll(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No existe pool con el tag: " + tag);
            return;
        }
        else
        {
            var poolContainer = transform.Find(tag);
            foreach (Transform child in poolContainer)
            {
                //child.gameObject.SetActive(false);
                if(tag!=null)
                {
                    child.GetComponent<IPoolable>().ReturnToPool();
                }
                    //Enqueue(tag, child.gameObject);
            }

        }
    }

    public void ClearPoolObjects()
    {
        throw new NotImplementedException();
    }

}
[System.Serializable]
public class Pool
{
    public string Tag;
    public GameObject Prefab;
    public int Size;
}