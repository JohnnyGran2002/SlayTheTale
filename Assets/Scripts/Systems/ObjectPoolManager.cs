using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _cardsEmpty;
    private static GameObject _particleSystemsEmpty;
    //each one is basically a folder for created GameObjects
    public enum PoolType
    {
        Cards,
        ParticleSystem,
        none
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        //an empty game object containing all pool types
        SetupEmpties();
    }

    //create a GameObject for each variable
    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");
        
        _cardsEmpty = new GameObject("Cards");
        _cardsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _particleSystemsEmpty = new GameObject("Particles");
        _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    //spawn in the correct game object at the correct position(used to replace instantiate method)
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.none)
    {
        //search through all ObjectPools and return the one with the where the LookupString from the PooledObjectInfo have matching name with the object to spawn in
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == objectToSpawn.name)
            {
                pool = p;
                break;
            }
        }

        //if unable to find pool, create one
        if (pool == null)
        {
            //setup the LookupString so that the pool can be found the next time
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            //add the pool to the list of pools
            ObjectPools.Add(pool);
        }

        //check for inactive objects in pool(fi there is something in the pool that can be used)
        GameObject spawnableObject = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            // if it finds one, assign it to spawnableObject
            if (obj != null)
            {
                spawnableObject = obj;
                break;
            }
        }

        
        if (spawnableObject == null)
        {
            //find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);

            //check if anything was found in the list of inactive GameOBjects, if no inactive objects was found, create a new one
            spawnableObject = Instantiate(spawnableObject, spawnPosition, spawnRotation);

            //if not null, parent spawnableObject
            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            //if inactive GameOject was found, reactivate it
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            //remove it from the list of inactive objectes
            pool.InactiveObjects.Remove(spawnableObject);
            //make the object active
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    //deactivate object and return it to pool(called in place of destroy call)
    public static void ReturnObjectToPool(GameObject gameObject)
    {
        //remove the word (Clone) by removing the 7 last words from the name of the object being passed in
        string gameObjectName = gameObject.name.Substring(0, gameObject.name.Length - 7);

        //find a pool where its LookupString matches the name of GameObject that is being passed in
        PooledObjectInfo pool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookupString == gameObjectName)
            {
                pool = p;
                break;
            }
        }

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + gameObject.name);
        }
        else
        {
            //deactivate the object and add it to pool
            gameObject.SetActive(false);
            pool.InactiveObjects.Add(gameObject);
        }
    }

    //add to the correct pool
    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Cards:
                return _cardsEmpty;

            case PoolType.ParticleSystem:
                return _particleSystemsEmpty;

            case PoolType.none:
                return null;

            default:
                return null;
        }
    }
}

//every one of these is a pool of objects
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
