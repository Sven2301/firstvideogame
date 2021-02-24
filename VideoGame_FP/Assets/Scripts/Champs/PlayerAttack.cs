using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	private bool _isAttacking;
	private Animator _animator;

	public float attackDamage;
	public float abilityPower;
	public float critChance;
	public float armorPenetration;
	public float magicPenetration;
	public bool useMagic;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		// Animator
		if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			_isAttacking = true;
		} else {
			_isAttacking = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		ArrayList damageInfo = new ArrayList();

		if (_isAttacking == true) {
			if (collision.CompareTag("Monster")) {
				if (collision.gameObject.name != "HitBox")
                {
                    if (useMagic)
                    {
						damageInfo.Add(abilityPower);
						damageInfo.Add(magicPenetration);
						damageInfo.Add(this.transform.gameObject);
						collision.SendMessageUpwards("magicDamage", damageInfo);
					}
                    else
                    {
						int crit = Random.Range(1, 100);

						if (crit <= critChance)
                        {
							damageInfo.Add(attackDamage * 2);
						}

                        else
                        {
							damageInfo.Add(attackDamage);
						}

						damageInfo.Add(armorPenetration);
						damageInfo.Add(this.transform.gameObject);
						collision.SendMessageUpwards("fisicDamage", damageInfo);
					}
				}
			}
		}
	}
}
