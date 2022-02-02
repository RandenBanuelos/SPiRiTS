using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// The controller for all inventory-based UI elements, namely the inventory slots underneath each player's HUD
/// </summary>
public class InventoryUI : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Houses all the inventory slots as children objects
    /// </summary>
    [SerializeField] private Transform itemsParent;


    // REFERENCES
    /// <summary>
    /// Cache for Inventory.Instance for more efficient computing
    /// </summary>
    private Inventory inventory;

    /// <summary>
    /// Tracks the current player whose HUD is being modified
    /// </summary>
    private int playerIndex;

    /// <summary>
    /// Cache of all five InventorySlots a player has
    /// </summary>
    private InventorySlot[] slots;

    
    // FUNCTIONS
    private void Start()
    {
        // Cache inventory and subscribe to Inventory's delegates
        inventory = Inventory.Instance;
        inventory.onItemChanged += UpdateUI;
        inventory.onInventoryCleared += ClearAllSlots;

        // Cache the slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // "Getter"
    public InventorySlot[] Slots => slots;

    /// <summary>
    /// "Setter" for InventoryUI.playerIndex
    /// </summary>
    /// <param name="index">Index to set playerIndex to</param>
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    /// <summary>
    /// Main function for updating each inventory slot in a select player's HUD
    /// </summary>
    private void UpdateUI()
    {
        // Cache the current player's inventory
        Dictionary<Item, int> playerInventory = inventory.GetPlayerInventory(playerIndex);

        // Iterate through all five inventory slots
        for (int i = 0; i < slots.Length; i++)
        {
            // Cache the current InventorySlot
            InventorySlot currentSlot = slots[i];

            // Check that the player has enough items for this slot (i.e. if this is Slot #3, the player needs at least three items in their inventory Dictionary)
            if (i < playerInventory.Count)
            {
                // Get the i-th Item in the player's inventory
                Item key = playerInventory.Keys.ToArray()[i];
                currentSlot.AddItem(key, playerInventory[key]);
            }
            else
            {
                currentSlot.ClearSlot();
            }
        }
    }

    /// <summary>
    /// Clear out all UI within the player's InventorySlots and unsubscribe from Inventory's delegates
    /// </summary>
    public void ClearAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].ClearSlot();

        inventory.onItemChanged -= UpdateUI;
        inventory.onInventoryCleared -= ClearAllSlots;
    }
}
