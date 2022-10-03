using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int floor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.setTimer(floor);
            GameManager.instance.diedPonpoko();
            AudioManager.instance.SetAudio(floor);
            AudioManager.instance.PlayAudio();
            GameManager.instance.restartGame(floor);
        }
    }
}
