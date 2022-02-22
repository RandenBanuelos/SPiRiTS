using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos

/// <summary>
/// Holds all relevant data about a specific Room prefab; used for randomized, modular levels
/// </summary>
public class Room : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// All sides that have an entrance in this room; a direction can only have one entrance for now
    /// </summary>
    [SerializeField] private List<Direction> entrances = new List<Direction>();

    /// <summary>
    /// All sides that have an exit in this room; a direction can only have one exit for now
    /// </summary>
    [SerializeField] private List<Direction> exits = new List<Direction>();

    /// <summary>
    /// Check if this is meant to be the final room of the level
    /// </summary>
    [SerializeField] private bool isBossRoom = false;


    [Header("Spawn Points")]
    // All four locations a player can spawn in, in order from Player #1 - #4
    [SerializeField] private List<Transform> playerSpawnPoints = new List<Transform>();

    /// <summary>
    /// All possible locations within the room that an enemy can spawn
    /// </summary>
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();

    



    // FUNCTIONS
    // "Getter's"
    public List<Direction> Entrances => entrances;

    public List<Direction> Exits => exits;

    public bool IsBossRoom => isBossRoom;

    public List<Transform> PlayerSpawnPoints => playerSpawnPoints;

    public List<Transform> EnemySpawnPoints => enemySpawnPoints;

}


/// <summary>
/// Specifies a direction that an opening/exit can be in the isometric Room
/// </summary>
public enum Direction
{
    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast
}