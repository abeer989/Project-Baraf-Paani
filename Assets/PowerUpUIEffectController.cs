using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class PowerUpUIEffectController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI iceText;
    [SerializeField] TextMeshProUGUI invincibilityText;

    [SerializeField] AudioClip iceGunEffectSFX;
    [SerializeField] AudioClip invincibilitySFX;

    [SerializeField] float tweenDuration;
    [SerializeField] float fadeDuration;
    [SerializeField] Ease easeType;


    [SerializeField] AudioSource audioSrc;

    Sequence iceSeq;

    Sequence invinSeq;

    private void Start()
    {
        TweenIceSetup();
        TweenInvinSetup();
    }


    public void SetActiveIceTween()
    {
        iceText.gameObject.SetActive(true);
        audioSrc.PlayOneShot(iceGunEffectSFX);
        iceSeq.Play();

    }

    public void SetActiveInvinciTween()
    {
        invincibilityText.gameObject.SetActive(true);
        audioSrc.PlayOneShot(invincibilitySFX);
        invinSeq.Play();
    }

    void TweenIceSetup()
    {
        iceSeq = DOTween.Sequence();

        iceSeq.Append(iceText.transform.DOScale(Vector3.one,tweenDuration).SetEase(easeType));
        iceSeq.Append(iceText.DOFade(0, fadeDuration));
        iceSeq.AppendCallback(ResetIceText);



    }

    void TweenInvinSetup()
    {
        invinSeq = DOTween.Sequence();

        invinSeq.Append(invincibilityText.transform.DOScale(Vector3.one, tweenDuration).SetEase(easeType));
        invinSeq.Append(invincibilityText.DOFade(0, fadeDuration));
        invinSeq.AppendCallback(ResetInvinText);
    }
    
    void ResetInvinText()
    {
        invincibilityText.transform.localScale = Vector3.zero;
        invincibilityText.gameObject.SetActive(false);
    }

    void ResetIceText()
    {
        iceText.transform.localScale = Vector3.zero;
        iceText.gameObject.SetActive(false);
    }



}
