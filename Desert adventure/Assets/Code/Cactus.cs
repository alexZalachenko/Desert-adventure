using UnityEngine;
using System.Collections;

public class Cactus : MonoBehaviour
{
    [SerializeField]
    private int c_damage;
    private bool c_onHitCooldown = false;
    [SerializeField]
    float c_hitSpeed;
    [SerializeField]
    private float c_stopAfterHitTime;


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

    IEnumerator CooldownAfterHit()
    {
        yield return new WaitForSecondsRealtime(c_stopAfterHitTime);
        c_onHitCooldown = false;
    }
}


