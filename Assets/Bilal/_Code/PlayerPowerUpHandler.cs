using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Photon.Realtime;

public class PlayerPowerUpHandler : MonoBehaviour
{

    


    [SerializeField] PlayerManager managerInstance;


    [SerializeField] Slider powerbarSlider;

    [SerializeField] int maxPower = 100;
    [SerializeField] int currentPower;

    WaitForSeconds depleteTick = new WaitForSeconds(0.1f);

    [Range(1, 2)]
    [SerializeField] float incrementSpeed;

    Coroutine depleteCoroutine;

   
    public Action onIceGunActive;
    public Action onInvincActive;

    private void Start()
    {
        currentPower = maxPower;
        powerbarSlider.maxValue = maxPower;
        powerbarSlider.value = maxPower;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("PowerUp"))
            return;


        Debug.Log("On trigger With powerup");

        // Enable iceGun

        var powerUp = collision.gameObject.GetComponent<PowerUpItem>();

        if(powerUp.type == PowerUpItem.PowerUpType.IceGun)
        {
            // do this

            onIceGunActive?.Invoke();
            


        }
        else if(powerUp.type == PowerUpItem.PowerUpType.TwoTimes)
        {
            // do this

            onInvincActive?.Invoke();
        }




        powerUp.SetActiveMe(false);
       // Destroy(powerUp.gameObject);
    }

    public void SetActivePowerSlider(bool state)
    {
        powerbarSlider.gameObject.SetActive(state);
    }

    public IEnumerator PowerCountDown(Action ontimeOut)
    {
        SetActivePowerSlider(true);

        while(currentPower> 0)
        {
            currentPower -= Mathf.CeilToInt((maxPower / 100) * incrementSpeed);
            powerbarSlider.value = currentPower;
            yield return depleteTick;
        }

        ontimeOut?.Invoke();

        SetActivePowerSlider(false);


    }

   


    public void ActivateIceGun()
    {
        // unlocks ice gun feature
        // starts icegun counter

        


    }

    
}
