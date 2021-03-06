using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    // VARIABLES
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField] private int MaxPlayers = 2;

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
        }
    }
    #endregion

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }


    public void ResetSession()
    {
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            Destroy(playerConfigs[i].Input);
        }

        playerConfigs.Clear();
    }


    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    }


    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        if (playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Sandbox");
        }
    }


    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log(pi.currentControlScheme);

        Debug.Log("Player #" + pi.playerIndex + " Joined!");

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