using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private Inventory inventory;
    private Equipment[] currentEquipment;

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
    private void Start()
    {
        inventory = Inventory.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }


    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.EquipSlot;

        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem, newItem.PlayerIndex);
        }

        onEquipmentChanged?.Invoke(newItem, oldItem);

        currentEquipment[slotIndex] = newItem;
    }


    public void Unequip(int slotIndex, int pIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem, pIndex);

            currentEquipment[slotIndex] = null;

            onEquipmentChanged?.Invoke(null, oldItem);
        }
    }


    public void UnequipAll(int pIndex)
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i, pIndex);
        }
    }
}
