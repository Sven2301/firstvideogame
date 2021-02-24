using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	// Variable para manejar sprites
	private SpriteRenderer _renderer;

	// Variables relacionadas a la salud del player
	public float baseHealthContainer;
	public float baseHealth;
	public float baseHealthCopy;
	public float healthIncrease;

	//Variables relacionadas a las resistencias del player
	public float armor;
	public float magicResist;

	// Variables relacionadas a la experiencia, nivel y oro obtenidos por el player
	public float xp;
	public float xpNeeded;
	public float coins;
	public int level;

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
    {

    }

    private void Update()
    {
        if(xp >= xpNeeded && level < 13)
        {
			levelUp();
        }
    }

    public void Damage()
	{
		StartCoroutine("VisualFeedback");
	}

	// Corutina que se ejecuta para indicar que ha recibido daño
	private IEnumerator VisualFeedback()
	{
		_renderer.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		_renderer.color = Color.white;
	}

	// Metodo que se ejecuta cada vez que el player sube de nivel
	// Actualiza los stats segund el nivel
	private void levelUp()
    {
		level++;
		this.xp = this.xp - this.xpNeeded;
		this.xpNeeded = this.xpNeeded * 1.5f;
		updateHealth();
	}

	private void updateHealth()
    {
		float increase = healthIncrease * (level - 1);
		this.baseHealthContainer = this.baseHealthCopy + increase;
		this.baseHealth += increase;
	}

	public void regenHealth(float percentage)
    {
		if (this.baseHealth < this.baseHealthContainer)
        {
			this.baseHealth += this.baseHealthContainer * percentage;
			if (this.baseHealth > this.baseHealthContainer)
            {
				this.baseHealth = this.baseHealthContainer;
            }
		}
	}
}
