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

    private bool isLocked = false;

    bool m_firstTake = true;

    private void Start()
    {
        if (!photonView)
            photonView = GetComponent<PhotonView>();

        pickUpHandlerInstance = GetComponent<PickUpHandler>();
        pickUpHandlerInstance.Direction = new Vector2(0, 0);

        catchingHandlerInstance.onTriggerEnterEvent = WhenInRange;
        catchingHandlerInstance.onTriggerExitEvent = WhenOutOFRange;

        if(!photonView.IsMine)
        {
            Destroy(playerMovementInstance.rb);
        }

    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            if (isFrozen || isLocked)
                return;

            var movement = playerMovementInstance.Movement();

            if (movement.sqrMagnitude > 0.1f)
            {
                pickUpHandlerInstance.Direction = movement.normalized;
            }

            SetAnimation(movement);


            if (Input.GetKeyDown(KeyCode.E))
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
                                //GameManager_Bilal.instance.RPC_Freeze(targetPlayer.photonView.ViewID);
                                targetPlayer.FreezePlayer();
                            }

                        }
                    }
                    else
                    {
                        if(targetPlayer)
                        {
                            if(targetPlayer.isFrozen)
                            {
                                //GameManager_Bilal.instance.RPC_UnFreeze(targetPlayer.photonView.ViewID);
                                targetPlayer.UnFreezePlayer();
                            }
                        }

                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                pickUpHandlerInstance.PickAndThrowFunction();
            }



        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine )
            return;

        

        playerMovementInstance.MovementFixedUpdateFunction();
    }

    //+++++_________++++++++++++_____________++++++++++_____________++++++++++++++++++++++++++++++++++++++++
    //----------=============----------------------=============---------------------------------------------



    #region Range Functions
    public void WhenInRange(PlayerManager target)
    {
        if (isNearTarget)
            return;

        isNearTarget = true;
        targetPlayer = target;
    }

    public void WhenOutOFRange(PlayerManager target)
    {
        if (targetPlayer != target)
        {
            return;
        }
        isNearTarget = false;
        targetPlayer = null;

    }
    #endregion


    #region Freeze / UnFreeze Function
    //public void FreezePlayer()
    //{
    //    playerSprite.color = Color.blue;
    //    isFrozen = true;
    //}

    //public void UnFreezePlayer()
    //{
    //    playerSprite.color = Color.white;
    //    isFrozen = false;
    //}

    public void FreezePlayer()
    {
        Debug.Log("FreezingPlayer");
        photonView.RPC(nameof(RPC_Freeze), RpcTarget.All);
    }

    public void UnFreezePlayer()
    {
        Debug.Log("UnfreezePlayer");
        photonView.RPC(nameof(RPC_UnFreeze), RpcTarget.All);
    }
    #endregion


    #region Utility Functions

    void SetAnimation(Vector2 movement)
    {
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }
    public void ResetPlayerParams()
    {
        indicatorInstance.ResetIndicator();
        isFrozen = false;
        isSeeker = false;
        playerSprite.color = Color.white;
    }

    public void LockPlayer()
    {
        isLocked = true;
        SetAnimation(Vector2.zero);
        playerMovementInstance.StopMovement();


    }

    public void UnlockPlayer()
    {
        isLocked = false;
    }
    #endregion




    #region Photon Phunctions

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Add(this);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }

    [PunRPC]
    private void RPC_Freeze()
    {

        Debug.Log($"{photonView.ViewID} Freezing me");

       

        playerSprite.color = Color.blue;
        isFrozen = true;
        SetAnimation(Vector2.zero);
        playerMovementInstance.StopMovement();

    }



    [PunRPC]
    private void RPC_UnFreeze()
    {
        Debug.Log($"{photonView.ViewID} UN Freezing me");

        if (!photonView.IsMine)
            return;

        playerSprite.color = Color.white;
        isFrozen = false;
        

    }


    #endregion




    


    private void OnDestroy()
    {
        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Remove(this);    
    }

    
}
