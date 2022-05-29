using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Base : MonoBehaviour
{
    [Tooltip("Gameobject to spawn as PoolItem.")]
    [SerializeField]
    private PoolingItemBase PoolItemPrefab;

    private int _poolSize;
    /// <summary>
    /// How many items will be spawned.
    /// </summary>
    public int PoolSize
    {
        get { return _poolSize; }
        set { _poolSize = value; }
    }

    /// <summary>
    /// Powerups that have been spawned but not been used in the game
    /// </summary>
    private List<PoolingItemBase> ActivePoolItems;
    /// <summary>
    /// Powerups that are used in the game and can be picked up by the player
    /// </summary>
    private List<PoolingItemBase> InactivePoolItems;

    /// <summary>
    /// Parentobject to have a clean Hirarchy
    /// </summary>
    private GameObject _poolParentObject;

    /// <summary>
    /// Spawns _poolsize amount of Powerup prefabs
    /// </summary>
    /// <param name="_clearPreviousPool"> if you want to clear the previous pool (if there is one)</param>
    public void SpawnPoolItems(bool _clearPreviousPool)
    {
        //clear the previous pool
        if (_clearPreviousPool == true)
        {
            ClearPreviousPool();
        }

        if (_poolSize <= 0)
        {
            Debug.LogError("The Pool size is <= 0. Please set a pool size  before spawning items. - Pool_Base.cs");
            return;
        }

        if (PoolItemPrefab == null)
        {
            Debug.LogError("No Pool item prefab has been assigned. Please assign one before spawning items. Pool_Base.cs");
            return;
        }

        CheckPoolParent();

        //check if the list has been initialized..
        if (InactivePoolItems == null)
        {
            //.. if no initialize it
            InactivePoolItems = new List<PoolingItemBase>();
        }

        //spawn items
        for (int i = 0; i < _poolSize; i++)
        {
            PoolingItemBase item = Instantiate(PoolItemPrefab, _poolParentObject.transform);
            SetupPoolItem(item);
            InactivePoolItems.Add(item);
        }
    }

    /// <summary>
    /// Cleans up all Active and Inactive items
    /// </summary>
    public void ClearPreviousPool()
    {
        //check active list for items to add to the inactive list to clean up all items
        CheckActivePoolForItems();

        //check if the powerupsPool list is not null and not empty
        if (InactivePoolItems != null && InactivePoolItems.Count > 0)
        {
            //iterate the list..
            for (int i = 0; i < _poolParentObject.transform.childCount; i++)
            {
                //destroy the Objects
                DestroyImmediate(_poolParentObject.transform.GetChild(i));
            }
        }

        //make new lists
        ActivePoolItems = new List<PoolingItemBase>();
        InactivePoolItems = new List<PoolingItemBase>();
    }

    /// <summary>
    /// Checks if there are any objects in the Active list and adds them to the inactive list for cleanup
    /// </summary>
    private void CheckActivePoolForItems()
    {
        //check if we have any active powerups
        if (ActivePoolItems != null && InactivePoolItems.Count > 0)
        {
            //check if the InactivePool list is null..
            if (InactivePoolItems == null)
            {
                //.. initialize it
                InactivePoolItems = new List<PoolingItemBase>();
            }

            //if yes we iterate the list
            foreach (PoolingItemBase item in ActivePoolItems)
            {
                //.. check if the item is not null..
                if (item != null)
                {
                    //add it to the pool list
                    InactivePoolItems.Add(item);
                }
            }
            //reset the list
            ActivePoolItems = null;
        }
    }

    /// <summary>
    /// checks if there already is a Pool parent and if not then creates and names it.
    /// </summary>
    private void CheckPoolParent()
    {
        if (_poolParentObject == null)
        {
            _poolParentObject = new GameObject();
            _poolParentObject.transform.SetParent(transform);

            if (PoolItemPrefab != null)
            {
                _poolParentObject.name = PoolItemPrefab.name + "'s Pool";
            }
        }
    }

    /// <summary>
    /// Getting an item from the pool
    ///Checking Inactive list and spawning a new item if necessery
    /// </summary>
    /// <returns>An Item of the type that is being pooled.</returns>
    public PoolingItemBase GetPoolItem()
    {
        PoolingItemBase item = null;

        item = GetItemFromInactiveList();

        if (item == null)
        {
            item = SpawnNewPoolingItem();
        }

        return item;
    }

    private PoolingItemBase GetItemFromInactiveList()
    {
        PoolingItemBase item = null;
        if (InactivePoolItems != null && InactivePoolItems.Count > 0)
        {
            for (int i = 0; i < InactivePoolItems.Count; i++)
            {
                item = InactivePoolItems[i];
                if (item != null)
                {
                    ActivePoolItems.Add(item);
                    InactivePoolItems[i] = null;
                    break;
                }
            }
        }
        return item;
    }
    private PoolingItemBase SpawnNewPoolingItem()
    {
        if (PoolItemPrefab == null) return null;

        CheckPoolParent();

        PoolingItemBase item = Instantiate(PoolItemPrefab, _poolParentObject.transform);
        SetupPoolItem(item);
        ActivePoolItems.Add(item);

        return item;
    }

    private void SetupPoolItem(PoolingItemBase _item)
    {
        _item.gameObject.name = PoolItemPrefab.gameObject.name;
        _item.Initialize(this);
    }
    
    /// <summary>
    /// Returns an item back to the pool
    /// </summary>
    /// <param name="_item">Item to be returned</param>
    public void ReturnItem(PoolingItemBase _item)
    {
        if (ActivePoolItems != null && ActivePoolItems.Count > 0)
        {
            int i = ActivePoolItems.FindIndex(x => _item);
            InactivePoolItems.Add(ActivePoolItems[i]);

            ActivePoolItems[i].transform.position = _poolParentObject.transform.position;

            ActivePoolItems[i] = null;            
        }
    }
}