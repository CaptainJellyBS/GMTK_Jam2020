using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public List<Enemy> enemyList;

    public int attackingEnemies = 0;

    private void Awake()
    {
        Instance = this;

        enemyList = new List<Enemy>();
        enemyList.AddRange(FindObjectsOfType<Enemy>());
    }
}
