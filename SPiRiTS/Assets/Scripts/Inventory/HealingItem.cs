using UnityEngine;

// Written by: Randen Banuelos

/// <summary>
/// Items that restore a player's health by a certain amount, or to their full health
/// </summary>
[CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Create New Healing Item")]
public class HealingItem : Item
{
    // VARIABLES
    [Header("Healing")]
    [SerializeField] private int healthRestored = 0;
    [SerializeField] private bool healsMax = false;


    // FUNCTIONS
    public int HealthRestored => healthRestored;
    public bool HealsMax => healsMax;

    public override void Use()
    {
        base.Use();

        if (healsMax)
        {
            Owner.SetCurrentHealth(Owner.MaxHealth);
        }
        else
        {
            int amount = Mathf.Clamp(Owner.CurrentHealth + healthRestored, Owner.CurrentHealth, Owner.MaxHealth);
            Owner.SetCurrentHealth(amount);
        }        
    }
}
