using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;

    private Inventory inventory;
    private int playerIndex;
    private InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;
        inventory.onInventoryCleared += ClearAllSlots;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }


    public InventorySlot[] Slots => slots;


    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }


    private void UpdateUI()
    {
        Debug.Log($"Updating UI for Player #{playerIndex + 1}...");

        Dictionary<Item, int> playerInventory = inventory.GetPlayerInventory(playerIndex);

        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot currentSlot = slots[i];

            if (i < playerInventory.Count)
            {
                Item key = playerInventory.Keys.ToArray()[i];
                currentSlot.AddItem(key, playerInventory[key]);
            }
            else
            {
                currentSlot.ClearSlot();
            }
        }
    }


    public void ClearAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].ClearSlot();

        inventory.onItemChangedCallback -= UpdateUI;
        inventory.onInventoryCleared -= ClearAllSlots;
    }
}
