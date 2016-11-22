using System.Collections;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{

    [SerializeField]
    private float c_movementSpeed;//supposing the sprite is moving to the right
    private bool c_movingLeft = true;

    [SerializeField]
    SpriteRenderer c_skeletonSprite;
    [SerializeField]
    Vector2 c_hitSpeed;//supposing the sprite is moving to the right

    [SerializeField]
    private float c_stopAfterHitTime;

    private bool c_onHitCooldown = false;

    void Start()
    {
        if (c_movingLeft)
        {
            c_movementSpeed *= -1;
            c_hitSpeed.x *= -1;
        }
    }

    // Update is called once per frame
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

    void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (c_onHitCooldown == false && p_collision.gameObject.name == "Player")
        {
            c_onHitCooldown = true;
            StartCoroutine(CooldownAfterHit());
            Debug.Log("QuitoVida");
        }
    }

    IEnumerator CooldownAfterHit()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        //gameObject.GetComponent<Collider2D>().enabled = false;
        enabled = false; //deactivate the script so the skeleton stops moving
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(c_stopAfterHitTime);
        //gameObject.GetComponent<Collider2D>().enabled = true;
        enabled = true;
        c_onHitCooldown = false;
    }
}