using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	//Para trabajar los estados de animacion
	private SpriteRenderer _renderer;
	private Animator _animator;

	public bool isDead = false; //Para señalar si el monstruo ha muerto

	// Variables relacionadas a la salud del monstruo
	public float baseHealth;
	public float baseHealthCopy;
	public float healthIncrease;

	//Variables relacionadas a las resistencias del monstruo
	public float armor;
	public float magicResist;

	//Variables relacioandas a la experiencia que otorga el monstruo
	public float xpGiven;
	public float xpGivenCopy;
	public float xpIncrease;

	//Variables relacioandas al oro que otorga el monstruo
	public int coinGiven;
	public int coinGivenCopy;
	public int coinIncrease;

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_animator = this.GetComponent<Animator>();
	}

	// Metodo que usaran los personajes que apliquen daño fisico
	public void fisicDamage(ArrayList damageInfo)
	{
		float attackDamage = (float)damageInfo[0];		// Daño de ataque
		float armorPenetration = (float)damageInfo[1];	// Penetracion
		GameObject player = (GameObject)damageInfo[2];	// Informacion de quien realiza el daño

		float finalDamage = calculateDamage(false, attackDamage, armorPenetration);
		baseHealth -= finalDamage;

		StartCoroutine("VisualFeedback");
		
		if (baseHealth <= 0) // Proceso que sucede una vez muere el monstruo
        {
			_animator.SetTrigger("Die");
			this.transform.Find("HurtBox").gameObject.SetActive(false);
			player.GetComponent<PlayerHealth>().xp += this.xpGiven;
			player.GetComponent<PlayerHealth>().coins += this.coinGiven;
			isDead = true;
        }
		
	}

	// Metodo que usaran los personajes que apliquen daño magico
	public void magicDamage(ArrayList damageInfo)
	{
		
		float abilityPower = (float)damageInfo[0];      // Daño de ataque
		float magicPenetration = (float)damageInfo[1];  // Penetracion
		GameObject player = (GameObject)damageInfo[2];  // Informacion de quien realiza el daño

		float finalDamage = calculateDamage(false, abilityPower, magicPenetration);
		baseHealth -= finalDamage;

		StartCoroutine("VisualFeedback");

		if (baseHealth <= 0)    // Proceso que sucede una vez muere el monstruo
		{
			_animator.SetTrigger("Die");
			this.transform.Find("HurtBox").gameObject.SetActive(false);
			player.GetComponent<PlayerHealth>().xp += this.xpGiven;
			player.GetComponent<PlayerHealth>().coins += this.coinGiven;
			isDead = true;
		}

	}

	// Este metodo calcula el daño total que realiza un player sobre un monstruo
	// descontando la penetracion del player y la armadura del monstruo.
	private float calculateDamage(bool isMagic, float attack, float penetration)
    {
		float finalDamage;
		float resist;

		if (isMagic) // Si el daño es magico
        {
			resist = this.magicResist - (magicResist * (penetration / 100));

			finalDamage = attack - (attack * (resist / 100));
        }

        else // Si el daño es fisico
        {
			resist = this.armor - (armor * (penetration / 100));

			finalDamage = attack - (attack * (resist / 100));
		}

		return finalDamage;
    }

	// Esta corutina se ejecuta para dar señal de que se ha recibido daño
	private IEnumerator VisualFeedback()
	{
		_renderer.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		_renderer.color = Color.white;
	}

	// Este metodo se ejecuta siempre cuando el monstruo está por spawnear
	// para settear los valores segun su nivel
	public void setHealth(int level)
    {
		this.baseHealth = this.baseHealthCopy + healthIncrease * (level - 1);
		this.xpGiven = this.xpGivenCopy + xpIncrease * (level - 1);
		this.coinGiven = this.coinGivenCopy + coinIncrease * (level - 1);
	}
}
