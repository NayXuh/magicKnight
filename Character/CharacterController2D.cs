using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float runSpeed = 400f;								// Maximum character speed
	[SerializeField] private float jumpForce = 8f;                          	// Amount of force added when the player jumps.
	[SerializeField] private float dashSpeed = 30f;								// How far will dash work
	[SerializeField] public float dashTime = 0.2f;								// How long will dash work (seconds)
	[SerializeField] private float fallMultiplier = 2.5f;						// How much acceleration applied when falling
 	[SerializeField] private float lowJumpMultiplier = 2f;						// How much acceleration applied when not holding "Jump"
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = true;                         	// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;                          	// A mask determining what is ground to the character

	const float groundedRadius = .2f;		// Radius of the overlap circle to determine if grounded
	private bool grounded;            		// Whether or not the player is grounded.
	private Rigidbody2D rb;
	private bool facingRight = true;  		// For determining which way the player is currently facing.
	private Vector2 velocity = Vector2.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void Move(float move, bool jump, bool dash)
	{
		if (IsGrounded() || airControl)
		{
				// Move the character by finding the target velocity
				Vector2 targetVelocity = new Vector2(move * runSpeed, rb.velocity.y);
				// And then smoothing it out and applying it to the character
				rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (rb.velocity.x > 0.1 && !facingRight)
				{
					// ... flip the player.
					Flip();
				// Otherwise if the input is moving the player left and the player is facing right...
				} else if (rb.velocity.x < -0.1 && facingRight) { 
					// ... flip the player.
					Flip();
				}
		}
		// If the player should jump...
		if (IsGrounded() && jump)
		{
			// Add a vertical force to the player.
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		}
		// If falling fall faster
		if(rb.velocity.y < -0.01)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		// If not holding "Jump", jump lower
		} else if (rb.velocity.y > 0.01 && !Input.GetButton("Jump")) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		// Adds high velocity to the character
		if (dash)
		{
			if (facingRight)
			{
				rb.velocity = new Vector2(dashSpeed, 0);
			} else {
				rb.velocity = new Vector2(-dashSpeed, 0);
			}
		}
	}

	public bool IsGrounded()
	{
		float extraHeight = 0.2f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
		return raycastHit.collider != null;
	}
	private void Flip()
	{
		// Switch the way the character is labelled as facing.
		facingRight = !facingRight;

		// Flip character sprite
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
