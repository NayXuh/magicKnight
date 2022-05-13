using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour 
{
    public Animator animator;
    public int maxHealth = 3;
    int currentHealth;
    float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        speed = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", speed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        rb.gravityScale = 2f;

        GameManager.EnemyDefeated();
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        Invoke("DisablePhysics", 1f);
    }
    
    void DisablePhysics()
    {
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
    
}
