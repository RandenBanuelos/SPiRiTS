using UnityEngine;
using UnityEngine.UI;

// Written by: Randen Banuelos
// Based on Brackeys' implementation in his Health Bar tutorial

/// <summary>
/// Modifies a player's health bar UI
/// </summary>
public class HealthBar : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Slider slider;

    // FUNCTIONS
    /// <summary>
    /// Set the maximum value of the health bar slider so that it slides proportionally
    /// </summary>
    /// <param name="health">A player's maximum health</param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// Update the health bar's position on the slider
    /// </summary>
    /// <param name="health">The number to set the health bar slider to; ranges from zero to maxValue</param>
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}