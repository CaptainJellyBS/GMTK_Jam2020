using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public List<Enemy> enemyList;
    public float nextLevel;
    public GameObject deathPanel, finishPanel;

    public int attackingEnemies = 0;

    private void Awake()
    {
        Instance = this;

        enemyList = new List<Enemy>();
        enemyList.AddRange(FindObjectsOfType<Enemy>());
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        if(enemyList.Count <1)
        {
            Finish();
        }
    }

    public void Finish()
    {
        Debug.Log("Ya did it");
        finishPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Die()
    {
        Debug.Log("Ya died it");
        deathPanel.SetActive(true); 
        Time.timeScale = 0;

    }
}
