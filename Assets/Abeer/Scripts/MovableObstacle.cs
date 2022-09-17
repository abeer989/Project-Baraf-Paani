using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : MonoBehaviour
{
    [SerializeField] GameObject interactionCanvas;
    [SerializeField] List<Transform> movePoints;

    [SerializeField] float moveSpeed;

    int curretTargetedPointIndex;
    bool inContact;


    private void OnEnable()
    {
        movePoints[0].parent.name = gameObject.name + "'s MovePoints";
        movePoints[0].parent.SetParent(null);

        curretTargetedPointIndex = 0;
    }

    void Update()
    {
        if (inContact)
        {
            interactionCanvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                curretTargetedPointIndex++;

                if (curretTargetedPointIndex >= movePoints.Count)
                    curretTargetedPointIndex = 0;
            }
        }

        else
            interactionCanvas.SetActive(false);

        if (transform.position != movePoints[curretTargetedPointIndex].position)
            transform.position = Vector2.MoveTowards(transform.position, movePoints[curretTargetedPointIndex].position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            inContact = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.playerTag))
            inContact = false;
    }
}
