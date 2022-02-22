using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

/// <summary>
/// Instantiate all players in the level, configuring their data and UI as necessary
/// </summary>
public class InitializeLevel : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Hosts all spawn points within the level
    /// </summary>
    [SerializeField] private Transform[] playerSpawns;

    /// <summary>
    /// The player prefab to Instantiate for each derived player
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// Camera that tracks all players
    /// </summary>
    [SerializeField] private MultipleTargetCamera cam;

    /// <summary>
    /// Hosts all four player HUD's, disabled by default
    /// </summary>
    [SerializeField] private List<Canvas> huds = new List<Canvas>();

    /// <summary>
    /// The health bar "icons" that match the color of the player they represent
    /// </summary>
    [SerializeField] private List<Image> healthBubbles = new List<Image>();


    // FUNCTIONS
    void Start()
    {
        // Collect all the players and their associated information from the PlayerConfigurationManagers
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        // Iterate through all players
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            // Instantiate a player prefab at the i-th spawn point
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);

            // Pass the corresponding PlayerConfiguration information to this new player prefab
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

            // Add this player to the tracking camera
            cam.AddTarget(player.transform);

            // Give this player an inventory Dictionary
            Inventory.Instance.AddNewPlayerInventory();

            // Get the corresponding HUD
            Canvas hud = huds[i];

            // Cache this player's Mover
            Mover playerMover = player.GetComponent<Mover>();

            // Activate HUD
            hud.gameObject.SetActive(true);

            // Link this HUD to the player's Mover
            playerMover.SetHUD(hud);

            // Link the HUD's HealthBar to the Mover
            playerMover.SetHealthBar(hud.GetComponentInChildren<HealthBar>());

            // Set the health bar's color to be the same as the player's color
            healthBubbles[i].color = playerConfigs[i].PlayerMaterial.color;
        }
    }
}
