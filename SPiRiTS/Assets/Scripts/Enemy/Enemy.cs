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
    [SerializeField] private string charName = "";
    // [SerializeField] private string description = "";


    [Header("Stats")]
    [SerializeField] private bool isBoss = false;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int defense = 0;
    // [SerializeField] private int attack = 0;

    [SerializeField] private Animator anim;
    [SerializeField] private Combat combat;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Canvas textUI;


    [Header("Weapon & Armor")]
    // [SerializeField] private Weapon equippedWeapon;
    [SerializeField] private Armor equippedArmor;
    // [SerializeField] bool usesWeapon = false;
    [SerializeField] bool usesArmor = false;


    [Header("Weaknesses & Resistances")]
    [SerializeField] List<Modifier> weakList = new List<Modifier>();
    [SerializeField] List<Modifier> resistList = new List<Modifier>();


    // [Header("Dropped Items")]
    // [SerializeField] private List<DroppedItem> drops = new List<DroppedItem>();


    [Header("Dissolve Material")]
    [SerializeField] private Material baseMaterial;
    [SerializeField] private float fadeTime = 3f;


    // REFERENCES
    private int currentHealth;
    private bool isDead;

    private Dictionary<ElementType, float> weaknesses;
    private Dictionary<ElementType, float> resistances;

    // Dissolve material references
    private GameManager manager;

    private SkinnedMeshRenderer[] meshes;
    private Material materialInstance;
    private float currentFade = -1.5f;


    // FUNCTIONS

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


    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        healthBar.gameObject.SetActive(false);
        textUI.gameObject.SetActive(false);     

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());
    }


    public IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(.7f);
        combat.Attack();

        yield return new WaitForSeconds(1.5f);
        ResetAttackLayer();
    }


    private void ResetAttackLayer()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }
}
