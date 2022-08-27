using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] List<Transform> movePoints;

    [Space]
    [SerializeField] string characterName;

    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    Transform jail;
    SpriteRenderer spriteRenderer;
    StateController state;

    int currentTargetedPoint;
    float waitCounter;

    private void OnEnable()
    {
        jail = FindObjectOfType<Jail>().transform;
        state = GetComponentInChildren<StateController>();

        gameObject.name = characterName;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.sprite = sprite;

        movePoints[0].parent.name = gameObject.name + "'s MovePoints";
        movePoints[0].parent.SetParent(null);

        currentTargetedPoint = 0;
        waitCounter = waitTime;
    }

    private void Update()
    {
        if (!state.IsBaraf)
        {
            if (movePoints.Count > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePoints[currentTargetedPoint].position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, movePoints[currentTargetedPoint].position) < 0.02)
                {
                    waitCounter -= Time.deltaTime;

                    if (waitCounter <= 0)
                    {
                        currentTargetedPoint++;

                        if (currentTargetedPoint >= movePoints.Count)
                            currentTargetedPoint = 0;

                        waitCounter = waitTime;
                    }
                }
            } 
        }

        else
        {
            if (transform.position != jail.position)
                StartCoroutine(nameof(GoToJailCR));
        }
    }

    IEnumerator GoToJailCR()
    {
        yield return new WaitForSeconds(.5f);

        transform.position = jail.position;

        yield break;
    }
}
