using TMPro;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] GameObject interactCanvas;
    [SerializeField] TextMeshProUGUI iceBallText;

    [Space]
    [SerializeField] float cooldownTime;

    bool inContact;
    float cooldownCounter;

    private void OnEnable()
    {
        //cooldownCounter = cooldownTime;
    }

    private void Update()
    {
        if(cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            SetIceBallText(Mathf.FloorToInt(Mathf.Clamp(cooldownCounter, 0, int.MaxValue)));
        }

        if (cooldownCounter <= 0)
        {
            iceBallText.SetText("ICE BALL AVAILABLE!");

            if (inContact)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerController.instance.ToggleFreezeShot(true);

                    if (interactCanvas.activeInHierarchy)
                        interactCanvas.SetActive(false);

                    cooldownCounter = cooldownTime;
                }
            } 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Tags.playerTag))
        {
            if (cooldownCounter <= 0)
            {
                interactCanvas.transform.position = other.contacts[0].point;

                if (!interactCanvas.activeInHierarchy)
                    interactCanvas.SetActive(true);

                inContact = true; 
            }
        }
    }    
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Tags.playerTag))
        {
            if(interactCanvas.activeInHierarchy)
                interactCanvas.SetActive(false);

            inContact = false;
        }
    }

    void SetIceBallText(int timeLeft)
    {
        iceBallText.SetText("ICE BALL IN: " + timeLeft.ToString() + "s");
    }
}
