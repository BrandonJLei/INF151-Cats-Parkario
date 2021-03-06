﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] private Collider2D StickColliderTop;
	[SerializeField] private Collider2D StickColliderBot;


	[SerializeField] private Sprite squareSprite;
	[SerializeField] private Sprite triangleSprite;
	[SerializeField] private Sprite circleSprite;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public bool baseCharacter = true;
	public bool circleCharacter = false;
	public bool triangleCharacter = false;
	public bool squareCharacter = false;

	private string currentShape = "Stick";
	
	
	public Animator animator;
	public CameraController cameraCont;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		//m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezeRotationZ;
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		animator.SetBool("IsJumping", !m_Grounded);


	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Upgrade()
    {
		Debug.Log("Upgrade!");
		if (circleCharacter == false)
		{
			 circleCharacter = true;
		}
		else if(triangleCharacter == false)
        {
			triangleCharacter = true;
        }
		else if (squareCharacter == false)
        {
			squareCharacter = true;
		}
    }

	public void ChangeToStick()
    {
		currentShape = "Stick";
		gameObject.layer = 8;
		cameraCont.offset = new Vector3(0, 0, -13);
		transform.rotation = Quaternion.identity;
		StickColliderTop.enabled = true;
		StickColliderBot.enabled = true;
		animator.enabled = true;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (gameObject.GetComponent<PolygonCollider2D>() != null)
		{
			Destroy(GetComponent<PolygonCollider2D>());
		}
	}

	public void ChangeToCircle()
	{
		if (circleCharacter)
		{
			currentShape = "Circle";
			gameObject.layer = 8;
			cameraCont.offset = new Vector3(0, 0, -10);
			animator.enabled = false;
			GetComponent<SpriteRenderer>().sprite = circleSprite;
			StickColliderTop.enabled = false;
			StickColliderBot.enabled = false;	
			if (gameObject.GetComponent<PolygonCollider2D>() != null)
			{
				Destroy(GetComponent<PolygonCollider2D>());
			}
			gameObject.AddComponent<PolygonCollider2D>();
			//m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;


		}
	}
	public void ChangeToTriangle()
	{
		if (triangleCharacter)
		{
			currentShape = "Triangle";
			gameObject.layer = 8;
			cameraCont.offset = new Vector3(0, 0, -10);
			transform.rotation = Quaternion.identity;
			animator.enabled = false;
			GetComponent<SpriteRenderer>().sprite = triangleSprite;
			StickColliderTop.enabled = false;
			StickColliderBot.enabled = false;
			if (gameObject.GetComponent<PolygonCollider2D>() != null)
            {
				Destroy(GetComponent<PolygonCollider2D>());
			}
			gameObject.AddComponent<PolygonCollider2D>();
		}
	}
	public void ChangeToSquare()
	{
		if (squareCharacter)
		{
			currentShape = "Square";
			gameObject.layer = 10;
			cameraCont.offset = new Vector3(0, 0, -10);
			transform.rotation = Quaternion.identity;
			animator.enabled = false;
			GetComponent<SpriteRenderer>().sprite = squareSprite;
			StickColliderTop.enabled = false;
			StickColliderBot.enabled = false;
			if (gameObject.GetComponent<PolygonCollider2D>() != null)
			{
				Destroy(GetComponent<PolygonCollider2D>());
			}
			gameObject.AddComponent<PolygonCollider2D>();
		}
	}
	public void death()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "LeftArrow" && !(currentShape == "Triangle"))
        {
			m_Rigidbody2D.AddForce(new Vector3(-1000.0f, 0.0f, 0.0f));
        }
		if (col.gameObject.tag == "RightArrow" && !(currentShape == "Triangle"))
		{
			m_Rigidbody2D.AddForce(new Vector3(1000.0f, 0.0f, 0.0f));
		}
		if (col.gameObject.tag == "Oomba")
		{
			death();
		}
	}
}