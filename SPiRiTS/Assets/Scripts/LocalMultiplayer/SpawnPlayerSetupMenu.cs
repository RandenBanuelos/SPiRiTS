using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

/// <summary>
/// Creates a new Player Setup Menu, passing on associated information
/// </summary>
public class SpawnPlayerSetupMenu : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// The Menu UI prefab to Instantiate
    /// </summary>
    [SerializeField] private GameObject playerSetupMenuPrefab;

    /// <summary>
    /// The Input component that is part of the PlayerConfiguration prefab
    /// </summary>
    [SerializeField] private PlayerInput input;


    // FUNCTIONS
    private void Awake()
    {
        // TODO: Use a safer method for accessing CharacterSlots than using Find()
        var rootMenu = GameObject.Find("CharacterSlots");

        if (rootMenu != null)
        {
            // Instantiate the menu to the Root Menu, which automatically aligns in a row
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);

            // Set the Input Module for the PlayerConfiguration's Input component
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();

            // Pass the player's input index and control scheme to the menu controller
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex, input.currentControlScheme);
        }
    }
}
