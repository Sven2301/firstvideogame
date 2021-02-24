using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealthRegen : MonoBehaviour
{

    private ArrayList playersInBase = new ArrayList();
    private float time = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 2f;
            foreach (GameObject player in playersInBase)
            {
                float percentage = 0.1f;
                if (player != null)
                {
                    player.GetComponent<PlayerHealth>().regenHealth(percentage);
                }
                
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersInBase.Add(collision.transform.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("plaer puot");
            playersInBase.Remove(collision.transform.gameObject);
        }
    }
}
