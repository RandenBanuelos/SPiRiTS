using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // VARIABLES
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


    // REFERENCES
    private CharacterController controller;
    private Animator anim;
    private float initialSpeed;
    private PlayerCombat combat;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private bool isGrounded;
    private bool isSprinting = false;

    
    // FUNCTIONS

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        combat = GetComponent<PlayerCombat>();
        initialSpeed = speed;
    }


    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    void Update()
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
}