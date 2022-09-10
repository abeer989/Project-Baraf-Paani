using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall_multi : MonoBehaviour
{
    [HideInInspector]
    public Vector2 moveDir;

    [SerializeField] Rigidbody2D RB;
    //[SerializeField] GameObject impactFX;

    [Space]
    [SerializeField] float speed;

    public int senderId;

    void Update() => RB.velocity = moveDir * speed;


    

    


    private void Start()
    {
       

        //if(!pV.IsMine)
        //{
        //    isLocal = false;
        //}
        //else
        //{
        //    isLocal = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            

        if (!collision.gameObject.tag.Equals("Player"))
            return;

       
            var targetPlayer = collision.gameObject.GetComponentInParent<PlayerManager>();
            
            if (targetPlayer.photonView.ViewID == senderId)
                return;

            if (!targetPlayer.isFrozen && !targetPlayer.isSeeker)
            {
                //GameManager_Bilal.instance.RPC_Freeze(targetPlayer.photonView.ViewID);
                targetPlayer.FreezePlayer();


            }

        


        Destroy(gameObject);

    }

    private void OnBecameInvisible() => Destroy(gameObject);
}
