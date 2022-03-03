using UnityEngine;

// Written by: Randen Banuelos
// Documentation by: Matthew Jung

/// <summary>
/// Items that restore a player's health by a certain amount, or to their full health
/// </summary>
[CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Create New Healing Item")]
public class HealingItem : Item
{
    // VARIABLES
    [Header("Healing")]
    /// <summary>
    /// Value of how much health is healed
    /// </summary>
    [SerializeField] private int healthRestored = 0;

    /// <summary>
    /// Bool value of whether or not the heal will heal to the max HP of the player
    /// </summary>
    [SerializeField] private bool healsMax = false;


    // FUNCTIONS
    public int HealthRestored => healthRestored;
    public bool HealsMax => healsMax;

    /// <summary>
    /// Overrides Use in the base class and heals the player
    /// </summary>
    public override void Use()
    {
        base.Use();

        if (healsMax)
        {
            Owner.SetCurrentHealth(Owner.MaxHealth);
        }
        else
        {
            // Makes sure that the heal will not overheal past the player's max HP
            int amount = Mathf.Clamp(Owner.CurrentHealth + healthRestored, Owner.CurrentHealth, Owner.MaxHealth);
            Owner.SetCurrentHealth(amount);
        }
    }
}