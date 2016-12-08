using UnityEngine;
using System.Collections;

public class SkeletonHealthManager : MonoBehaviour {


    [SerializeField]
    private int c_maxHealth;
    private int c_currentHealth;
    
    void Start()
    {
        c_currentHealth = c_maxHealth;
    }

    public void ReceiveDamage(int p_damage)
    {
        c_currentHealth -= p_damage;
        if (c_currentHealth <= 0)
            OnDie();
    }

    private void OnDie()
    {
        gameObject.SetActive(false);
    }
}