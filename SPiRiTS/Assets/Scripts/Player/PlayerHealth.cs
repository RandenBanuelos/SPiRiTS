using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float m_StartingHealth;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Image m_FillImage;
    [SerializeField] private Color m_FullHealthColor = Color.green;
    [SerializeField] private Color m_ZeroHealthColor = Color.red;

    private float m_CurrentHealth;
    private bool m_Dead;

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    /*
    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }
    */

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }
}
