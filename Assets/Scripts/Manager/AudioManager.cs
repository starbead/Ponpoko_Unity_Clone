using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum audio
    {
        death1,
        death2,
        death3,
        death4,
        death5,
        gameclear,
        gameover,
        getitem,
        trappot,
    }

    public static AudioManager instance;

    [SerializeField]
    AudioClip[] audioclips;

    AudioSource audioSrc;

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
        audioSrc = GetComponent<AudioSource>();
    }

    public void SetAudio(int idx)
    {
        audioSrc.clip = audioclips[idx];
    }

    public void PlayAudio()
    {
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    public void StopAudio()
    {
        audioSrc.Stop();
    }
}
