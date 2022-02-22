using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

/// <summary>
/// Handles the joining of players and configuration of player data, sent over from UI via singleton access
/// </summary>
public class PlayerConfigurationManager : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private int MaxPlayers = 2;


    // REFERENCES
    private List<PlayerConfiguration> playerConfigs;
    private PlayerInputManager inputManager;

    // Singleton instance
    #region Singleton
    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("PLAYER_CONFIGURATION_MANAGER SINGLETON - Trying to create another instance of singleton!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
            inputManager = GetComponent<PlayerInputManager>();
        }
    }
    #endregion


    // FUNCTIONS
    /// <summary>
    /// Returns the list of player configs
    /// </summary>
    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    /// <summary>
    /// Disables joining in the inputManager
    /// </summary>
    public void DisableJoin()
    {
        inputManager.DisableJoining();
    }

    /// <summary>
    /// Enables joining in the inputManager
    /// </summary>
    public void EnableJoin()
    {
        inputManager.EnableJoining();
    }

    /// <summary>
    /// Removes input from each playerConfig
    /// Afterwards, clears list of playerConfigs
    /// </summary>
    public void ResetSession()
    {
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            Destroy(playerConfigs[i].Input);
        }

        playerConfigs.Clear();
    }

    /// <summary>
    /// Sets the player's PlayerMaterial to a color
    /// </summary>
    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    }

    /// <summary>
    /// Sets the given player number to be ready
    /// After, checks if all players are ready
    /// If so, load in game scene
    /// </summary>
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        if (playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Sandbox");
        }
    }

    /// <summary>
    /// Handles player joining
    /// If there are less players than the max limit and the player index is not already in the list of PlayerConfigs, add player to PlayerConfigs
    /// </summary>
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log($"Player #{pi.playerIndex + 1} Joined! ({pi.currentControlScheme})");

        if (playerConfigs.Count + 1 <= MaxPlayers && !playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    // CONSTRUCTOR
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }


    // VARIABLES
    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    public bool IsReady { get; set; }

    public Material PlayerMaterial { get; set; }
}