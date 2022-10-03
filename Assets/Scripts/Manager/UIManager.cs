using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject ScorePrefabs;
    public Sprite[] Scores;
    public Text scoreText;
    public GameObject[] GameExit;
    int[] scoreData;

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
        ScorePrefabs = Instantiate(ScorePrefabs, this.transform);
        ScorePrefabs.SetActive(false);
        scoreData = new int[] { 10, 30, 60, 110, 210, 410, 710, 1210, 2210, 4210 };
    }

    public void ShowGameScore(int idx, Transform transform)
    {
        ScorePrefabs.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Scores[idx];
        ScorePrefabs.transform.position = transform.position + new Vector3(1f, 0.5f, 0);
        ScorePrefabs.SetActive(true);
        scoreText.text = "Score : " + scoreData[idx].ToString();
        StartCoroutine(DelayHide());
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(0.5f);
        ScorePrefabs.SetActive(false);
    }

    public void ShowGameClear()
    {
        GameExit[0].SetActive(true);
    }
    public void ShowGameOver()
    {
        GameExit[1].SetActive(true);
    }
}
