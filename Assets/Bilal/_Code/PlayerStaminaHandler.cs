using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStaminaHandler : MonoBehaviour
{
    [SerializeField] Slider staminabarSlider;

    [SerializeField] int maxStamina = 100;
    [SerializeField] int currentStamina;

    [Range(1,2)]
    [SerializeField] float incrementSpeed;

    WaitForSeconds initialRegenDelay = new WaitForSeconds(2.0f);
    WaitForSeconds regenTick = new WaitForSeconds(0.1f);


    Coroutine regenCoroutine;

    private void Start()
    {
        currentStamina = maxStamina;
        staminabarSlider.maxValue = maxStamina;
        staminabarSlider.value = maxStamina;
    }


    public void SetActiveStaminaSlider(bool state)
    {
        staminabarSlider.gameObject.SetActive(state);
    }

    
    public bool UseStamina(int cost)
    {
        if(currentStamina - cost > 0)
        {
            SetActiveStaminaSlider(false);

            currentStamina -= cost;
            staminabarSlider.value = currentStamina;

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);

            regenCoroutine = StartCoroutine(StaminaRegen());

            return true;
        }
        else
        {
            Debug.Log("Not Enough stamina");
            return false;
        }
    }


    private IEnumerator StaminaRegen()
    {


        yield return initialRegenDelay;

        SetActiveStaminaSlider(true);

        while(currentStamina < maxStamina)
        {
            currentStamina += Mathf.CeilToInt((maxStamina / 100) * incrementSpeed) ;
            staminabarSlider.value = currentStamina;
            yield return regenTick;
        }

        SetActiveStaminaSlider(false);

        regenCoroutine = null;
    }
}
