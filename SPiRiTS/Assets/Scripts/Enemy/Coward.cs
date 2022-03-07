using UnityEngine;
using UnityEngine.AI;

// Written by: Randen Banuelos and Matthew Jung
// Documentation by: Matthew Jung
// Based on Dave / GameDevelopment's implementation in his Enemy AI tutorial

/// <summary>
/// Controls NavMesh navigation and state changes for player detection
/// </summary>
public class Coward : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Stores the NavMeshAgent for enemy movement
    /// </summary>
    [SerializeField] private NavMeshAgent agent;

    /// <summary>
    /// Stores the enemy for attacking
    /// </summary>
    [SerializeField] private Enemy enemyObject;

    /// <summary>
    /// Stores the location that attacks originate from
    /// </summary>
    [SerializeField] private Transform attackPoint;

    /// <summary>
    /// Stores what is considered "ground"
    /// </summary>
    [SerializeField] private LayerMask groundLayer;

    /// <summary>
    /// Stores what is considered "player"
    /// </summary>
    [SerializeField] private LayerMask playerLayer;


    // Patrolling
    /// <summary>
    /// The range in where walkpoints are generated from
    /// </summary>
    [SerializeField] private float walkPointRange;

    /// <summary>
    /// The time that the enemy will wait at a walkpoint for while patrolling before moving again
    /// </summary>
    [SerializeField] private float waitTime;


    // States
    /// <summary>
    /// The range at which the enemy will "see" a player
    /// </summary>
    [SerializeField] private float sightRange;

    /// <summary>
    /// Bool value of whether or not the player is within sight range
    /// </summary>
    [SerializeField] private bool playerInSightRange;

    // REFERENCES
    /// <summary>
    /// Stores the player the enemy will chase
    /// </summary>
    private Transform chasedPlayer;


    /// <summary>
    /// Stores the location of the destination for the enemy
    /// </summary>
    private Vector3 walkPoint;

    /// <summary>
    /// Bool value of whether or not the enemy has a walkpoint
    /// </summary>
    private bool walkPointSet;

    /// <summary>
    /// Bool value of whether or not the enemy can walk
    /// </summary>
    private bool canWalk = true;



    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Checks if enemy is not dead and not attacking
    /// If so, try to locate a player within its sight and attack ranges
    /// If player is within its sight range but not attack range, call ChasePlayer
    /// If within both ranges, call AttackPlayer
    /// Else, call Patrolling
    /// 
    /// If enemy is currently attacking, look at its player target
    /// </summary>
    private void Update()
    {
        // Check for sight and attack range
        if (!enemyObject.IsDead)
        {
            playerInSightRange = Physics.CheckSphere(attackPoint.position, sightRange, playerLayer);

            if (!playerInSightRange)
                Patrolling();
            if (playerInSightRange)
                RunFromPlayer();
        }
    }

    /// <summary>
    /// No player within its sight or attack ranges
    /// If the enemy has the ability to walk, either look for a possible walkpoint, or move towards target destination
    /// Once enemy is at target walkpoint, stay at the current position for waitTime, then call ResetWalk
    /// </summary>
    private void Patrolling()
    {
        chasedPlayer = null;

        if (canWalk)
        {
            if (!walkPointSet)
                SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            // Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                canWalk = false;

                Invoke(nameof(ResetWalk), waitTime);
            }
        }
    }

    /// <summary>
    /// Allows enemy to walk again
    /// </summary>
    private void ResetWalk()
    {
        canWalk = true;
    }

    /// <summary>
    /// Calculates a random X and Z value within the defined ranges, and sets a walkpoint with those values away from the player
    /// If the calculated walkpoint is on ground, then set it as the active walkpoint
    /// </summary>
    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            walkPointSet = true;
    }

    /// <summary>
    /// Player is within its sight range
    /// Finds players within sight range, and runs in the opposite direction from the closest player
    /// </summary>
    private void RunFromPlayer()
    {
        Vector3 shortestDistance = Vector3.zero;

        Collider[] nearbyPlayers = Physics.OverlapSphere(attackPoint.position, sightRange, playerLayer);

        foreach (Collider player in nearbyPlayers)
        {
            Vector3 distanceToPlayer = transform.position - player.transform.position;

            if (shortestDistance == Vector3.zero || distanceToPlayer.magnitude < shortestDistance.magnitude)
            {
                shortestDistance = distanceToPlayer;
                chasedPlayer = player.transform;
            }
        }

        if (chasedPlayer != null)
            agent.SetDestination(transform.position + (transform.position - chasedPlayer.position));
    }

    /// <summary>
    /// Draws enemy ranges
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, sightRange);
    }
}