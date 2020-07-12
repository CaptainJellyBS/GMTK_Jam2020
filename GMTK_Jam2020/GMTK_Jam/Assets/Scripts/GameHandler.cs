using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public List<Enemy> enemyList;
    public int nextLevel;
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

    public void RestartLevel()
    {
        AudioManager.Instance.StopAllMusic();
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.StopAllMusic();
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        AudioManager.Instance.StopAllMusic();
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene(nextLevel);
    }
}
