using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES
    [SerializeField] float speed = 6f;
    [SerializeField] float sprintMultiplier = 1.2f;
    [SerializeField] private float jumpHeight;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float animDampTime = 0.1f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] Transform cam;

    [SerializeField] private KeyCode runButton = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpButton = KeyCode.Space;
    // [SerializeField] private KeyCode attackButton = KeyCode.Mouse0;

    private float turnSmoothVelocity;
    private Vector3 velocity;


    // REFERENCES
    private CharacterController controller;
    private Animator anim;
    private float initialSpeed;


    // FUNCTIONS
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        initialSpeed = speed;
    }


    private void Update()
    {
        Move();

        /*if (Input.GetKeyDown(attackButton) && anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer")) == 0)
        {
            StartCoroutine(Attack());
        }*/
    }


    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (isGrounded && Input.GetKeyDown(jumpButton))
        {
            Jump();
        }

        if (direction.magnitude >= 0.1f)
        {
            if (Input.GetKey(runButton) && isGrounded)
            {
                // Run
                Run();
            }
            else
            {
                // Walk
                Walk();
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
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


    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }


    /*private IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }*/
}
