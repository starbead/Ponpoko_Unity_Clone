using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.SetAudio((int)AudioManager.audio.getitem);
            AudioManager.instance.PlayAudio();
            GameManager.instance.countUpScore();
            gameObject.SetActive(false);
            int idx = GameManager.instance.getGameScore();
            UIManager.instance.ShowGameScore(idx, gameObject.transform);
            if(idx == 9)
            {
                AudioManager.instance.SetAudio((int)AudioManager.audio.gameclear);
                AudioManager.instance.PlayAudio();
                UIManager.instance.ShowGameClear();
                Time.timeScale = 0;
            }
        }
    }
}
