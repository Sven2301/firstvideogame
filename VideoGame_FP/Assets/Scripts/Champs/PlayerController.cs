using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float longIdleTime = 5f;
	public float speed = 2.5f;
	public float jumpForce = 2.5f;

	public Transform groundCheck;
	public Transform upCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius;

	// References
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Transform _transform;

	// Timer
	private float _longIdleTimer;
	public float spellUpTimer = 0;
	private float spellDownTimer = 0;

	// Movement
	private Vector2 _movement;
	private bool _facingRight = true;
	private bool _isGrounded;

	// Bool
	public bool isSpellUpReady = true;
	public bool isSpellDownReady = true;
	public bool spellUpCalled;
	public bool spellDownCalled;
	private bool _isAttacking;


	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_transform = GetComponent<Transform>();
	}

	void Start()
    {
        
    }

    void Update()
    {
		if (_isAttacking == false) {
			// Movement
			float horizontalInput = Input.GetAxisRaw("Horizontal");
			_movement = new Vector2(horizontalInput, 0f);

			// Flip character
			if (horizontalInput < 0f && _facingRight == true) {
				Flip();
			} else if (horizontalInput > 0f && _facingRight == false) {
				Flip();
			}
		}

		// Is Grounded?
		_isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		// Is Jumping?
		if (Input.GetButtonDown("Jump") && _isGrounded == true && _isAttacking == false) {
			_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}

		// Wanna Attack?
		if (Input.GetButtonDown("Fire1") && _isGrounded == true && _isAttacking == false) {
			_movement = Vector2.zero;
			_rigidbody.velocity = Vector2.zero;
			_animator.SetTrigger("Attack");
		}

		if (Input.GetKeyDown("x"))
		{
			spellUpCalled = true;
		}

		if (Input.GetKeyDown("c"))
		{
			spellDownCalled = true;
		}

		// Spell timers
		if (!isSpellUpReady)
        {
			spellUpTimer += Time.deltaTime;

			if (spellUpTimer >= 10)
            {
				isSpellUpReady = true;
				spellUpTimer = 0;

			}
        }

		if (!isSpellDownReady)
		{
			spellDownTimer += Time.deltaTime;

			if (spellDownTimer >= 10)
			{
				isSpellDownReady = true;
				spellDownTimer = 0;
			}
		}
	}

	void FixedUpdate()
	{
		if (_isAttacking == false) {
			float horizontalVelocity = _movement.normalized.x * speed;
			_rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
		}

		if (spellUpCalled)
        {
			spellUpCalled = false;
			spellUp();
        }

		if (spellDownCalled)
        {
			spellDownCalled = false;
			spellDown();
        }
	}

	void LateUpdate()
	{
		_animator.SetBool("Idle", _movement == Vector2.zero);
		_animator.SetBool("IsGrounded", _isGrounded);
		_animator.SetFloat("VerticalVelocity", _rigidbody.velocity.y);

		// Animator
		if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			_isAttacking = true;
		} else {
			_isAttacking = false;
		}

		// Long Idle
		if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
			_longIdleTimer += Time.deltaTime;

			if (_longIdleTimer >= longIdleTime) {
				_animator.SetTrigger("LongIdle");
			}
		} else {
			_longIdleTimer = 0f;
		}
	}

	private void Flip()
	{
		_facingRight = !_facingRight;
		float localScaleX = transform.localScale.x;
		localScaleX = localScaleX * -1f;
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
	}

	private void spellUp()
    {
		if (isSpellUpReady)
        {
			RaycastHit2D hit = Physics2D.Raycast(upCheck.position, Vector2.up, Mathf.Infinity, groundLayer);
			if (hit.collider != null)
            {
				RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(hit.point.x, hit.point.y+0.2f), Vector2.up, Mathf.Infinity, groundLayer);

				if (hit2.collider != null && hit2.point != hit.point)
                {
					_transform.position = new Vector3(hit2.point.x, hit2.point.y + 0.5f, _transform.position.z);
					//isSpellUpReady = false;
				}
			}

            else
            {
				Debug.Log("No se ejecuta");
            }

        }
    }

	private void spellDown()
	{
		if (isSpellDownReady)
		{
			RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, Mathf.Infinity, groundLayer);
			Debug.Log("New cast");
			if (hit.collider != null)
			{

				Debug.Log(hit.point);
				RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(hit.point.x, hit.point.y - 0.2f), Vector2.down, Mathf.Infinity, groundLayer);

				if (hit2.collider != null)
				{
					Debug.Log(hit2.point);
					RaycastHit2D hit3 = Physics2D.Raycast(new Vector2(hit2.point.x, hit2.point.y - 0.2f), Vector2.down, Mathf.Infinity, groundLayer);

					if (hit3.collider != null)
                    {
						Debug.Log(hit3.point);
						_transform.position = new Vector3(hit3.point.x, hit3.point.y + 0.5f, _transform.position.z);
						//isSpellDownReady = false;
					}
					
				}
			}

			else
			{
				Debug.Log("No se ejecuta");
			}

		}
	}
}
