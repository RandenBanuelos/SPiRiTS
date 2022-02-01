using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    // VARIABLES
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;


    // FUNCTIONS

    private void Awake()
    {
        var rootMenu = GameObject.Find("CharacterSlots");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex, input.currentControlScheme);
        }
    }
}
