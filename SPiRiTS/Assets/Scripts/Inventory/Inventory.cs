using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// Handles all modifications to, and creations of, player inventory Dictionaries
/// </summary>
public class Inventory : MonoBehaviour
{
    // REFERENCES
    /// <summary>
    /// The core Inventory system is a List comprised of Item-int Dictionaries. Each player is "assigned"
    /// one of these Dictionaries within the list, with their "access key" being their assigned player index
    /// given by the Multiplayer Input Module (i.e. Player #1's "inventory" is located in items[playerIndex - 1])
    /// </summary>
    private List<Dictionary<Item, int>> items = new List<Dictionary<Item, int>>();

    /// <summary>
    /// OnItemChanged sends out a message to the InventoryUI class to update each player's HUD whenever it is invoked
    /// </summary>
    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    /// <summary>
    /// OnInventoryCleared messages InventoryUI to clear every player's HUD of all items
    /// </summary>
    public delegate void OnInventoryCleared();
    public OnInventoryCleared onInventoryCleared;

    // Singleton instance
    #region Singleton
    /// <summary>
    /// The Inventory singleton that all other classes will use when modifying a
    /// player's inventory Dictionary
    /// </summary>
    public static Inventory Instance { get; private set; }

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


    // FUNCTIONS
    /// <summary>
    /// Create a new empty Item-int Dictionary to the core Inventory list; happens whenever a new player is instantiated
    /// </summary>
    public void AddNewPlayerInventory()
    {
        items.Add(new Dictionary<Item, int>());
    }

    /// <summary>
    /// Clear out the core Inventory list; useful when resetting the level after going back to the Main Menu or the Player Select screen
    /// </summary>
    public void ResetInventory()
    {
        items.Clear();
        onInventoryCleared?.Invoke();
    }

    /// <summary>
    /// A getter for a specific player's inventory Dictionary
    /// </summary>
    /// <param name="playerIndex">What location in items to index; corresponds to the order in which the player logged in</param>
    /// <returns>An Item-int Dictionary of all that specific player's items and their amounts</returns>
    public Dictionary<Item, int> GetPlayerInventory(int playerIndex)
    {
        return items[playerIndex];
    }

    /// <summary>
    /// Increment an item in a certain player's Inventory Dictionary by amount or, if item was not
    /// in that Dictionary yet, create a new entry
    /// </summary>
    /// <param name="item">The item being incremented/created in the player's Dictionary</param>
    /// <param name="playerIndex">The index to find which specific player's Dictionary to modify</param>
    /// <param name="amount">How much to increment the item to, which is typically just one</param>
    public void Add(Item item, int playerIndex, int amount = 1)
    {
        // Don't add an item if it is a default item in the player Inventory
        if (!item.IsDefaultItem)
        {
            if (items[playerIndex].ContainsKey(item))
            {
                // Since the item is already in the player's Inventroy, just increment the item's count by amount
                items[playerIndex][item] += amount;
            }
            else
            {
                // Make a new entry in the Dictionary
                items[playerIndex].Add(item, amount);
            }

            Debug.Log($"Added: {amount} {item} to Player #{playerIndex}, confirmation - {items[playerIndex][item]}");
            onItemChanged?.Invoke();
        }
    }

    /// <summary>
    /// Remove a single item from a specific player's inventory Dictionary and, if that item's amount is now zero,
    /// remove it from the Dictionary
    /// </summary>
    /// <param name="item">The item to remove from the Dictionary</param>
    /// <param name="playerIndex">Where to index items, i.e. the player whose Dictionary we're accessing</param>
    public void Remove(Item item, int playerIndex)
    {
        // Check that the player's Dictionary actually has the item
        if (items[playerIndex].ContainsKey(item))
        {
            items[playerIndex][item] -= 1;

            Debug.Log($"Removed: One {item} from Player #{playerIndex}, confirmation - {items[playerIndex][item]}");

            if (items[playerIndex][item] == 0)
            {
                // Remove from Dictionary if none of this item are left
                items[playerIndex].Remove(item);
            }
     
            onItemChanged?.Invoke();
        }
    }
}
