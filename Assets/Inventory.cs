using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject _objectInInventory;

    public static Inventory Instance;

    private void Start()
        {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        }

    public void AddToInventory(GameObject item)
        {
        _objectInInventory = item;
        _objectInInventory.SetActive(false);
        }

    internal GameObject GetItem()
        {
        return _objectInInventory;
        }

    public GameObject RemoveCurrItem()
        {
        GameObject buffer = _objectInInventory;
        _objectInInventory.SetActive(true);
        _objectInInventory = null;
        return buffer;
        }

    public bool HasItem()
        {
        return _objectInInventory != null;
        }
    }
