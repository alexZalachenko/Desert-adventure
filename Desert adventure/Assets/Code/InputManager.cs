using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Rigidbody2D c_playerRigidbody;
    [SerializeField]
    private SpriteRenderer c_playerSpriteRenderer;
    [SerializeField]
    private Animator c_playerAnimator;
    [SerializeField]
    private WeaponManager c_playerWeaponManager;

    [SerializeField]
    private float c_horizontalSpeed;
    [SerializeField]
    private float c_maxSpeed;
    [SerializeField]
    private float c_flyingSpeed;
    [SerializeField]
    private float c_maxFlyingSpeed;
    [SerializeField]
    private float c_verticalSpeed;
    [SerializeField]
    private float c_slidingCooldown;
    private float c_timeToSlide;
    private bool c_onGround;

    [SerializeField]
    private float c_attackCooldown;
    private float c_timeToAttack;

    [SerializeField]
    private LayerMask c_terrainLayer;

    private float c_quarterSprite;

    //user input
    float c_xForce;
    float c_yForce;

    #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    [SerializeField]
    //which percentage of the screen (in normalized device coordinates) will make the player move when touched
    private float c_movementTouchBound;

    private float c_rightTouchBoundPixels;
    private float c_leftTouchBoundPixels;

    private Vector2 c_touchOrigin;
    private float c_minTravelToAction = 1;

    [SerializeField]
    private float c_timeUntilHold = 1;//time until a tap becomes a hold
    private float c_holdingTime = 0;
    private float c_tapHoldingThreshold = 0.3f;//when tapping, some time passes since the user presses and releases. Holding values below this value will be assumed to be a tap
    #endif

    //for checking if the player is grounded
    private Transform c_footTransform; 
    #endregion

    void Start()
    {
        c_onGround = false;
        c_playerAnimator.SetBool("Jumping", !c_onGround);

        c_footTransform = transform.GetChild(0).transform;

        c_quarterSprite = c_playerSpriteRenderer.bounds.extents.x * 0.5f;

        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        c_leftTouchBoundPixels = Screen.width * c_movementTouchBound * 0.01f;
        c_rightTouchBoundPixels = Screen.width - c_leftTouchBoundPixels;
        #endif
    }

    void Update()
    {
        CheckPlayerInput();
        #if UNITY_STANDALONE
        CheckAttackInput();
        #endif
        CheckGrounded();
    }

    void FixedUpdate()
    {
        CheckMovementInput();

        //set propper animation
        c_playerAnimator.SetInteger("SpeedX", Mathf.RoundToInt(Mathf.Abs(c_playerRigidbody.velocity.x)));

        //set script's facing direction
        if (c_xForce < 0 && !c_playerSpriteRenderer.flipX)
            c_playerSpriteRenderer.flipX = true;
        else if (c_xForce > 0 && c_playerSpriteRenderer.flipX)
            c_playerSpriteRenderer.flipX = false;
    }

    private void CheckMovementInput()
    {
        if (c_onGround)
        {
            if (c_xForce * c_playerRigidbody.velocity.x < c_maxSpeed)
                c_playerRigidbody.AddForce(Vector2.right * c_xForce * c_horizontalSpeed, 0);
        }
        else
        {
            if (c_xForce * c_playerRigidbody.velocity.x < c_maxFlyingSpeed)
                c_playerRigidbody.AddForce(Vector2.right * c_xForce * c_flyingSpeed);
        }

        //sliding
        if (c_yForce < 0 && Time.time > c_timeToSlide && c_xForce != 0)
        {
            c_playerAnimator.SetTrigger("Slide");
            c_timeToSlide = Time.time + c_slidingCooldown;
        }

        //jumping
        if (c_yForce > 0 && c_onGround)
        {
            c_playerRigidbody.AddForce(Vector2.up * c_verticalSpeed);
            c_onGround = false;
            c_playerAnimator.SetBool("Jumping", true);
            transform.parent = null;
        }
    }

    //on mobile devices I will check attack input inside private void CheckPlayerInput() for sake of performance
    private void CheckAttackInput()
    {
        if (Input.GetButton("Fire1") && Time.time > c_timeToAttack)
        {
            c_playerWeaponManager.Attack();
            c_timeToAttack = Time.time + c_attackCooldown;
        }
    }

    private void CheckGrounded()
    {
        //fire three rays to see if the player is on ground
        Vector3[] t_checkPositions = { c_footTransform.position,
                                       c_footTransform.position,
                                       c_footTransform.position
                                     };

        t_checkPositions[0].x -= c_quarterSprite;
        t_checkPositions[2].x += c_quarterSprite;

        Collider2D t_raycastCollider;
        for (int t_position = 0; t_position < t_checkPositions.Length; t_position++)
        {
            t_raycastCollider = Physics2D.Raycast(t_checkPositions[t_position], Vector2.down, 0.01f, c_terrainLayer).collider;

            if (t_raycastCollider != null)//The player is grounded
            {
                if (t_raycastCollider.gameObject.tag == "MovingPlatform" && transform.parent == null)
                    transform.parent = t_raycastCollider.transform.parent.parent;

                if (c_onGround == false)
                {
                    c_onGround = true;
                    c_playerAnimator.SetBool("Jumping", false);
                }
                return;
            }
        }
        //The player is flying
        if (c_onGround == true)
        {
            c_onGround = false;
            c_playerAnimator.SetBool("Jumping", true);
            transform.parent = null;
        }
    }

    //on mobile devices I will check attack input from here instead of from private void CheckAttackInput() for sake of performance
    private void CheckPlayerInput()
    {
        #if UNITY_STANDALONE

        c_xForce = Input.GetAxisRaw("Horizontal");
        c_yForce = Input.GetAxisRaw("Vertical");

        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        c_xForce = c_yForce = 0;

        if (Input.touchCount > 0)
        {
            Touch[] t_touches = Input.touches;
            for (int t_index = 0; t_index < Input.touchCount; t_index++)
            {
                //right side of screen
                if (t_touches[t_index].position.x > c_rightTouchBoundPixels)
                {
                    if (t_touches[t_index].phase == TouchPhase.Stationary)
                        c_xForce = 1;
                }
                //left side of screen
                else if (t_touches[t_index].position.x < c_leftTouchBoundPixels)
                {
                    if (t_touches[t_index].phase == TouchPhase.Stationary)
                        c_xForce = -1;
                }
                //center of screen
                else
                {
                    if (t_touches[t_index].phase == TouchPhase.Began)
                        c_touchOrigin = t_touches[t_index].position;

                    else if (t_touches[t_index].phase == TouchPhase.Ended)
                    {
                        Vector2 t_travel = t_touches[t_index].position - c_touchOrigin;

                        //it's a tap (IT'S A TRAP!!!! AAAARGH)
                        if (t_travel.sqrMagnitude == 0 && Time.time > c_timeToAttack)
                        {
                            if (c_holdingTime > c_tapHoldingThreshold)
                                c_holdingTime = 0;
                            else
                            {
                                c_playerWeaponManager.Attack();
                                c_timeToAttack = Time.time + c_attackCooldown;
                            }
                        }
                        //slided finger
                        else if (t_travel.sqrMagnitude > c_minTravelToAction && Mathf.Abs(t_travel.y) > Mathf.Abs(t_travel.x))
                            c_yForce = t_travel.y > 0 ? 1 : -1;
                    }
                    else
                    {
                        //touch holded
                        if (c_holdingTime > c_timeUntilHold)
                        {
                            //abrir menu opciones ingame
                            Debug.Log("holding");
                            c_holdingTime = 0;
                        }
                        else
                            c_holdingTime += t_touches[t_index].deltaTime;
                    }
                }
            }
        }
        #endif
    }

    public void ReceiveForce(Vector2 p_forceToAdd)
    {
        c_playerRigidbody.AddForce(p_forceToAdd);
    }
}
