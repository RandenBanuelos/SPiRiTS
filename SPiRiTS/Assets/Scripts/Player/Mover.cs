using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // VARIABLES 
    [Header("General")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float sprintMultiplier = 1.2f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float animDampTime = 0.1f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int defense = 0;
    // [SerializeField] private int attack = 0;

    [SerializeField] private Canvas playerHUD = null;
    private HealthBar healthBar;


    [Header("Weapon & Armor")]
    // [SerializeField] private Weapon equippedWeapon;
    [SerializeField] private Armor equippedArmor;
    // [SerializeField] bool usesWeapon = false;
    [SerializeField] bool usesArmor = false;


    [Header("Weaknesses & Resistances")]
    [SerializeField] List<Modifier> weakList = new List<Modifier>();
    [SerializeField] List<Modifier> resistList = new List<Modifier>();


    // REFERENCES
    private GameManager manager;

    private CharacterController controller;
    private Animator anim;
    private float initialSpeed;
    private Combat combat;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private bool isGrounded;
    private bool isSprinting = false;
    private bool isDead = false;

    private int currentHealth;
    private Dictionary<ElementType, float> weaknesses;
    private Dictionary<ElementType, float> resistances;


    // FUNCTIONS

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


    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }


    public void SetHUD(Canvas hud)
    {
        playerHUD = hud;
        playerHUD.GetComponent<InventoryUI>().SetPlayerIndex(GetComponent<PlayerInputHandler>().GetPlayerIndex());
        healthBar = playerHUD.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }


    public InventoryUI GetInventory()
    {
        return playerHUD.GetComponent<InventoryUI>();
    }


    public int CurrentHealth => currentHealth;


    public int MaxHealth => maxHealth;


    public bool IsDead => isDead;


    public void SetCurrentHealth(int amount)
    {
        currentHealth = amount;
        healthBar.SetHealth(amount);
    }


    public void SetHealthBar(HealthBar bar)
    {
        healthBar = bar;
    }


    void Update()
    {
        if (!PauseMenu.GameIsPaused && !isDead)
        {
            isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                if (isSprinting && isGrounded)
                {
                    // Run
                    Run();
                }
                else
                {
                    // Walk
                    Walk();
                }

                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                Idle();
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }  
    }

    private void Walk()
    {
        speed = initialSpeed;
        anim.SetFloat("Speed", 0.5f, animDampTime, Time.deltaTime);
    }


    private void Run()
    {
        speed = initialSpeed * sprintMultiplier;
        anim.SetFloat("Speed", 1f, animDampTime, Time.deltaTime);
    }


    private void Idle()
    {
        anim.SetFloat("Speed", 0f, animDampTime, Time.deltaTime);
    }


    public void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }


    public void EnableSprint(bool sprint)
    {
        isSprinting = sprint;
    }


    public void AttemptAttack()
    {
        if (anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer")) == 0)
            StartCoroutine(Attack());
    }


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
        transform.gameObject.tag = "Dead";
        anim.SetTrigger("Die");
        playerHUD.gameObject.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Enemy");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());

        Debug.Log("Got to the point.");
        manager.AddDeadPlayer(this);
    }
}