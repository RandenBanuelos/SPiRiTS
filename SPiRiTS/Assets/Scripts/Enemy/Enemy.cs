using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Written by: Randen Banuelos

/// <summary>
/// Base class for all enemy types; includes statistics, health, and attack functionalities
/// </summary>
public class Enemy : MonoBehaviour
{
    // VARIABLES
    [Header("Descriptors")]
    /// <summary>
    /// The enemy name for the UI
    /// </summary>
    [SerializeField] private string charName = "";

    /// <summary>
    /// The description of the enemy
    /// </summary>
    // [SerializeField] private string description = "";


    [Header("Stats")]
    /// <summary>
    /// Bool value of whether or not the enemy is a boss
    /// </summary>
    [SerializeField] private bool isBoss = false;

    /// <summary>
    /// The max health of the enemy
    /// </summary>
    [SerializeField] private int maxHealth = 100;

    /// <summary>
    /// The defense stat of the enemy
    /// </summary>
    [SerializeField] private int defense = 0;

    /// <summary>
    /// The attack stat of the enemy
    /// </summary>
    // [SerializeField] private int attack = 0;


    /// <summary>
    /// Reference to the animator
    /// </summary>
    [SerializeField] private Animator anim;

    /// <summary>
    /// Reference to combat for attacking
    /// </summary>
    [SerializeField] private Combat combat;

    /// <summary>
    /// Stores the health bar for the enemy
    /// </summary>
    [SerializeField] private HealthBar healthBar;

    /// <summary>
    /// Reference to the canvas to display the UI
    /// </summary>
    [SerializeField] private Canvas textUI;


    [Header("Weapon & Armor")]
    /// <summary>
    /// Stores the weapon the enemy currently has equipped
    /// </summary>
    // [SerializeField] private Weapon equippedWeapon;

    /// <summary>
    /// Stores the armor the enemy currently has equipped
    /// </summary>
    [SerializeField] private Armor equippedArmor;

    /// <summary>
    /// Bool value of whether or not the enemy can use a weapon
    /// </summary>
    // [SerializeField] bool usesWeapon = false;

    /// <summary>
    /// Bool value of whether or not the enemy can wear armor
    /// </summary>
    [SerializeField] bool usesArmor = false;


    [Header("Weaknesses & Resistances")]
    /// <summary>
    /// List of all the enemy's weaknesses
    /// </summary>
    [SerializeField] List<Modifier> weakList = new List<Modifier>();

    /// <summary>
    /// List of all the enemy's resistances
    /// </summary>
    [SerializeField] List<Modifier> resistList = new List<Modifier>();


    // [Header("Dropped Items")]
    /// <summary>
    /// List of all items the enemy will drop
    /// </summary>
    // [SerializeField] private List<DroppedItem> drops = new List<DroppedItem>();


    [Header("Dissolve Material")]
    /// <summary>
    /// Material used for the SkinnedMeshRenderer
    /// </summary>
    [SerializeField] private Material baseMaterial;

    /// <summary>
    /// Time the material will fade out once the enemy dies
    /// </summary>
    [SerializeField] private float fadeTime = 3f;


    // REFERENCES
    /// <summary>
    /// Stores the current health of the enemy
    /// </summary>
    private int currentHealth;

    /// <summary>
    /// Bool value of whether or not the enemy is considered dead
    /// </summary>
    private bool isDead;

    /// <summary>
    /// Stores all of the weaknesses of the enemy, values are pulled from weakList
    /// </summary>
    private Dictionary<ElementType, float> weaknesses;

    /// <summary>
    /// Stores all of the resistances of the enemy, values are pulled from resistList
    /// </summary>
    private Dictionary<ElementType, float> resistances;

    // Dissolve material references
    /// <summary>
    /// Reference to the game manager
    /// </summary>
    private GameManager manager;

    /// <summary>
    /// Array of meshes in the SkinnedMeshRenderer component
    /// </summary>
    private SkinnedMeshRenderer[] meshes;

    /// <summary>
    /// Stores the instantiated material using baseMaterial
    /// </summary>
    private Material materialInstance;

    /// <summary>
    /// Stores the current time at which the enemy has been fading when dead
    /// </summary>
    private float currentFade = -1.5f;


    // FUNCTIONS
    /// <summary>
    /// Initializes enemy object, setting up health, UI, and materials
    /// Sets up dictionary of weaknesses and resistances
    /// If the enemy instance is a boss, add instance to manager's list
    /// </summary>
    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        textUI.GetComponentInChildren<TMP_Text>().text = charName;

        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

        materialInstance = Instantiate<Material>(baseMaterial);

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material = materialInstance;
        }

        weaknesses = new Dictionary<ElementType, float>();
        resistances = new Dictionary<ElementType, float>();

        for (int i = 0; i < weakList.Count; i++)
            weaknesses.Add(weakList[i].Element, weakList[i].Mod);

        for (int i = 0; i < resistList.Count; i++)
            resistances.Add(resistList[i].Element, resistList[i].Mod);

        manager = GameManager.Instance;

        if (isBoss)
            manager.AddInstantiatedBoss(this);
    }

    /// <summary>
    /// Checks if the enemy is dead
    /// If so, fade the enemy out, and remove enemy from game once done fading
    /// </summary>
    private void Update()
    {
        if (isDead)
        {
            if (currentFade < fadeTime)
            {
                currentFade += Time.deltaTime;

                if (currentFade > 0)
                    materialInstance.SetFloat("_Fade", currentFade / fadeTime);
            }
            else
            {
                materialInstance.SetFloat("_Fade", 1f);

                if (isBoss)
                    manager.AddDeadBoss(this);

                this.gameObject.SetActive(false);
            }
        }
    }


    public bool IsDead => isDead;


    // Defending
    /// <summary>
    /// Deals damage to the enemy
    /// If the enemy is alive and the damage value is greater than 0, update the enemy's health and activate the animator
    /// If the health after damage taken is <= 0, kill the enemy
    /// Else, trigger the hurt animation
    /// </summary>
    public void TakeDamage(int damage, ElementType element = ElementType.None)
    {
        if (!isDead)
        {
            int finalDamage = CalculateDamageTaken(damage, element) - defense;
            if (finalDamage > 0)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
                currentHealth -= finalDamage;
                healthBar.SetHealth(currentHealth);
            }

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

    /// <summary>
    /// Calculates the damage taken based on armor, weaknesses and resistances
    /// </summary>
    private int CalculateDamageTaken(int damage, ElementType element)
    {
        float finalDamage = damage;
        float weakMultiplier = 1f;
        float resistMultiplier = 1f;

        if (usesArmor && equippedArmor != null)
        {
            if (equippedArmor.Weaknesses.ContainsKey(element))
                weakMultiplier *= equippedArmor.Weaknesses[element];
            else if (equippedArmor.Resistances.ContainsKey(element))
                resistMultiplier *= equippedArmor.Resistances[element];
        }


        if (weaknesses.ContainsKey(element))
            weakMultiplier *= weaknesses[element];
        else if (resistances.ContainsKey(element))
            resistMultiplier *= resistances[element];

        finalDamage *= weakMultiplier;
        finalDamage *= resistMultiplier;

        return Mathf.RoundToInt(finalDamage);
    }

    /// <summary>
    /// "Kills" the enemy object
    /// Deactivates the enemy from the game and deactivates collision with player
    /// </summary>
    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        healthBar.gameObject.SetActive(false);
        textUI.gameObject.SetActive(false);     

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());
    }

    /// <summary>
    /// Enemy attack
    /// </summary>
    public IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(.7f);
        combat.Attack();

        yield return new WaitForSeconds(1.5f);
        ResetAttackLayer();
    }

    /// <summary>
    /// Resets animator's attack layer
    /// </summary>
    private void ResetAttackLayer()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }
}
