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

    [SerializeField] GameObject damageNumbersPrefab;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] StateController state;

    [SerializeField] bool move;

    Jail jail;

    int currentTargetedPoint;
    float waitCounter;
    bool goToJailCRCalledOnce;

    private void OnEnable()
    {
        jail = FindObjectOfType<Jail>();
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
            if (!goToJailCRCalledOnce)
                StartCoroutine(nameof(GoToJailCR));
        }
    }

    IEnumerator GoToJailCR()
    {
        goToJailCRCalledOnce = true;

        Instantiate(damageNumbersPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        yield return new WaitForSeconds(1f);

        int rand = Random.Range(0, jail.jailPoints.Count);

        int whileBreaker = 0;
        while (jail.jailPoints[rand].isOccupied && whileBreaker < 50)
        {
            rand = Random.Range(0, jail.jailPoints.Count);
            whileBreaker++;
        }

        JailPoint jp = jail.jailPoints[rand];
        transform.position = jp.transform.position;
        jp.isOccupied = true;

        yield break;
    }

    public void JailCRBoolOff() => goToJailCRCalledOnce = false;
}
