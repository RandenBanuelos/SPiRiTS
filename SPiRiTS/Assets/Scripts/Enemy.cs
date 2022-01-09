using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Animator anim;
    [SerializeField] private int maxHealth = 100;

    private bool isDead;


    // REFERENCES
    private int currentHealth;


    // FUNCTIONS

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger("Hurt");
            }
        }
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerObj.GetComponent<Collider>());
    }
}
