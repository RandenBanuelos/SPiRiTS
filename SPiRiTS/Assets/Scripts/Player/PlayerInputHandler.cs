﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    // VARIABLES
    private PlayerConfiguration playerConfig;
    private Mover mover;

    [SerializeField] private SkinnedMeshRenderer playerMesh;

    private PlayerControls controls;


    // FUNCTIONS

    private void Awake()
    {
        mover = GetComponent<Mover>();
        controls = new PlayerControls();
    }


    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerMesh.material = pc.PlayerMaterial;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (mover != null)
        {
            if (obj.action.name == controls.PlayerMovement.Movement.name)
            {
                OnMove(obj);
            }
            else if (obj.action.name == controls.PlayerMovement.Jump.name)
            {
                OnJump(obj);
            }
            else if (obj.action.name == controls.PlayerMovement.Sprint.name)
            {
                OnSprint(obj);
            }
            else if (obj.action.name == controls.PlayerMovement.Attack.name)
            {
                OnAttack(obj);
            }
        }      
    }


    public void OnMove(CallbackContext context)
    {
        mover.SetInputVector(context.ReadValue<Vector2>());
    }


    public void OnJump(CallbackContext context)
    {
        mover.Jump();
    }


    public void OnSprint(CallbackContext context)
    {
        if (context.started)
            mover.EnableSprint(true);
        else if (context.canceled)
            mover.EnableSprint(false);
    }


    public void OnAttack(CallbackContext context)
    {
        mover.AttemptAttack();
    }
}
