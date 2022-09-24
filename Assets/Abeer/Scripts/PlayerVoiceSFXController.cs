using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceSFXController : MonoBehaviour
{
    [SerializeField] AudioClip baraf_SFX;
    [SerializeField] AudioClip pani_SFX;
    [SerializeField] AudioClip mujeBachao_SFX;
    [SerializeField] AudioClip kiaHaalHain_SFX;
    [SerializeField] AudioClip keseHo_SFX;

    List<AudioClip> greetingSFX;

    [SerializeField] AudioClip iceCrackSFx;

    [SerializeField] AudioSource audioSrc;

    private void Awake()
    {
        greetingSFX = new List<AudioClip>() { kiaHaalHain_SFX, keseHo_SFX };
    }

    private void Start()
    {
        
    }

    public void PlayGreetingBasedOnCharacter(int greetingIndex)
    {
        audioSrc.PlayOneShot(greetingSFX[greetingIndex]);
    }

    public void PlayBarafOneShot()
    {
        audioSrc.PlayOneShot(baraf_SFX);
    }

    public void PlayPaniOneShot()
    {
        audioSrc.PlayOneShot(pani_SFX);
    }

    public void PlayBachaoOneShot()
    {
        audioSrc.PlayOneShot(mujeBachao_SFX);
    }

    public void PlayKiaHaalOneShot()
    {
        audioSrc.PlayOneShot(kiaHaalHain_SFX);
    }

    public void PlayKeseHoOneShot()
    {
        audioSrc.PlayOneShot(keseHo_SFX);
    }

    public void PlayIceCrackSFXOneShot()
    {
        audioSrc.PlayOneShot(iceCrackSFx);
    }

}
