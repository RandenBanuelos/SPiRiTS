using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Create New Healing Item")]
public class HealingItem : Item
{
    [Header("Healing")]
    [SerializeField] private int healthRestored = 0;
    [SerializeField] private bool healsMax = false;

    public override void Use()
    {
        base.Use();

        if (healsMax)
        {
            Debug.Log("Healed max health");
            Owner.SetCurrentHealth(Owner.MaxHealth);
        }
        else
        {
            int amount = Mathf.Clamp(Owner.CurrentHealth + healthRestored, Owner.CurrentHealth, Owner.MaxHealth);
            Debug.Log("Healed " + amount + " health");
            Owner.SetCurrentHealth(amount);
        }        
    }


    public int HealthRestored => healthRestored;
    public bool HealsMax => healsMax;
}
