using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float maxLifeTime = 3f;
    [SerializeField] private float attackRange = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }


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


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
