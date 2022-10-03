using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField]
    public bool TrapPot;
    public GameObject PotbugPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!TrapPot)
            {
                AudioManager.instance.SetAudio((int)AudioManager.audio.getitem);
                AudioManager.instance.PlayAudio();
                GameManager.instance.countUpScore();
                gameObject.SetActive(false);
                int idx = GameManager.instance.getGameScore();
                UIManager.instance.ShowGameScore(idx, gameObject.transform);
            }
            else
            {
                //potbug spawn
                PotbugPrefab.SetActive(true);
            }
        }
    }
}
