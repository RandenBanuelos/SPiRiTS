using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' implementation in his Melee Combat tutorial

/// <summary>
/// Handles the initiation of different types of attacks
/// </summary>
public class Combat : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Stores the location of where attacks originate
    /// </summary>
    [SerializeField] private Transform attackPoint;

    /// <summary>
    /// Stores what is considered an enemy
    /// </summary>
    [SerializeField] private LayerMask enemyLayer;


    /// <summary>
    /// The range for melee attacks, originates from attackPoint
    /// </summary>
    [SerializeField] private float attackRange = 0.5f;

    /// <summary>
    /// Attack damage stat
    /// </summary>
    [SerializeField] private int attackDamage = 40;

    /// <summary>
    /// Speed at which ranged attack projectiles are launched
    /// </summary>
    private float attackForce = 15f;

    /// <summary>
    /// Stores the hitbox for projectile
    /// </summary>
    [SerializeField] private Rigidbody bullet;

    /// <summary>
    /// Bool value on whether to use melee or ranged combat
    /// </summary>
    [SerializeField] private bool isRanged = false;

    // FUNCTIONS
    /// <summary>
    /// If player is not a ranged unit, use MeleeAttack,
    /// </summary>
    public void Attack()
    {
        if (!isRanged)
        {
            MeleeAttack();
        }
        else
        {
            RangedAttack();
        }
    }

    /// <summary>
    /// Instantiates a bullet, and "launches" it directly forward from the player's attackPoint
    /// </summary>
    private void RangedAttack()
    {
        Rigidbody bulletInstance =
            Instantiate(bullet, attackPoint.position,attackPoint.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        bulletInstance.velocity = attackForce * attackPoint.forward;
    }

    /// <summary>
    /// Finds enemies within player's attack range and deals damage to all enemies
    /// </summary>
    private void MeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Enemy enemyObject = enemy.GetComponent<Enemy>();
            if (enemyObject == null)
                enemy.GetComponent<Mover>().TakeDamage(attackDamage);
            else
                enemyObject.TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// Draws attack ranges
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
