using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioClip titleMusic;
    [SerializeField] AudioClip roomMusic;
    [SerializeField] AudioClip gameMusic;

    AudioSource audioSrc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayTitleMusic() => PlayMusic(titleMusic);
    public void PlayRoomMusic() => PlayMusic(roomMusic);
    public void PlayGameMusic() => PlayMusic(gameMusic);

    void PlayMusic(AudioClip musicToPlay)
    {
        if (audioSrc.isPlaying)
            audioSrc.Stop();

        audioSrc.clip = musicToPlay;
        audioSrc.Play();
        
    }

    


}
