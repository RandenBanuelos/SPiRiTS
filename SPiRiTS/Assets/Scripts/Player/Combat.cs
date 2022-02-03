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
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 40;

    private float attackForce = 15f;

    [SerializeField] private Rigidbody bullet;
    [SerializeField] private bool isRanged = false;

    // FUNCTIONS

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

    private void RangedAttack()
    {
        Rigidbody bulletInstance =
            Instantiate(bullet, attackPoint.position,attackPoint.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        bulletInstance.velocity = attackForce * attackPoint.forward;
    }

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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
