using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] private string itemName = "";
    [SerializeField] private string description = "";
    [SerializeField] private Sprite icon = null;
    [SerializeField] private float cooldownTimer = 1f;
    [SerializeField] private bool isDefaultItem = false;

    private int playerIndex = 0;
    private Mover owner;


    public string ItemName => itemName;

    public string Description => description;

    public Sprite Icon => icon;

    public bool IsDefaultItem => isDefaultItem;

    public float CooldownTimer => cooldownTimer;

    public int PlayerIndex => playerIndex;

    public Mover Owner => owner;

    public virtual void Use()
    {
        // Use an item, potentially causing something to happen
        Debug.Log("Using a(n) " + itemName);
    }


    public void Remove()
    {
        Inventory.Instance.Remove(this, playerIndex);
    }


    public void SetIndex(int index)
    {
        playerIndex = index;
    }


    public void SetPlayerOwner(Mover mover)
    {
        owner = mover;
    }
}
