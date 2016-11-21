using System.Collections;
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
    private float c_flyingHorizontalSpeed;
    [SerializeField]
    private float c_verticalSpeed;
    [SerializeField]
    private float c_slidingCooldown;
    private float c_timeToSlide;
    private bool c_onGround;

    [SerializeField]
    private float c_attackCooldown;
    private float c_timeToAttack;
    #endregion

    void Start()
    {
        c_onGround = false;
        c_playerAnimator.SetBool("Jumping", !c_onGround);
    }
    
    void FixedUpdate()
    {
        float t_xForce = Input.GetAxisRaw("Horizontal");
        float t_yForce = Input.GetAxisRaw("Vertical");

        CheckMovementInput(t_xForce, t_yForce);
        CheckAttackInput();

        //set propper animation
        c_playerAnimator.SetInteger("SpeedX", Mathf.CeilToInt(c_playerRigidbody.velocity.x));

        //set script's facing direction
        if (t_xForce < 0 && !c_playerSpriteRenderer.flipX)
            c_playerSpriteRenderer.flipX = true;
        else if (t_xForce > 0 && c_playerSpriteRenderer.flipX)
            c_playerSpriteRenderer.flipX = false;
    }

    public void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (c_onGround)
            return;
        for (int t_collisionContact = 0; t_collisionContact < p_collision.contacts.Length; t_collisionContact++)
        {
            if (p_collision.contacts[t_collisionContact].normal == Vector2.up)
            {
                c_onGround = true;
                c_playerAnimator.SetBool("Jumping", !c_onGround);
                return;
            }
        }
    }

    private void CheckMovementInput(float p_xForce, float p_yForce)
    {
        if (c_onGround)
        {
            if (p_yForce == 0)
                c_playerRigidbody.velocity = new Vector2(p_xForce * c_horizontalSpeed, c_playerRigidbody.velocity.y);
            else if (p_yForce < 0)
            {
                c_playerRigidbody.velocity = new Vector2(p_xForce * c_horizontalSpeed, c_playerRigidbody.velocity.y);
                if ( p_xForce != 0 && Time.time > c_timeToSlide)
                {
                    c_playerAnimator.SetTrigger("Slide");
                    c_timeToSlide = Time.time + c_slidingCooldown;
                }
            }
            else
            {
                c_playerRigidbody.velocity = new Vector2(p_xForce * c_horizontalSpeed, c_playerRigidbody.velocity.y + c_verticalSpeed);
                c_onGround = false;
                c_playerAnimator.SetBool("Jumping", !c_onGround);
            }
        }
        else
            c_playerRigidbody.velocity = new Vector2(p_xForce * c_flyingHorizontalSpeed, c_playerRigidbody.velocity.y);
    }

    private void CheckAttackInput()
    {
        if(Input.GetButton("Fire1") && Time.time > c_timeToAttack)
        {
            c_playerWeaponManager.Attack();
            c_timeToAttack = Time.time + c_attackCooldown;
        }
    }
}
