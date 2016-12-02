using System.Collections;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{

    [SerializeField]
    private float c_movementSpeed;
    private bool c_movingLeft = true;

    [SerializeField]
    SpriteRenderer c_skeletonSprite;
    [SerializeField]
    float c_hitSpeed;

    [SerializeField]
    private float c_stopAfterHitTime;

    [SerializeField]
    private int c_damage;
    private bool c_onHitCooldown = false;

    void Start()
    {
        if (c_movingLeft)
            c_movementSpeed *= -1;
    }

    void Update()
    {
        //move the skeleton
        Vector2 t_newPosition = transform.position;
        t_newPosition.x += Time.deltaTime * c_movementSpeed;
        transform.position = t_newPosition;

        //check if it's going to fall into a ravine
        Vector2 t_raycastOrigin;
        if (c_movingLeft)
            t_raycastOrigin = c_skeletonSprite.bounds.min;
        else
            t_raycastOrigin = c_skeletonSprite.bounds.max;

        if (Physics2D.Raycast(t_raycastOrigin, Vector2.down, 0.5f).collider == null)
            FlipMovementDirection();
    }

    void FlipMovementDirection()
    {
        c_skeletonSprite.flipX = !c_skeletonSprite.flipX;
        c_movementSpeed *= -1;
        c_movingLeft = !c_movingLeft;
    }

    public void OnCollisionStay2D(Collision2D p_collision)
    {
        if (c_onHitCooldown == false && p_collision.gameObject.tag == "Player")
        {
            p_collision.transform.root.GetComponent<HealthManager>().receiveDamage(c_damage);
            c_onHitCooldown = true;
            Vector2 t_hitForce = p_collision.transform.position - transform.position;
            p_collision.gameObject.GetComponent<InputManager>().ReceiveForce(t_hitForce.normalized * c_hitSpeed);
            StartCoroutine(CooldownAfterHit());
        }
    }

    public void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (p_collision.gameObject.tag != "Player")
            FlipMovementDirection();
    }

    IEnumerator CooldownAfterHit()
    {
        enabled = false; //deactivate the script so the skeleton stops moving
        yield return new WaitForSecondsRealtime(c_stopAfterHitTime);
        enabled = true;
        c_onHitCooldown = false;
    }
}