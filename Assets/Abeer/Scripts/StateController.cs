using UnityEngine;

public class StateController : MonoBehaviour
{
    bool isBaraf;
    bool barafPossible;

    public bool IsBaraf { get { return isBaraf; } }

    void Update()
    {
        if (barafPossible)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Baraf();
        }
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
