using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Documentation by: Matthew Jung

/// <summary>
/// Handles equipment and unequipment of all equipment for a player
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's inventory
    /// </summary>
    private Inventory inventory;

    /// <summary>
    /// Array of all equipment currently equipped by the player
    /// </summary>
    private Equipment[] currentEquipment;

    /// <summary>
    /// OnEquipmentChanged sends out a message with the new item and the item it replaced
    /// </summary>
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    // Singleton instance
    #region Singleton

    public static EquipmentManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("EQUIPMENT_MANAGER SINGLETON - Trying to create another instance of singleton!");
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
    /// Gets the reference to the inventory reference, and creates the currentEquipment array
    /// </summary>
    private void Start()
    {
        inventory = Inventory.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }


    /// <summary>
    /// Handles the equipment of one item
    /// </summary>
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.EquipSlot;

        Equipment oldItem = null;

        // If the equipment slot is already taken:
        if (currentEquipment[slotIndex] != null)
        {
            // Save the item as oldItem and move it to the inventory, replacing the new item's spot in the inventory
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem, newItem.PlayerIndex);
        }

        // Passes both items ot the onEquipmentChanged delegate
        onEquipmentChanged?.Invoke(newItem, oldItem);

        // Update the equipment slot with the new item
        currentEquipment[slotIndex] = newItem;
    }


    /// <summary>
    /// Handles the unequipment of one item, given its slot index and the index it will take up in the inventory
    /// </summary>
    public void Unequip(int slotIndex, int pIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            // Stores the item being unequipped, and adds it to the inventory
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem, pIndex);

            // Replaces the equipment slot with null
            currentEquipment[slotIndex] = null;

            // Passes both items to the delegate
            onEquipmentChanged?.Invoke(null, oldItem);
        }
    }


    /// <summary>
    /// Loops through all equipment and passes them all to Unequip
    /// </summary>
    public void UnequipAll(int pIndex)
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i, pIndex);
        }
    }
}