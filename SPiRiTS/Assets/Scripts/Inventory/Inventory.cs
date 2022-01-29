using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Dictionary<Item, int>> items = new List<Dictionary<Item, int>>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public delegate void OnInventoryCleared();
    public OnInventoryCleared onInventoryCleared;

    // Singleton instance
    #region Singleton
    public static Inventory Instance { get; private set; }


    // Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("INVENTORY_MANAGER SINGLETON - Trying to create another instance of singleton!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    #endregion

    public void AddNewPlayerInventory()
    {
        items.Add(new Dictionary<Item, int>());
    }


    public void ResetInventory()
    {
        items.Clear();
        onInventoryCleared?.Invoke();
    }


    public void Add(Item item, int playerIndex, int amount = 1)
    {
        if (!item.IsDefaultItem)
        {
            if (items[playerIndex].ContainsKey(item))
            {
                items[playerIndex][item] += amount;
            }
            else
            {
                items[playerIndex].Add(item, amount);
            }

            Debug.Log($"Added: {amount} {item} to Player #{playerIndex}, confirmation - {items[playerIndex][item]}");
            onItemChangedCallback?.Invoke();
        }
    }


    public void Remove(Item item, int playerIndex)
    {
        if (items[playerIndex].ContainsKey(item))
        {
            items[playerIndex][item] -= 1;

            Debug.Log($"Removed: One {item} from Player #{playerIndex}, confirmation - {items[playerIndex][item]}");

            if (items[playerIndex][item] == 0)
            {
                items[playerIndex].Remove(item);
            }
     
            onItemChangedCallback?.Invoke();
        }
    }


    public Dictionary<Item, int> GetPlayerInventory(int playerIndex)
    {
        return items[playerIndex];
    }
}
