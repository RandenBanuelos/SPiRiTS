using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

// TODO: Break this into more subclasses
/// <summary>
/// Primary Player class; handles player stats, movement, attacking, and health
/// </summary>
public class Mover : MonoBehaviour
{
    // VARIABLES 
    [Header("General")]
    /// <summary>
    /// Current speed of the player
    /// </summary>
    [SerializeField] private float speed = 6f;

    /// <summary>
    /// Multiplier if the player is sprinting. Multiplies with speed
    /// </summary>
    [SerializeField] private float sprintMultiplier = 1.2f;

    /// <summary>
    /// Distance that the player will jump
    /// </summary>
    [SerializeField] private float jumpHeight = 3f;


    /// <summary>
    /// The distance the player can be away from the ground in order to be considered "grounded"
    /// </summary>
    [SerializeField] private float groundCheckDistance;

    /// <summary>
    /// Stores what is considered to be "ground"
    /// </summary>
    [SerializeField] private LayerMask groundMask;

    /// <summary>
    /// Gravity applied to the player. Applied at all times while the game is running
    /// </summary>
    [SerializeField] private float gravity = -9.81f;


    /// <summary>
    /// Time between animation transitions
    /// </summary>
    [SerializeField] private float animDampTime = 0.1f;

    /// <summary>
    /// The rate at which the player will turn while moving
    /// </summary>
    [SerializeField] private float turnSmoothTime = 0.1f;


    /// <summary>
    /// The max speed that the player will turn at
    /// </summary>
    private float turnSmoothVelocity;

    /// <summary>
    /// The velocity of the player. Used for applying gravity to the player
    /// </summary>
    private Vector3 velocity;

    [Header("Stats")]
    /// <summary>
    /// Max health of the player
    /// </summary>
    [SerializeField] private int maxHealth = 100;

    /// <summary>
    /// Defense stat of the player
    /// </summary>
    [SerializeField] private int defense = 0;

    /// <summary>
    /// Attack stat of the player
    /// </summary>
    // [SerializeField] private int attack = 0;


    /// <summary>
    /// Stores the HUD for the player
    /// </summary>
    [SerializeField] private Canvas playerHUD = null;

    /// <summary>
    /// Stores the health bar for the player
    /// </summary>
    private HealthBar healthBar;


    [Header("Weapon & Armor")]
    /// <summary>
    /// Stores the weapon the player currently has equipped
    /// </summary>
    // [SerializeField] private Weapon equippedWeapon;

    /// <summary>
    /// Stores the armor the player currently has equipped
    /// </summary>
    [SerializeField] private Armor equippedArmor;

    /// <summary>
    /// Bool value of whether or not the player can use a weapon
    /// </summary>
    // [SerializeField] bool usesWeapon = false;

    /// <summary>
    /// Bool value of whether or not the player can wear armor
    /// </summary>
    [SerializeField] bool usesArmor = false;


    [Header("Weaknesses & Resistances")]
    /// <summary>
    /// List of all the player's weaknesses
    /// </summary>
    [SerializeField] List<Modifier> weakList = new List<Modifier>();

    /// <summary>
    /// List of all the player's resistances
    /// </summary>
    [SerializeField] List<Modifier> resistList = new List<Modifier>();


    // REFERENCES
    /// <summary>
    /// Reference to the game manager
    /// </summary>
    private GameManager manager;

    /// <summary>
    /// Reference to the character controller for movement
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// Reference to the animator
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Stores the base speed of the player, unchanged by sprint multipliers
    /// </summary>
    private float initialSpeed;

    /// <summary>
    /// Reference to the combat for attacking
    /// </summary>
    private Combat combat;

    /// <summary>
    /// Stores the direction that the player will move towards
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;

    /// <summary>
    /// Stores the input of the player
    /// </summary>
    private Vector2 inputVector = Vector2.zero;

    /// <summary>
    /// Bool value of whether or not player is currently touching the ground
    /// </summary>
    private bool isGrounded;

    /// <summary>
    /// Bool value of whether or not the player is currently sprinting
    /// </summary>
    private bool isSprinting = false;

    /// <summary>
    /// Bool value of whether or not the player is currently alive or dead
    /// </summary>
    private bool isDead = false;


    /// <summary>
    /// Stores the current health of the player
    /// </summary>
    private int currentHealth;

    /// <summary>
    /// Stores all of the weaknesses of the player, values are pulled from weakList
    /// </summary>
    private Dictionary<ElementType, float> weaknesses;

    /// <summary>
    /// Stores all of the resistances of the player, values are pulled from resistList
    /// </summary>
    private Dictionary<ElementType, float> resistances;


    // FUNCTIONS
    /// <summary>
    /// Gets all components
    /// Creates dictionaries of weaknesses and resistances using values in weakList and resistList
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        combat = GetComponent<Combat>();
        initialSpeed = speed;
        currentHealth = maxHealth;

        weaknesses = new Dictionary<ElementType, float>();
        resistances = new Dictionary<ElementType, float>();

        for (int i = 0; i < weakList.Count; i++)
            weaknesses.Add(weakList[i].Element, weakList[i].Mod);

        for (int i = 0; i < resistList.Count; i++)
            resistances.Add(resistList[i].Element, resistList[i].Mod);

        manager = GameManager.Instance;

        manager.AddInstantiatedPlayer(this);
    }

    /// <summary>
    /// Sets inputVector
    /// </summary>
    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    /// <summary>
    /// Sets up HUD values of player number and health
    /// </summary>
    public void SetHUD(Canvas hud)
    {
        playerHUD = hud;
        playerHUD.GetComponent<InventoryUI>().SetPlayerIndex(GetComponent<PlayerInputHandler>().GetPlayerIndex());
        healthBar = playerHUD.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    /// <summary>
    /// Returns the InventoryUI for this player
    /// </summary>
    public InventoryUI GetInventory()
    {
        return playerHUD.GetComponent<InventoryUI>();
    }


    public int CurrentHealth => currentHealth;


    public int MaxHealth => maxHealth;


    public bool IsDead => isDead;

    /// <summary>
    /// Updates the health bar to the value of amount
    /// </summary>
    public void SetCurrentHealth(int amount)
    {
        currentHealth = amount;
        healthBar.SetHealth(amount);
    }

    /// <summary>
    /// Updates healthBar to the HealthBar input bar
    /// </summary>
    public void SetHealthBar(HealthBar bar)
    {
        healthBar = bar;
    }

    /// <summary>
    /// Handles movement of players
    /// </summary>
    void Update()
    {
        // Checks if game is unpaused and player is alive
        if (!PauseMenu.GameIsPaused && !isDead)
        {
            // Returns bool value if the player is within groundCheckDistance range of the ground
            isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

            // Checks if the player is currently touching the ground, and if so, reset the player's y velocity to -2
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Returns a Vector3 based on the player's movement input
            moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;

            // If the player is giving any input, then play the respective run/walk animation and update speed, then move the player
            if (moveDirection.magnitude >= 0.1f)
            {
                // Checks if player is currently sprinting on the ground
                if (isSprinting && isGrounded)
                {
                    // Run
                    Run();
                }
                // Otherwise, they are considered walking
                else
                {
                    // Walk
                    Walk();
                }

                // Finds the direction that the player should be moving, and turns the player to face that direction
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                // Moves the player, based on their direction and whether or not they are sprinting or not
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            // If the player is giving no input, play Idle animation
            else
            {
                Idle();
            }

            // Applies gravity to the player
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }  
    }

    /// <summary>
    /// Updates speed for walking and tells the animator that the player is walking
    /// </summary>
    private void Walk()
    {
        speed = initialSpeed;
        anim.SetFloat("Speed", 0.5f, animDampTime, Time.deltaTime);
    }

    /// <summary>
    /// Updates speed for running and tells the animator that the player is running
    /// </summary>
    private void Run()
    {
        speed = initialSpeed * sprintMultiplier;
        anim.SetFloat("Speed", 1f, animDampTime, Time.deltaTime);
    }

    /// <summary>
    /// Tells the animator that the player is idle
    /// </summary>
    private void Idle()
    {
        anim.SetFloat("Speed", 0f, animDampTime, Time.deltaTime);
    }

    /// <summary>
    /// Allows the player to jump, as long they are currently grounded
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    /// <summary>
    /// Updates isSprinting value
    /// </summary>
    public void EnableSprint(bool sprint)
    {
        isSprinting = sprint;
    }

    /// <summary>
    /// Allows the player to attack, as long as they are not currently attacking
    /// </summary>
    public void AttemptAttack()
    {
        if (anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer")) == 0)
            StartCoroutine(Attack());
    }

    /// <summary>
    /// Coroutine for attacking, locks player from attacking more than once per cooldown
    /// </summary>
    private IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f);
        combat.Attack();

        yield return new WaitForSeconds(0.5f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }


    // Defending
    /// <summary>
    /// Deals damage to the player
    /// </summary>
    public void TakeDamage(int damage, ElementType element = ElementType.None)
    {
        if (!isDead)
        {
            // Caluculate the damage the player would take, given resistances and weaknesses
            int finalDamage = CalculateDamageTaken(damage, element) - defense;

            // If the finalDamage is greater than 0, turn off attack animations, deal damage, and update the health bar
            if (finalDamage > 0)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
                currentHealth -= finalDamage;
                healthBar.SetHealth(currentHealth);
            }

            // If the attack results in the player being at or below 0 HP, they die. Otherwise play the hurt animation
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
    /// Calculates the damage after taking into accound resistances and weaknesses
    /// </summary>
    private int CalculateDamageTaken(int damage, ElementType element)
    {
        float finalDamage = damage;
        float weakMultiplier = 1f;
        float resistMultiplier = 1f;

        // Takes into account if the player is currently wearing armor, and calculates the weaknesses/resistances for the armor, based on the element of the damage
        if (usesArmor && equippedArmor != null)
        {
            if (equippedArmor.Weaknesses.ContainsKey(element))
                weakMultiplier *= equippedArmor.Weaknesses[element];
            else if (equippedArmor.Resistances.ContainsKey(element))
                resistMultiplier *= equippedArmor.Resistances[element];
        }

        // Calculates weaknesses/resistances for the player, based on the element of the damage
        if (weaknesses.ContainsKey(element))
            weakMultiplier *= weaknesses[element];
        else if (resistances.ContainsKey(element))
            resistMultiplier *= resistances[element];

        // Calculates overall damage and returns the value
        finalDamage *= weakMultiplier;
        finalDamage *= resistMultiplier;

        return Mathf.RoundToInt(finalDamage);
    }

    /// <summary>
    /// Kills the player; plays the death animation, disables HUD and collision, and updates its tag and adds the player to the list of dead players in the GameManager
    /// </summary>
    private void Die()
    {
        isDead = true;
        transform.gameObject.tag = "Dead";
        anim.SetTrigger("Die");
        playerHUD.gameObject.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Enemy");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());

        manager.AddDeadPlayer(this);
    }
}