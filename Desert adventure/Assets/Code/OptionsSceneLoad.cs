using UnityEngine;
using UnityEngine.UI;

public class OptionsSceneLoad : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Toggle>().isOn = GameObject.Find("SoundManager").GetComponent<SoundManager>().SoundActive;
    }
}