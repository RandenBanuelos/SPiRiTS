using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;


    public void SetMaxHealth(int health)
    {
        Debug.Log("Set max health to " + health);
        slider.maxValue = health;
        Debug.Log("Set to " + slider.maxValue);
        slider.value = health;
    }


    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
