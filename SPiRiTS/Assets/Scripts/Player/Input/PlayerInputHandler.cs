using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

/// <summary>
/// Handles input from the Player Input component, sending messages over to the associated Mover component
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private SkinnedMeshRenderer playerMesh;


    // REFERENCES
    private PlayerConfiguration playerConfig;
    private Mover mover;
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

    
    public int GetPlayerIndex()
    {
        return playerConfig.PlayerIndex;
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
            else if (obj.action.name == controls.PlayerMovement.UseSpell.name)
            {
                OnItemUsed(obj, 0);
            }
            else if (obj.action.name == controls.PlayerMovement.UseItem1.name)
            {
                OnItemUsed(obj, 1);
            }
            else if (obj.action.name == controls.PlayerMovement.UseItem2.name)
            {
                OnItemUsed(obj, 2);
            }
            else if (obj.action.name == controls.PlayerMovement.UseItem3.name)
            {
                OnItemUsed(obj, 3);
            }
            else if (obj.action.name == controls.PlayerMovement.UseUltimate.name)
            {
                OnItemUsed(obj, 4);
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


    public void OnItemUsed(CallbackContext context, int slotNumber)
    {
        mover.GetInventory().Slots[slotNumber].UseItem();
    }
}
