using TMPro;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] GameObject interactionCanvas;
    [SerializeField] TextMeshProUGUI barafIndicatorText;

    bool isBaraf;
    bool barafPossible;

    public bool IsBaraf { get { return isBaraf; } }

    void Update()
    {
        if (barafPossible)
        {
            interactionCanvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                barafPossible = false;
                interactionCanvas.SetActive(false);
                Baraf();
            }
        }

        else
            interactionCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            barafPossible = true;
    }    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            barafPossible = false;
    }

    public void Baraf() => isBaraf = true;
}
