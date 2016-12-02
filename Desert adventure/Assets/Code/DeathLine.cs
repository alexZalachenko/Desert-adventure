using UnityEngine;

public class DeathLine : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.gameObject.tag != "Player")
            Destroy(p_collider.gameObject);
        else
            p_collider.transform.root.gameObject.GetComponent<HealthManager>().receiveDamage(1000);
    }
}