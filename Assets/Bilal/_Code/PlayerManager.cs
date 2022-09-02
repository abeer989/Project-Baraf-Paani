using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour, IPunInstantiateMagicCallback, IPunObservable
{
    public PlayerMovement playerMovementInstance;  // handles movement - Dash- Pickup
    public PlayerIndicatorHandler indicatorInstance;
    public PickUpHandler pickUpHandlerInstance;
    public PlayerCatchingHandler catchingHandlerInstance;

    [SerializeField] SpriteRenderer playerSprite;

    [SerializeField] Animator anim;

    public Player photonPlayer;
    public PhotonView photonView;

    public bool isSeeker = false;

    bool isNearTarget;
    PlayerManager targetPlayer;

    public bool isFrozen = false;

    bool m_firstTake = true;

    private void Start()
    {
        if (!photonView)
            photonView = GetComponent<PhotonView>();

        pickUpHandlerInstance = GetComponent<PickUpHandler>();
        pickUpHandlerInstance.Direction = new Vector2(0, 0);

        catchingHandlerInstance.onTriggerEnterEvent = WhenInRange;
        catchingHandlerInstance.onTriggerExitEvent = WhenOutOFRange;
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            if (isFrozen)
                return;

             var movement = playerMovementInstance.Movement();

            if (movement.sqrMagnitude > 0.1f)
            {
                pickUpHandlerInstance.Direction = movement.normalized;
            }

            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Speed", movement.sqrMagnitude);


            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!GameManager_Bilal.instance.GetGameActiveStatus())
                {
                    return;
                }

                if(isNearTarget)
                {
                    if(isSeeker)
                    {
                        Debug.Log("Catch");
                        if (targetPlayer)
                        {
                            //Debug.Log("freezing Player");
                            //targetPlayer.FreezePlayer();
                            if (!targetPlayer.isFrozen && !targetPlayer.isSeeker)
                            {
                                GameManager_Bilal.instance.RPC_Freeze(targetPlayer.photonView.ViewID);
                            }

                        }
                    }
                    else
                    {
                        if(targetPlayer)
                        {
                            if(targetPlayer.isFrozen)
                            {
                                GameManager_Bilal.instance.RPC_UnFreeze(targetPlayer.photonView.ViewID);
                            }
                        }

                    }
                }
            }

            


        }
    }

    private void FixedUpdate()
    {
        playerMovementInstance.MovementFixedUpdateFunction();
    }

    //+++++_________++++++++++++_____________++++++++++_____________++++++++++++++++++++++++++++++++++++++++
    //----------=============----------------------=============---------------------------------------------




    public void WhenInRange(PlayerManager target)
    {
        if (isNearTarget)
            return;

        isNearTarget = true;
        targetPlayer = target;
    }

    public void WhenOutOFRange(PlayerManager target)
    {
        if(targetPlayer!= target)
        {
            return;
        }
        isNearTarget = false;
        targetPlayer = null;

    }


    public void FreezePlayer()
    {
        playerSprite.color = Color.blue;
        isFrozen = true;
    }

    public void UnFreezePlayer()
    {
        playerSprite.color = Color.white;
        isFrozen = false;
    }

    

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Add(this);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       //if(stream.IsWriting)
       // {
       //     Debug.Log("is Writing");
       //     //stream.SendNextisFrozen);
       // }
       //else
       // {
       //     Debug.Log("is reading");
       //     if(this.m_firstTake)
       //     {
       //         m_firstTake = false;
       //     }

       //     isFrozen = (bool)stream.ReceiveNext();

       //     if (isFrozen)
       //         FreezePlayer();
       //     else
       //         UnFreezePlayer();

       // }
    }

    private void OnDestroy()
    {
        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Remove(this);    
    }

    public void ResetPlayerParams()
    {
        indicatorInstance.ResetIndicator();
        isFrozen = false;
        isSeeker = false;
        playerSprite.color = Color.white;
    }
}
