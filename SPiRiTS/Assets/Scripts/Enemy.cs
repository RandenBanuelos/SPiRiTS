using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Animator anim;

    //   Enemy Descriptions
    [SerializeField] private string name = "";
    [SerializeField] private string description = "";

    //   Enemy Stats
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int attack = 0;

    // Enemy Health UI
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Image m_FillImage;
    [SerializeField] private Color m_FullHealthColor = Color.green;
    [SerializeField] private Color m_ZeroHealthColor = Color.red;

    //   Enemy Items
    //[SerializeField] private Weapon equippedWeapon;
    //[SerializeField] private Armor equippedArmor;
    [SerializeField] bool usesWeapon = false;
    [SerializeField] bool usesArmor = false;

    //   Enemy weaknesses and resistances
    //[SerializeField] private List<Element> weaknesses = new List<Element>();
    //[SerializeField] private List<Element> resistances = new List<Element>();

    //   Enemy Dropped items
    [SerializeField] private List<DroppedItem> drops = new List<DroppedItem>();

    //   Current life state
    private bool isDead;


    // REFERENCES
    private int currentHealth;


    // FUNCTIONS

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            SetHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger("Hurt");
            }
        }
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = currentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, currentHealth / maxHealth);
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());
    }


}
