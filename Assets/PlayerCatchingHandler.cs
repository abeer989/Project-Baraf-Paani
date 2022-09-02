using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCatchingHandler : MonoBehaviour
{
    public Action<PlayerManager> onTriggerEnterEvent;
    public Action<PlayerManager> onTriggerExitEvent;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
            return;

        Debug.Log("On Collsion Enter");
        var playerManager = collision.gameObject.GetComponentInParent<PlayerManager>();

        onTriggerEnterEvent?.Invoke(playerManager);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
            return;

        Debug.Log("On Collsion Exit");
        var playerManager = collision.gameObject.GetComponentInParent<PlayerManager>();


        onTriggerExitEvent?.Invoke(playerManager);
    }
}
