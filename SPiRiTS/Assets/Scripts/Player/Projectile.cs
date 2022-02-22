using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Stores the name of enemy layers
    /// </summary>
    [SerializeField] private string enemyLayerName = "Enemy";

    /// <summary>
    /// Stores the layer given to enemies
    /// </summary>
    [SerializeField] private LayerMask enemyLayer;

    /// <summary>
    /// Damage that this projectile will deal
    /// </summary>
    [SerializeField] private int attackDamage = 20;

    /// <summary>
    /// The time that this projectile will fly for, before being destroyed
    /// Value stored in seconds
    /// </summary>
    [SerializeField] private float maxLifeTime = 3f;

    /// <summary>
    /// Area that nearby enemies will be harmed by this projectile
    /// </summary>
    [SerializeField] private float attackRange = 0.1f;


    // Start is called before the first frame update
    /// <summary>
    /// Projectile will be destroyed after maxLifeTime seconds
    /// </summary>
    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    /// <summary>
    /// Deals damage to enemies hit by projectile
    /// Upon entering a trigger, check if enemies are within attackRange and deal damage to all of them
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        Debug.Log($"Hit {hitEnemies.Length} {enemyLayer.ToString()}'s");

        if (enemyLayer.ToString() == enemyLayerName) // Projectile shot by a player
        {
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else // Projectile shot by an enemy
        {
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Mover>().TakeDamage(attackDamage);
            }
        }
        
    }

    /// <summary>
    /// Draws hitbox
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
