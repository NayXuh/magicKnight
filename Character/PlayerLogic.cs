using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{
                                                //Reference to...
    public CharacterController2D controller;    //... CharacterController2D
    public Animator animator;                   //... Animator
    public Rigidbody2D rb;                      //... Rigidbody2D

    public LayerMask enemyLayer;        //Stores enemy layer
    public Transform attackPoint;       //Stores attack point position
    public float attackRange = 0.5f;    //Radius of attack circle
    public int attackDamage = 1;        //Damage of attack
    public float attackRate = 2f;       //How many times per second can attack
    float nextAttackTime = 0f;          //Stores time until next attack
    bool isAlive = true;				//Stores the player's "alive" state
                                //Stores user input for...
    float horizontalMove = 0f;  //... horizontalMovement
    bool jump = false;          //... jumping
    bool dash = false;          //... dashing
    int trapLayer;              //Numerical value for trap layer
    int enemyLayerNum;          //Numerical value for enemy layer

    void Start()
	{
		//Get the integer representation of the "Traps" layer
		trapLayer = LayerMask.NameToLayer("Trap");
        enemyLayerNum = LayerMask.NameToLayer("Enemy");
        rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
                Invoke("Attack", 0.4f);
            }
        }

        if(Input.GetButtonDown("Fire3"))
        {
            dash = true;
            animator.SetBool("IsDashing", true);
            Invoke("StopDash", controller.dashTime);
        }

        if(rb.velocity.y < -0.1)
        {
            animator.SetBool("IsJumping", false);
            if(!controller.IsGrounded())
                animator.SetBool("IsFalling", true);
        } else {
            animator.SetBool("IsFalling", false);
        }
    }

    //Is called 50 times per second
    void FixedUpdate()
    {
        
        if(!isAlive || GameManager.IsGameOver())
        {
            if(GameManager.IsGameOver())
                animator.Rebind();
            rb.drag = 10;
            return;
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
    }

    //Attack all enemies in range
    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyLogic>().TakeDamage(attackDamage);
        }
    }

    //Stops dashing and animation
    public void StopDash()
    {
        dash = false;
        animator.SetBool("IsDashing", false);
    }

    //Check if trap or enemy was touched then kill player
    void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collided object isn't on the Traps AND Enemy layer OR if the player isn't currently
		//alive, exit.
		if ((collision.gameObject.layer != trapLayer && collision.gameObject.layer != enemyLayerNum) || !isAlive)
        {
			return;
        }

        if(collision.gameObject.layer == enemyLayerNum)
        {
            collision.GetComponent<EnemyLogic>().animator.SetTrigger("Attack");
        }
		//Trap or enemy was touched, so set the player's alive state to false...
		isAlive = false;
        //...and play death animation
        animator.SetTrigger("TakeHit");
        animator.SetBool("IsDead", true);

        Invoke("KillPlayer", 1.5f);
    }

    //Kills player
    void KillPlayer()
    {
        
        //Disable player game object
		gameObject.SetActive(false);

		//Tell the Game Manager that the player died
		GameManager.PlayerDied();
    }

    //For displaying attack range during testing
    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
