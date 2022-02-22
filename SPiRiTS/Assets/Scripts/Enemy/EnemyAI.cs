using UnityEngine;
using UnityEngine.AI;

// Written by: Randen Banuelos
// Based on Dave / GameDevelopment's implementation in his Enemy AI tutorial

/// <summary>
/// Controls NavMesh navigation and state changes for player detection
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Enemy enemyObject;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    // Patrolling
    [SerializeField] private float walkPointRange;
    [SerializeField] private float waitTime;  

    // Attacking
    [SerializeField] private float timeBetweenAttacks;

    // States
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private bool playerInSightRange;
    [SerializeField] private bool playerInAttackRange;

    // REFERENCES
    private Transform chasedPlayer;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool canWalk = true;

    private bool attacking;


    // FUNCTIONS
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        // Check for sight and attack range
        if (!enemyObject.IsDead && !attacking)
        {
            playerInSightRange = Physics.CheckSphere(attackPoint.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(attackPoint.position, attackRange, playerLayer);

            if (!playerInSightRange && !playerInAttackRange)
                Patrolling();
            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
            if (playerInSightRange && playerInAttackRange)
                AttackPlayer();
        } 

        if (attacking)
        {
            // Prevents the enemy from tilting towards the ground while looking at the player (Playtest #1 Bug Fix)
            // Based on Revolver2k's solution at https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html

            Vector3 targetPosition = new Vector3(chasedPlayer.position.x,
                                        transform.position.y,
                                        chasedPlayer.position.z);
            transform.LookAt(targetPosition);
        }
    }


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


    private void ResetWalk()
    {
        canWalk = true;
    }


    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            walkPointSet = true;
    }


    private void ChasePlayer()
    {
        Vector3 shortestDistance = Vector3.zero;

        Collider[] nearbyPlayers = Physics.OverlapSphere(attackPoint.position, sightRange, playerLayer);

        foreach(Collider player in nearbyPlayers)
        {
            Vector3 distanceToPlayer = transform.position - player.transform.position;

            if (shortestDistance == Vector3.zero || distanceToPlayer.magnitude < shortestDistance.magnitude)
            {
                shortestDistance = distanceToPlayer;
                chasedPlayer = player.transform;
            }
        }

        if (chasedPlayer != null)
            agent.SetDestination(chasedPlayer.position);
    }


    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(chasedPlayer);

        if (!attacking)
        {
            attacking = true;

            StartCoroutine(enemyObject.Attack());

            Invoke(nameof(ResetAttackAndWalk), timeBetweenAttacks);
        }
    }


    private void ResetAttackAndWalk()
    {
        attacking = false;
        canWalk = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, sightRange);
    }
}
