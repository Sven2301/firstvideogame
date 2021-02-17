using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject _monsters;
    private GameObject _monsterPool;

    public int amount;
    
    void Start()
    {
        _monsterPool = GameObject.Find("MonsterPool");
        _monsters = GameObject.Find("Monsters");

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _monsters.transform.childCount; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                AddMonsterToPool(_monsters.transform.GetChild(i).gameObject);
            }
        }

    }

    private void AddMonsterToPool(GameObject prefab)
    {
        GameObject monster = Instantiate(prefab, this.transform.position, Quaternion.identity, _monsterPool.transform);
        monster.SetActive(false);
    }

    public void SpawnMonster(GameObject _spawn)
    {
        GameObject monster;
        int index;

        while (true)
        {
            index = Random.Range(0, _monsterPool.transform.childCount);
            monster = _monsterPool.transform.GetChild(index).gameObject;
            
            if (monster.GetComponent<EnemyPatrol>().isActive == false)
            {
                monster.GetComponent<EnemyPatrol>().isActive = true;
                monster.transform.position = _spawn.transform.position;
                monster.GetComponent<EnemyPatrol>().spawn = _spawn.transform.gameObject;
                monster.GetComponent<EnemyHealth>().setHealth(_spawn.GetComponent<SpawnGuarding>().level);
                monster.SetActive(true);
                _spawn.transform.gameObject.SendMessage("SwitchEmpty");
                break;
            }
        }

    }
}
