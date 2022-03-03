using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    /// <summary>
    /// Reference to the animator
    /// </summary>
    [SerializeField] private Animator anim;

    /// <summary>
    /// Reference to PlayerInput for input
    /// </summary>
    [SerializeField] private PlayerInput input;


    /// <summary>
    /// Stores the instance of PlayerControls
    /// </summary>
    private PlayerControls inputActions;


    /// <summary>
    /// Stores the input value of the player
    /// </summary>
    private Vector2 movementInput;

    /// <summary>
    /// Creates a new instance of PlayerControls, and subscribes to input's delegate
    /// </summary>
    private void Start()
    {
        inputActions = new PlayerControls();
        input.onActionTriggered += Input_onActionTriggered;
    }

    /// <summary>
    /// Updates the movementInput vector
    /// </summary>
    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == inputActions.PlayerMovement.Movement.name)
            movementInput = obj.ReadValue<Vector2>();
    }

    /// <summary>
    /// Calculates the direction the player is looking, and if the player inputs anything, play the running animation
    /// </summary>
    private void FixedUpdate()
    {
        float horizontal = movementInput.x;
        float vertical = movementInput.y;

        Vector3 lookDirection = new Vector3(horizontal, 0, vertical);
        transform.rotation = Quaternion.LookRotation(lookDirection);

        if (vertical > 0.1 || horizontal > 0.1)
            anim.SetBool("IsRunning", true);
        else
            anim.SetBool("IsRunning", false);
    }
}