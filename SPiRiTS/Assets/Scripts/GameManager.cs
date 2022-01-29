using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void AddInstantiatedPlayer(Mover player)
    {
        allPlayers.Add(player);
    }


    public void AddDeadPlayer(Mover player)
    {
        deadPlayers.Add(player);

        if (deadPlayers.Count == allPlayers.Count)
        {
            onAllPlayersDead?.Invoke();
        }
    }


    public void AddInstantiatedBoss(Enemy boss)
    {
        allBosses.Add(boss);
    }


    public void AddDeadBoss(Enemy boss)
    {
        deadBosses.Add(boss);

        if (deadBosses.Count == allBosses.Count)
        {
            onAllBossesDead?.Invoke();
        }
    }


    public void ResetGameManager()
    {
        allPlayers.Clear();
        deadPlayers.Clear();
        allBosses.Clear();
        deadBosses.Clear();
    }
}
