using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject DeathScore;
    int lifeCount;
    int gameScore;
    float[] duringDeath;
    bool blockinput;
    float timer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        lifeCount = 3;
        gameScore = -1;
        duringDeath = new float[] { 0.48f, 1.392f, 2.4f, 3.672f, 4.824f };
    }

    public void diedPonpoko()
    {
        blockinput = true;
        --lifeCount;
        DeathScore.transform.GetChild(lifeCount).gameObject.SetActive(false);
        if (lifeCount == 0)
        {
            AudioManager.instance.SetAudio((int)AudioManager.audio.gameover);
            AudioManager.instance.PlayAudio();
            Time.timeScale = 0f;
            UIManager.instance.ShowGameOver();
        }
    }

    public void restartGame(int floor)
    {
        gameScore = -1;
        StartCoroutine(DelayRestart(duringDeath[floor]));
    }

    public void countUpScore()
    {
        ++gameScore;
    }
    public int getGameScore()
    {
        return gameScore;
    }

    IEnumerator DelayRestart(float delay)
    {
        yield return new WaitForSeconds(delay + 0.5f);
        blockinput = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool isBlockInput()
    {
        return blockinput;
    }

    public void setTimer(int floor)
    {
        switch (floor)
        {
            case 0:
                timer = duringDeath[0];
                break;
            case 1:
                timer = duringDeath[1];
                break;
            case 2:
                timer = duringDeath[2];
                break;
            case 3:
                timer = duringDeath[3];
                break;
            case 4:
                timer = duringDeath[4];
                break;
        }
    }

    public float getTimer()
    {
        return this.timer;
    }
}