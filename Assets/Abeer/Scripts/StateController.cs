using TMPro;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] GameObject interactionCanvas;
    //[SerializeField] TextMeshProUGUI barafIndicatorText;

    bool isBaraf;
    bool stateChangePossible;

    public bool IsBaraf { get { return isBaraf; } }

    void Update()
    {
        if (stateChangePossible)
        {
            interactionCanvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                //stateChangePossible = false;
                //interactionCanvas.SetActive(false);

                if (!isBaraf)
                    Baraf();

                else
                    Paani();
            }
        }

        else
            interactionCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            stateChangePossible = true;
    }    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            stateChangePossible = false;
    }

    public void Baraf()
    {
        GameManager.instance.UpdateScore(1);
        isBaraf = true;
    }

    public void Paani()
    {
        GameManager.instance.UpdateScore(-1);
        isBaraf = false;
        GetComponentInParent<NPCController>()?.JailCRBoolOff();
    }
}
