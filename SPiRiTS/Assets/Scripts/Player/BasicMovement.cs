using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput input;

    private PlayerControls inputActions;

    private Vector2 movementInput;


    private void Start()
    {
        inputActions = new PlayerControls();
        input.onActionTriggered += Input_onActionTriggered;
    }


    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == inputActions.PlayerMovement.Movement.name)
            movementInput = obj.ReadValue<Vector2>();
    }


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
