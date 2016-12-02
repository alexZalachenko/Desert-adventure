using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    [SerializeField]
    GameObject c_heart;
    [SerializeField]
    GameObject c_halfHeart;
    [SerializeField]
    Transform c_heartHolder;

    [SerializeField]
    private int c_maxHealth;
    private int c_currentHealth;

    private GameObject[] c_hearts;

    void Start()
    {
        c_currentHealth = c_maxHealth;
        c_hearts = new GameObject[c_maxHealth];

        Vector2 t_offsetVect = Vector2.zero;
        float t_offset = c_heart.GetComponent<RectTransform>().rect.width;

        float t_anchorsOffset = 1f / (c_maxHealth / 2f);
        Vector2 t_anchorMin = new Vector2(0, 0);
        Vector2 t_anchorMax = new Vector2(t_anchorsOffset, 1);

        for (int t_index = 0; t_index < c_maxHealth; t_index++)
        {
            GameObject t_heart;
            if (t_index % 2 == 0)
                t_heart = Instantiate(c_halfHeart, c_heartHolder) as GameObject;
            else
                t_heart = Instantiate(c_heart, c_heartHolder) as GameObject;

            t_heart.GetComponent<RectTransform>().anchorMin = t_anchorMin;
            t_heart.GetComponent<RectTransform>().anchorMax = t_anchorMax;

            t_heart.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            t_heart.GetComponent<RectTransform>().offsetMax = Vector2.zero;

            if (t_index % 2 != 0)
            {
                c_hearts[t_index - 1].SetActive(false);

                t_anchorMin.x += t_anchorsOffset;
                t_anchorMax.x += t_anchorsOffset;
                t_offsetVect.x += t_offset;

                t_heart.GetComponent<RectTransform>().anchoredPosition += t_offsetVect;
            }
                c_hearts[t_index] = t_heart;
        }
    }

    public void receiveDamage(int p_damage)
    {
        for (int t_index = 0; t_index < p_damage; t_index++)
        {
            c_hearts[c_currentHealth - 1].SetActive(false);
            --c_currentHealth;

            if (c_currentHealth <= 0)
                OnDie();
            else if (c_currentHealth % 2 != 0)
                c_hearts[c_currentHealth - 1].SetActive(true);
        }
    }

    private void OnDie()
    {
        CheckpointManager.ActiveCheckpoint.RestartGameFromCheckpoint(transform.root);
    }

}