using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	private bool _isAttacking;
	private Animator _animator;
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		// Animator
		if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			_isAttacking = true;
		}
		else
		{
			_isAttacking = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_isAttacking == true)
		{
			if (collision.CompareTag("Player"))
			{
				collision.SendMessageUpwards("Damage");
			}
		}
	}
}
