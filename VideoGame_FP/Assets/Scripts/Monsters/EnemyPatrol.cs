using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	public float speed = 1f;

	private Transform _target;
	private Rigidbody2D _rigidBody;
	private bool facingRight = true;
	private Animator _animator;
	private Vector2 _movement;
	private int amountOfPlayers;
	private EnemyHealth _healthScript;
	private float _deadTimer = 0f;

	public GameObject spawn;
	public bool isOnRange;
	public float range;
	public float attackRange;
	public bool guardSpawn;
	public bool isActive = false;
	


	private void Awake()
	{
		_rigidBody = this.GetComponent<Rigidbody2D>();
		_animator = this.GetComponent<Animator>();
		_healthScript = this.GetComponent<EnemyHealth>();

	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (_healthScript.isDead)
        {
			_deadTimer += Time.deltaTime;
			_animator.SetFloat("deadTime", _deadTimer);
			if (_deadTimer >= 3f)
            {
				ResetState();
			}
        }
	}

	private void FixedUpdate()
	{
		if (!_healthScript.isDead)
		{

			if (guardSpawn)
			{
				PatrolToSpawn();
			}

			else
			{
				float horVelocity = speed;

				if (_target != null)
				{
					float distance = Vector3.Distance(this.transform.position, _target.position);

					if (distance <= attackRange)
					{
						_movement = Vector2.zero;
						_rigidBody.velocity = _movement;
						isOnRange = true;
					}
					else if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
					{

						if (facingRight == false)
						{
							horVelocity *= -1f;
						}

						_movement = new Vector2(horVelocity, _rigidBody.velocity.y);
						_rigidBody.velocity = _movement;
					}

				}
			}
		}

	}

	private void LateUpdate()
	{
		if (!_healthScript.isDead)
		{
			_animator.SetBool("Idle", _movement == Vector2.zero);

			if (isOnRange && !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
			{
				_movement = Vector2.zero;
				_rigidBody.velocity = _movement;
				_animator.SetTrigger("Attack");
				isOnRange = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_healthScript.isDead)
		{
			if (collision.CompareTag("Player"))
			{
				amountOfPlayers++;
				_target = collision.GetComponent<Transform>();
				Flip(_target);
			}
		}

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!_healthScript.isDead)
		{
			if (collision.CompareTag("Player"))
			{
				_target = collision.GetComponent<Transform>();
				Flip(_target);
			}
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!_healthScript.isDead)
		{
			if (collision.CompareTag("Player"))
			{
				amountOfPlayers--;
				_target = null;
				isOnRange = false;
				_movement = Vector2.zero;
				_rigidBody.velocity = _movement;

				if (amountOfPlayers == 0)
				{
					if (!guardSpawn)
					{
						SwitchGuardSpawn();
					}

					PatrolToSpawn();
				}
			}
		}

	}

	private void Flip(Transform objRef)
	{
		
		if (objRef != null)
		{
			// If we are in the left, change target to the right
			if (objRef.position.x >= this.transform.position.x)
			{
				transform.localScale = new Vector3(1, 1, 1);
				facingRight = true;
			}

			// If we are in the right, change target to the left
			else if (objRef.position.x < this.transform.position.x)
			{
				transform.localScale = new Vector3(-1, 1, 1);
				facingRight = false;
			}
		}

	}

	public void SwitchGuardSpawn()
    {
		guardSpawn = !guardSpawn;
    }

	public void ResetState()
    {
		this.spawn.GetComponent<SpawnGuarding>().isEmpty = true;
		this.spawn.GetComponent<SpawnGuarding>().isReady = false;
		this.spawn = null;
		this.gameObject.SetActive(false);
		this._deadTimer = 0f;
		this.isActive = false;
		this.isOnRange = false;
		this.facingRight = true;
		this.guardSpawn = false;
		this.GetComponent<EnemyHealth>().isDead = false;
		this._target = null;
		this.transform.Find("HurtBox").gameObject.SetActive(true);
		this._movement = Vector2.zero;
	}

	public void PatrolToSpawn()
	{
		// Method to move the enemy
		float horVelocity = speed;

		if (spawn != null)
		{
			Flip(spawn.transform);

			float distance = Vector3.Distance(this.transform.position, spawn.transform.position);
			
			if (distance <= range)
			{
				_movement = Vector2.zero;
				_rigidBody.velocity = _movement;
				SwitchGuardSpawn();
			}
			else if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
			{

				if (facingRight == false)
				{
					horVelocity *= -1f;
				}

				_movement = new Vector2(horVelocity, _rigidBody.velocity.y);
				_rigidBody.velocity = _movement;
			}

		}

        else
        {
			Debug.Log("No spawn point set.");
        }


	}
}
