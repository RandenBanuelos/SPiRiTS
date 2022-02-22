using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Written by: Randen Banuelos
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// The HUD slot icon that is updated when a player's inventory Dictionary changes
/// </summary>
public class InventorySlot : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Image icon;
    [SerializeField] private Image background;

    /// <summary> 
    /// Current amount of this item in the player's inventory 
    /// </summary>
    [SerializeField] private TMP_Text amount;

    /// <summary> 
    /// Gives live countdown of cooldown for using this item 
    /// </summary>
    [SerializeField] private TMP_Text cooldownTimerText;


    // REFERENCES
    /// <summary> 
    /// The item this InventorySlot is showing information for 
    /// </summary>
    Item item;

    /// <summary> 
    /// Used for the cooldown timer 
    /// </summary>
    private float currentTimer = 0f;

    /// <summary> 
    /// Used for disabling item use while on cooldown 
    /// </summary>
    private bool canUse = true;

    /// <summary> 
    /// Checks if an update needs any text changing 
    /// </summary>
    private bool needsText = true;

    /// <summary> 
    /// Numeric representation of amount; useful for tracking how much 
    /// </summary>
    private int amountInt;

    /// <summary> 
    /// Cache for Inventory.Instance for more efficient computing 
    /// </summary>
    private Inventory inventory;


    // FUNCTIONS
    private void Start()
    {
        // Cache inventory
        inventory = Inventory.Instance;
    }

    private void Update()
    {
        // Check if this item is on cooldown
        if (!canUse)
        {
            // Decrease timer
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0f)
            {
                // End cooldown once the timer runs out
                EndCooldown();
            }
            else
            {
                // Don't update text if needsText is true
                if (needsText)
                    cooldownTimerText.text = currentTimer.ToString("F1");
            }
        }
    }
    
    // "Getter"
    public Item Item => item;

    /// <summary>
    /// Set a new item to this InventorySlot; can also be used for updating
    /// </summary>
    /// <param name="newItem">The item to set to this slot</param>
    /// <param name="count">How many of this item is in the player's inventory</param>
    public void AddItem(Item newItem, int count)
    {
        // TODO: Make a separate UpdateItem function for better efficiency when decrementing an item
        amountInt = count;
        amount.text = amountInt.ToString();

        item = newItem;

        icon.gameObject.SetActive(true);
        icon.sprite = newItem.Icon;
    }

    /// <summary>
    /// Clear out the slot and any item information
    /// </summary>
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.gameObject.SetActive(false);

        amount.text = "";
    }

    /// <summary>
    /// Use this item and either start the cooldown or remove it from the slot
    /// </summary>
    public void UseItem()
    {
        // Check that the item is not on cooldown and that the game is not paused (Playtest #1 Bug Fix)
        if (canUse && !PauseMenu.GameIsPaused)
        {
            // Don't use an item if none is set
            if (item != null)
            {
                item.Use();
                int remaining = amountInt - 1;

                // If this was not the last item left
                if (remaining > 0)
                {
                    // Remove one of this item from the player's inventory Dictionary
                    inventory.Remove(item, item.PlayerIndex);
                    StartCooldown();
                }
                else
                {
                    // Temporarily cache this item for computation in the event of a potential null reference
                    Item tempItem = item;
                    inventory.Remove(tempItem, tempItem.PlayerIndex);
                    RemovedItemCooldown();
                }
            }
        }
    }

    /// <summary>
    /// Initiate the cooldown for this item, visual modifying the InventorySlot
    /// </summary>
    private void StartCooldown()
    {
        // Disable item use
        canUse = false;

        // Gray out the slot
        icon.color = Color.gray;
        background.color = Color.gray;

        // Show updated amount
        amount.text = amountInt.ToString();

        // Set timer
        currentTimer = item.CooldownTimer;
        cooldownTimerText.text = currentTimer.ToString();
    }

    /// <summary>
    /// Stop cooldown for this item, resetting slot back to normal visuals
    /// </summary>
    private void EndCooldown()
    {
        // Enable item use
        canUse = true;

        // Want text to update, so set to true
        needsText = true;

        // Reset colors back to normal
        icon.color = Color.white;
        background.color = Color.white;

        // End timer
        currentTimer = 0f;
        cooldownTimerText.text = "";
    }

    /// <summary>
    /// Specialized cooldown when the item removed was the final one; needed to account for Unity's new Input System's multiple key firing errors
    /// </summary>
    private void RemovedItemCooldown()
    {
        // Disable item, since there is none left
        canUse = false;

        // Don't need to adjust text for a null slot
        needsText = false;

        // Set a negligibly small timer to block Unity from sending more UseItem actions
        currentTimer = .5f;
    }
}
