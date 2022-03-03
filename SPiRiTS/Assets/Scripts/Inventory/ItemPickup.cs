using UnityEngine;

// Written by: Randen Banuelos
// Documentation by: Matthew Jung
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// Component for overworld collectibles
/// </summary>
public class ItemPickup : Interactable
{
    /// <summary>
    /// Stores the item to be picked up
    /// </summary>
    [SerializeField] private Item item;

    /// <summary>
    /// Stores the model for the item
    /// </summary>
    private GameObject modelRenderer;

    /// <summary>
    /// Stores the VFX for the item
    /// </summary>
    private GameObject vfxRenderer;

    /// <summary>
    /// Instantiates the model and VFX for the item
    /// </summary>
    private void Awake()
    {
        modelRenderer = Instantiate(item.Model, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation, gameObject.transform);
        vfxRenderer = Instantiate(item.VfxDrop, transform.position, transform.rotation, gameObject.transform);
    }

    /// <summary>
    /// Overrides the Interact in the base class, picks up item when interacted with
    /// </summary>
    public override void Interact(Transform player)
    {
        base.Interact(player);

        PickUp(player);
    }

    /// <summary>
    /// Puts the item in the player's inventory and removes it from the scene
    /// </summary>
    private void PickUp(Transform player)
    {
        int playerIndex = player.GetComponent<PlayerInputHandler>().GetPlayerIndex();
        item.PlayerIndex = playerIndex;
        item.Owner = player.GetComponent<Mover>();

        Inventory.Instance.Add(item, playerIndex);

        Destroy(gameObject);
    }
}