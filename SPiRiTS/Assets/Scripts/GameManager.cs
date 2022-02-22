using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos

/// <summary>
/// Tracks alive players/bosses, initiating a lose/win state if one of the two groups are all dead
/// </summary>
public class GameManager : MonoBehaviour
{
    public delegate void OnAllPlayersDead();
    public OnAllPlayersDead onAllPlayersDead;

    public delegate void OnAllBossesDead();
    public OnAllBossesDead onAllBossesDead;

    // REFERENCES
    private List<Mover> allPlayers = new List<Mover>();
    private List<Mover> deadPlayers = new List<Mover>();

    private List<Enemy> allBosses = new List<Enemy>();
    private List<Enemy> deadBosses = new List<Enemy>();


    // Singleton instance
    #region Singleton
    public static GameManager Instance { get; private set; }


    // Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("GAME_MANAGER SINGLETON - Trying to create another instance of singleton!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    #endregion


    // FUNCTIONS
    /// <summary>
    /// Adds a new player instance to the list of all players
    /// </summary>
    public void AddInstantiatedPlayer(Mover player)
    {
        allPlayers.Add(player);
    }

    /// <summary>
    /// Adds player to list of dead players
    /// If list of dead players is the same as the list of all players, invoke the onAllPlayersDead event
    /// </summary>
    public void AddDeadPlayer(Mover player)
    {
        deadPlayers.Add(player);

        if (deadPlayers.Count == allPlayers.Count)
        {
            onAllPlayersDead?.Invoke();
        }
    }

    /// <summary>
    /// Adds a new boss instance to the list of all bosses
    /// </summary>
    public void AddInstantiatedBoss(Enemy boss)
    {
        allBosses.Add(boss);
    }

    /// <summary>
    /// Adds boss to list of dead bosses
    /// If list of dead bosses is the same as the list of all players, invoke the onAllBossesDead event
    /// </summary>
    public void AddDeadBoss(Enemy boss)
    {
        deadBosses.Add(boss);

        if (deadBosses.Count == allBosses.Count)
        {
            onAllBossesDead?.Invoke();
        }
    }

    /// <summary>
    /// Clears the lists of all/dead players and bosses
    /// </summary>
    public void ResetGameManager()
    {
        allPlayers.Clear();
        deadPlayers.Clear();
        allBosses.Clear();
        deadBosses.Clear();
    }
}
