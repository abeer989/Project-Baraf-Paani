using UnityEngine;
using System.Collections.Generic;

public class GuardPatrolAndChaseAI : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] List<Transform> movePoints;

    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    SpriteRenderer spriteRenderer;

    int currentTargetedPoint;
    float waitCounter;
    bool move;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer && sprite)
            spriteRenderer.sprite = sprite;

        movePoints[0].parent.name = gameObject.name + "'s MovePoints";
        movePoints[0].parent.SetParent(null);

        move = true;
    }

    private void Update()
    {
        if (!Jail.instance.RunnerInRange)
        {
            if (move)
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
        }

        else
        {
            if (PlayerController.instance != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, PlayerController.instance.transform.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < 0.2f)
                {
                    Debug.Log("seeker caught");
                }
            }
        }
    }
}
