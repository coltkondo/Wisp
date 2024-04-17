using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    private Rigidbody2D rb;
    private GameObject thisObject;
    private Vector2 playerGround;
    public float speed;
    private float speedHolder;
    private Animator anim;
    [Tooltip("To use Blend Tree it needs the following parameters: float \"distance\", float \"Horizontal\", float \"Vertical\", bool \"SpriteFacingRight\" ")]
    public bool useBlendTree = false;
    private GameManager gameManager;
    public bool isGrounded;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform; // load in the player's position
        anim = animator;
        speedHolder = speed;
        gameManager = FindAnyObjectByType<GameManager>();
        thisObject = animator.gameObject;
        rb = thisObject.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform enemyPos = animator.transform;
        float step = speed * Time.deltaTime;

        if (enemyPos.eulerAngles != Vector3.zero)
        {
            enemyPos.eulerAngles = Vector3.zero;
        }

        if (useBlendTree)
        {
            Vector3 blendTreePos = (playerPos.position - enemyPos.transform.position);
            anim.SetFloat("Horizontal", blendTreePos.x);
            anim.SetFloat("Vertical", blendTreePos.y);

            FlipCheck(blendTreePos.x);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("isTimeStopped"))
        {
            speed = 0f;
            rb.gravityScale = 0f;
        } else
        {
            speed = speedHolder;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            if (isGrounded == true)
            {
                rb.gravityScale = 10f;
            } else
            {
                rb.gravityScale = 0f;
            }
        }

        if (isGrounded == true)
        {
            /*playerGround = playerPos.position;
            playerGround.y = enemyPos.position.y;
            enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerGround, step); */
            playerGround = playerPos.position;
            if (enemyPos.position.y > playerGround.y)
            {
                //enemyPos.eulerAngles = Vector3.zero;
                enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerPos.position, step); // move towards the player
            } 
            else
            {
                playerGround.y = enemyPos.position.y;
                //enemyPos.eulerAngles = Vector3.zero; 
                enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerGround, step);
            }
        } 
        else
        {
            //enemyPos.eulerAngles = Vector3.zero;
            enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerPos.position, step); // move towards the player
        }

        //enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerPos.position, step); // move towards the player
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        Vector2 lineToObject = (anim.transform.position - other.transform.position).normalized;
        anim.transform.position = Vector2.MoveTowards(anim.transform.position, -1 * lineToObject, speed * Time.deltaTime); // don't get too stuck on objects (barely noticeable)
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = 0f;
    }

        // Flip Check & Flip code from Player Movement Script
        private void FlipCheck(float move)
	{
		//Flip the sprite so that they are facing the correct way when moving
		if (move > 0 && !anim.GetBool("SpriteFacingRight")) //if moving to the right and the sprite is not facing the right.
		{
			Flip();
		}
		else if (move < 0 && anim.GetBool("SpriteFacingRight")) //if moving to the left and the sprite is facing right
		{
			Flip();
		}
	}

	private void Flip()
    {
        anim.SetBool("SpriteFacingRight", !anim.GetBool("SpriteFacingRight")); // Flip whether the sprite is facing right
        
        // Find the child GameObject that represents the visual sprite      
        Transform spriteTransform = anim.gameObject.transform;
        if (spriteTransform != null)
        {
            // Vector3 currentScale = spriteTranform.localScale;
            // currentScale.x *= -1;
            // spriteTransform.localScale = currentScale;
            spriteTransform.localScale = new Vector3(spriteTransform.localScale.x *-1, spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }  
}
