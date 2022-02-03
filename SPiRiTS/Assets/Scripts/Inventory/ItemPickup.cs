using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// Component for overworld collectibles
/// </summary>
public class ItemPickup : Interactable
{
    [SerializeField] private Item item;
    private GameObject modelRenderer;
    private GameObject vfxRenderer;

    private void Awake()
    {
        modelRenderer = Instantiate(item.Model, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation, gameObject.transform);
        vfxRenderer = Instantiate(item.VfxDrop, transform.position, transform.rotation, gameObject.transform);
    }

    public override void Interact(Transform player)
    {
        base.Interact(player);

        PickUp(player);
    }


    private void PickUp(Transform player)
    {
        int playerIndex = player.GetComponent<PlayerInputHandler>().GetPlayerIndex();
        item.PlayerIndex = playerIndex;
        item.Owner = player.GetComponent<Mover>();

        Inventory.Instance.Add(item, playerIndex);

        Destroy(gameObject);
    }
}
