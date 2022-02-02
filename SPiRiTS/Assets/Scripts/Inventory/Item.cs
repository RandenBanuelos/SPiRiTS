using UnityEngine;

// Written by: Randen Banuelos

/// <summary>
/// Base class for specialized Item classes (e.g. HealingItem)
/// </summary>
public class Item : ScriptableObject
{
    // VARIABLES
    [Header("Basic Item Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private string description = "";

    /// <summary>
    /// Determines how long it takes before a player can use another one of this item, if they have more than one of them
    /// </summary>
    [SerializeField] private float cooldownTimer = 1f;

    /// <summary>
    /// True if the player loads into a new level with this item in their Inventory dictionary by default
    /// </summary>
    [SerializeField] private bool isDefaultItem = false;

    [Header("UI")]
    [SerializeField] private Sprite icon = null;

    [Header("Visuals")]
    [SerializeField] private GameObject model = null;
    [SerializeField] private GameObject vfxDrop = null;


    // REFERENCES
    /// <summary>
    /// The "player number" that holds this item (e.g. Player #1); very useful for adding to Inventory dictionaries
    /// </summary>
    private int playerIndex = 0;

    /// <summary>
    /// A link to the player object that "owns" this Item; useful for calculations in Inventory.cs
    /// </summary>
    private Mover owner;


    // FUNCTIONS
    // "Getters"
    public string ItemName => itemName;

    public string Description => description;

    public Sprite Icon => icon;

    public GameObject Model => model;

    public GameObject VfxDrop => vfxDrop;

    public bool IsDefaultItem => isDefaultItem;

    public float CooldownTimer => cooldownTimer;

    // "Getters"/"Setters"
    public int PlayerIndex
    {
        get => playerIndex;
        set => playerIndex = value;
    }

    public Mover Owner
    {
        get => owner;
        set => owner = value;
    } 

    /// <summary>
    /// A generalized Use function that is overridden depending on the subclass (e.g. HealingItem uses Use() to restore player health)
    /// </summary>
    public virtual void Use()
    {

    }

    /// <summary>
    /// Removes one of this Item from a player's dictionary of items (determined by playerIndex) in the Inventory Instance singleton
    /// </summary>
    public void Remove()
    {
        Inventory.Instance.Remove(this, playerIndex);
    }
}
