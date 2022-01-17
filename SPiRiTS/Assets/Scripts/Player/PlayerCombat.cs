using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 40;

    private float attackForce = 15f;

    public Rigidbody bullet;
    public bool isRanged;

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
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
