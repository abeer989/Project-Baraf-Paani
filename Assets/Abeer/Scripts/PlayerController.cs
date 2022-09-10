using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables/Ref.
    public static PlayerController instance;

    [Header("Components")]
    [SerializeField] Rigidbody2D RB;
    [SerializeField] Animator animator;

    [Header("After-image")]
    [SerializeField] SpriteRenderer playerSR;
    [SerializeField] SpriteRenderer afterImageSR;
    [SerializeField] Color afterImageColor;

    [Header("Freeze Shot")]
    [SerializeField] GameObject pivot;
    [SerializeField] FreezeShotController freezeShotController;

    [Header("Floats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashRechargeWait;
    [SerializeField] float timeBetweenAfterImages;
    [SerializeField] float afterImageLifeTime;

    BoxCollider2D playerBoundsBox;

    string horizontalAxis = "Horizontal";
    string verticalAxis = "Vertical";
    string lastExitUsed;

    float dashCounter;
    float dashRechargeCounter;
    float afterImageCounter;

    bool canMove = true;

    // public properties:
    public string LastExitUsed
    {
        get { return lastExitUsed; }
        set { lastExitUsed = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    } 
    #endregion

    void Update()
    {
        #region Vertical/Horizontal Movement:
        float horizontalMovement = Input.GetAxisRaw(horizontalAxis);
        float verticalMovement = Input.GetAxisRaw(verticalAxis);

        if (canMove)
        {
            //--> Dashing:
            if (dashRechargeCounter > 0)
                dashRechargeCounter -= Time.deltaTime;

            else
            {
                if (horizontalMovement != 0 || verticalMovement != 0)
                {
                    // if RMB is pressed:
                    if (Input.GetButtonDown("Fire2"))
                    {
                        dashCounter = dashTime;
                        ShowAfterImage();
                    } 
                }
            }

            if (dashCounter > 0)
            {
                if (horizontalMovement != 0 || verticalMovement != 0)
                {
                    dashCounter -= Time.deltaTime;
                    RB.velocity = new Vector2(dashSpeed * horizontalMovement, dashSpeed * verticalMovement);

                    // --> Showing After-images:
                    afterImageCounter -= Time.deltaTime;
                    if (afterImageCounter <= 0)
                        ShowAfterImage();

                    dashRechargeCounter = dashRechargeWait; // when the player has dashed once, don't let them dash again immediately.
                                                            // Instead, have a recharge timer in place 
                }
            }

            // --> Move normally if the player isn't dashing already:
            else
                RB.velocity = new Vector3(horizontalMovement, verticalMovement, 0) * moveSpeed;

            if (horizontalMovement == 1 || horizontalMovement == -1 || verticalMovement == 1 || verticalMovement == -1)
            {
                animator.SetFloat("lastMoveX", horizontalMovement);
                animator.SetFloat("lastMoveY", verticalMovement);
            }

            // keeping the player within the same bounds as the camera:
            if (playerBoundsBox)
            {
                transform.position = new Vector3(x: Mathf.Clamp(transform.position.x, playerBoundsBox.bounds.min.x + .5f, playerBoundsBox.bounds.max.x - .5f),
                                                 y: Mathf.Clamp(transform.position.y, playerBoundsBox.bounds.min.y, playerBoundsBox.bounds.max.y - 2),
                                                 z: transform.position.z);
            } 
        }

        else
            RB.velocity = new Vector3(horizontalMovement, verticalMovement, 0) * moveSpeed;

        animator.SetFloat("moveX", RB.velocity.x);
        animator.SetFloat("moveY", RB.velocity.y);
        #endregion
    }

    /// <summary>
    /// set bounds for player according to the camera bounds:
    /// </summary>
    /// <param name="_boundsBox"></param>
    public void SetBounds(BoxCollider2D _boundsBox) => playerBoundsBox = _boundsBox;

    public void ToggleFreezeShot(bool state)
    {
        pivot.SetActive(state);
        freezeShotController.enabled = state;
    }

    void ShowAfterImage()
    {
        SpriteRenderer afterImage = Instantiate(original: afterImageSR, position: transform.position, rotation: transform.rotation);
        afterImage.sprite = playerSR.sprite;
        afterImage.transform.localScale = transform.localScale;
        afterImage.color = afterImageColor;

        Destroy(afterImage.gameObject, afterImageLifeTime);
        afterImageCounter = timeBetweenAfterImages;
    }
}
