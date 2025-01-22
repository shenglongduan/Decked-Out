﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour , IGameModule
{
    [Serializable]
    public class PooledObject
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }
    public List<PooledObject> objectsToPool = new List<PooledObject>();

    public bool IsInitialized { get { return _isInitialized; } }
    private bool _isInitialized = false;
    private readonly Dictionary<string, List<GameObject>> _objectPoolByName = new Dictionary<string, List<GameObject>>();

    #region IGameModule Implementation
    public IEnumerator LoadModule()
    {
        Debug.Log("Loading Object Pool");
        InitializePool();
        yield return new WaitUntil(() => { return IsInitialized; });

        ServiceLocator.Register<ObjectPoolManager>(this);
        yield return null;
    }
    #endregion

    public GameObject GetObjectFromPool(string poolName)
    {
        GameObject ret = null;
        if (_objectPoolByName.ContainsKey(poolName))
        {
            ret = GetNextObject(poolName);
        }
        else
        {
            // No pool found by that name
            Debug.LogError("No Pool Exists With Name: " + poolName);
        }
        return ret;
    }

    public void RecycleObject(GameObject go)
    {
        go.SetActive(false);
    }

    private void InitializePool()
    {
        GameObject PoolManagerGO = new GameObject("Object Pool");
        PoolManagerGO.transform.SetParent(GameLoader.SystemsParent);
        foreach (PooledObject poolObj in objectsToPool)
        {
            if (!_objectPoolByName.ContainsKey(poolObj.name))
            {
                Debug.Log(string.Format("Creating Pool: {0} Size: {1}", poolObj.name, poolObj.poolSize));
                GameObject poolGO = new GameObject(poolObj.name);
                poolGO.transform.SetParent(PoolManagerGO.transform);
                _objectPoolByName.Add(poolObj.name, new List<GameObject>());
                for (int i = 0; i < poolObj.poolSize; ++i)
                {
                    GameObject go = Instantiate(poolObj.prefab);
                    go.name = string.Format("{0}_{1:000}", poolObj.name, _objectPoolByName[poolObj.name].Count);
                    go.transform.SetParent(poolGO.transform);
                    go.SetActive(false);
                    _objectPoolByName[poolObj.name].Add(go);
                }
            }
            else
            {
                Debug.Log("WARNING: Attempting to create multiple pools with the same name: " + poolObj.name);
                continue;
            }
        }

        _isInitialized = true;
    }

    private GameObject GetNextObject(string poolName)
    {
        List<GameObject> pooledObjects = _objectPoolByName[poolName];
        foreach (GameObject go in pooledObjects)
        {
            if (go == null)
            {
                Debug.LogError("Pooled Object Is NULL");
                continue;
            }

            if (go.activeInHierarchy)
            {
                continue; // keep looking
            }
            else
            {
                return go;
            }
        }

        Debug.LogError("Object Pool Depleted: No Unused Objects To Return");
        return null;
    }
}