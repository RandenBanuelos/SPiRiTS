using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;

    public override void Interact(Transform player)
    {
        base.Interact(player);

        PickUp(player);
    }


    private void PickUp(Transform player)
    {
        int playerIndex = player.GetComponent<PlayerInputHandler>().GetPlayerIndex();
        item.SetIndex(playerIndex);
        item.SetPlayerOwner(player.GetComponent<Mover>());

        Inventory.Instance.Add(item, playerIndex);

        Destroy(gameObject);
    }
}
