using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public PlayerAudio playerAudio;
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public bool isFacingRight = true;

    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    public bool isDisabled = false;

    public bool wallDisabled = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private GameObject smokeTrailPrefab; // Assign this in the Inspector

    Animator anim;
    PlayerCombat combat;

    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the GameObject.
        playerAudio = GetComponent<PlayerAudio>();
        combat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (isDisabled == true)
        {
            horizontal = 0;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            anim.SetBool("IsWalking", true);
            if (playerAudio && !playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
				{
					playerAudio.WalkSource.Play();
				}
        }
        else
        {
            anim.SetBool("IsWalking", false);
            if (playerAudio && playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
				{
					playerAudio.WalkSource.Stop();
				}
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        	if (playerAudio && playerAudio != null)
		    {
			    playerAudio.JumpSource.Play();
		    }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            if (playerAudio && playerAudio != null)
		    {
			    playerAudio.JumpSource.Play();
		    }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (!wallDisabled) {
            WallSlide();
            WallJump();
        }

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    /*private void Flip()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wisp_Attack"))
        {
            return;
        } else
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)  // This version can be used to prevent Wisp from flipping mid-attack.
            {                                                                           // You might want this if you don't like that Wisp can deal dmg while facing the other way
                isFacingRight = !isFacingRight;                                         // if the player turns mid-attack.
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        
    }*/

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void DisablePlayer()
    {
        isDisabled = true;
    }

    public void EnablePlayer()
    {
        isDisabled = false;
    }

    private IEnumerator Dash()
{
    canDash = false;
    isDashing = true;
    
    // Adjust these values as needed to position the smoke trail correctly relative to your player's feet
    Vector3 smokeSpawnPosition = transform.position - new Vector3(0, 0.3f, 0); // Adjust 0.5f to lower or raise the spawn position
    
    // Instantiate smoke trail with the adjusted position
    GameObject smokeTrailInstance = Instantiate(smokeTrailPrefab, smokeSpawnPosition, Quaternion.identity);
    
    // Flip the smoke trail based on the player's facing direction
    smokeTrailInstance.GetComponent<SpriteRenderer>().flipX = !isFacingRight;

    float originalGravity = rb.gravityScale;
    rb.gravityScale = 0f;
    rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
    
    yield return new WaitForSeconds(dashingTime);
    
    rb.gravityScale = originalGravity;
    isDashing = false;
    yield return new WaitForSeconds(dashingCooldown);
    canDash = true;
}

}
