using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Animator anim;

    [SerializeField] private KeyCode attackButton;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 40;


    // FUNCTIONS

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackButton) && anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer")) == 0)
        {
            StartCoroutine(Attack());
        }
    }


    private IEnumerator Attack()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }


        yield return new WaitForSeconds(0.9f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
