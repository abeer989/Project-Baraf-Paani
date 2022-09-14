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
    public PlayerStaminaHandler staminaHandlerInstance;
    public PlayerFreezeShotController freezeShotControllerInstance;
    public PlayerPowerUpHandler powerUpHandlerInstance;


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


    Vector2 updatedMovement;

    Vector2 networkPos;



    bool isIceGunActive = false;
    bool isInvincible = false;

    private void Start()
    {
        


        if (!photonView)
            photonView = GetComponent<PhotonView>();


        photonPlayer = photonView.Owner;

        

        pickUpHandlerInstance = GetComponent<PickUpHandler>();
        pickUpHandlerInstance.Direction = new Vector2(0, 0);

        catchingHandlerInstance.onTriggerEnterEvent = WhenInRange;
        catchingHandlerInstance.onTriggerExitEvent = WhenOutOFRange;

        powerUpHandlerInstance.onIceGunActive = ActivateIceGun;

        powerUpHandlerInstance.onInvincActive = ActivateInvincibility;

        if (!photonView.IsMine)
        {
            Destroy(playerMovementInstance.rb);
            
        }
        else
        {
           
            indicatorInstance.SetActiveLocalIndicator(true);
        }

    }

    
    
    private void Update()
    {

        if (!photonView.IsMine)
        {
            transform.position = Vector3.MoveTowards(transform.position, networkPos, Time.deltaTime * playerMovementInstance.moveSpeed);

            return;
        }


        Vector2 oldPosition = transform.position;

        if(photonView.IsMine)
        {
            if (isFrozen || isLocked)
                return;

            bool isDashing = false;

            if(Input.GetKeyDown(playerMovementInstance.DashingKey))
            {
                isDashing =  staminaHandlerInstance.UseStamina(40);
            }

            var movement = playerMovementInstance.Movement(isDashing);

            if (movement.sqrMagnitude > 0.1f)
            {
                pickUpHandlerInstance.Direction = movement.normalized;
            }

            SetAnimation(movement);


            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (!GameManager_Bilal.instance.GetGameActiveStatus())
                //{
                //    return;
                //}

                if (isNearTarget)
                {
                    if (isSeeker)
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
                        if (targetPlayer)
                        {
                            if (targetPlayer.isFrozen)
                            {
                                //GameManager_Bilal.instance.RPC_UnFreeze(targetPlayer.photonView.ViewID);
                                targetPlayer.UnFreezePlayer();
                            }
                        }

                    }
                }
                else
                {
                    if(isIceGunActive)
                    {
                        photonView.RPC(
                       nameof(freezeShotControllerInstance.RPC_FireIceBall),
                       RpcTarget.All, movement,
                       photonView.ViewID
                       );
                    }
                        

                    //freezeShotControllerInstance.FireIceBall(movement,photonView.ViewID);
                   
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                pickUpHandlerInstance.PickAndThrowFunction();
            }



        }

        updatedMovement = (Vector2)transform.position - oldPosition;
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

        if (!photonView.IsMine) return;

        if (isNearTarget)
            return;

        target.indicatorInstance.SetACtiveActionIndicator(true);

        //if (isSeeker && !target.isSeeker || !isSeeker && target.isFrozen)
        //{
        //    target.SetActiveActionIndicator(true);
        //}

        isNearTarget = true;
        targetPlayer = target;
    }

    public void WhenOutOFRange(PlayerManager target)
    {
        if (!photonView.IsMine) return;

        if (targetPlayer != target)
        {
            return;
        }

        target.indicatorInstance.SetACtiveActionIndicator(false);

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

    #region PowerUpFunctions

    public void ActivateIceGun()
    {
        if (isIceGunActive)
            return;

        isIceGunActive = true;
        StartCoroutine(powerUpHandlerInstance.PowerCountDown(StopIceGun));
    }

    public void StopIceGun()
    {
        isIceGunActive = false;
    }

    public void ActivateInvincibility()
    {
      
       StartCoroutine(powerUpHandlerInstance.PowerCountDown(StopInvincibility));
       

       Debug.Log("Active Invincibiltiy");

       photonView.RPC(nameof(RPC_NotifyInvincible), RpcTarget.All, true);

    }

    public void StopInvincibility()
    {
        Debug.Log("Active Invincibiltiy");

        photonView.RPC(nameof(RPC_NotifyInvincible), RpcTarget.All, false) ;
    }

    #endregion


    #region Photon Phunctions

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Add(this);
    }

   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(updatedMovement);
        }
        else
        {
            networkPos = (Vector2)stream.ReceiveNext();
            updatedMovement = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPos += (updatedMovement * lag);

        }


        //if(stream.IsWriting)
        //{
        //    stream.SendNext(playerMovementInstance.rb.position);
        //    stream.SendNext(playerMovementInstance.rb.rotation);
        //    stream.SendNext(playerMovementInstance.rb.velocity);
        //}
        //else
        //{
        //    playerMovementInstance.rb.position = (Vector2) stream.ReceiveNext();
        //    playerMovementInstance.rb.rotation = (float)stream.ReceiveNext();
        //    playerMovementInstance.rb.velocity = (Vector2)stream.ReceiveNext();
        //}
        //// throw new System.NotImplementedException();
        //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        //playerMovementInstance.rb.position += playerMovementInstance.rb.velocity * lag;
    }


    [PunRPC]
    private void RPC_NotifyInvincible(bool state)
    {
        Debug.Log($"Is invincible = {state}");
        isInvincible = state;
        
      


    }


    [PunRPC]
    private void RPC_Freeze()
    {
        if (isInvincible)
            return;
        

        Debug.Log($"{photonView.ViewID} Freezing me");

       

        playerSprite.color = Color.blue;
        isFrozen = true;
        SetAnimation(Vector2.zero);
        playerMovementInstance.StopMovement();
        indicatorInstance.SetActiveBrufIndicator(true);
        
        

    }



    [PunRPC]
    private void RPC_UnFreeze()
    {
        Debug.Log($"{photonView.ViewID} UN Freezing me");

        if (!photonView.IsMine)
            return;

        playerSprite.color = Color.white;
        isFrozen = false;
        indicatorInstance.SetActiveBrufIndicator(false);

    }


    #endregion




    


    private void OnDestroy()
    {
        GameManager_Bilal.instance.spawnPlayersInstance.playersInRoom_List.Remove(this);    
    }

    
}
