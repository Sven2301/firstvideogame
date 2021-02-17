using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGuarding : MonoBehaviour

{
    public bool isEmpty = true;
    public float resetTime = 60f;
    public bool isReady = false;
    public float timer = 0f;
    public int level = 1;
    public int topLevel = 13;

    // Start is called before the first frame update
    void Start()
    {
        timer = resetTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isEmpty)
        {
            if (collision.CompareTag("Monster"))
            {
                collision.SendMessageUpwards("SwitchGuardSpawn");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isEmpty && !isReady)
        {
            timeReset();
        }
    }

    private void timeReset()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isReady = true;
            this.GetComponentInParent<Transform>().gameObject.SendMessageUpwards("SpawnMonster", this.transform.gameObject);
            this.level++;
            if (this.level > topLevel)
            {
                this.level = this.topLevel;
            }
            this.timer = this.resetTime;
        }
    }

    public void SwitchEmpty()
    {
        isEmpty = !isEmpty;
    }
}

