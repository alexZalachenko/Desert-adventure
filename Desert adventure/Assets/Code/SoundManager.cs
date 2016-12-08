using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private AudioSource c_backgroundMusic;
    [SerializeField]
    private AudioSource c_effectsSounds;

    [SerializeField]
    private AudioClip c_knifeClip;
    [SerializeField]
    private AudioClip c_hitClip;
    [SerializeField]
    private AudioClip c_slideClip;
    [SerializeField]
    private AudioClip c_jumpClip;
    [SerializeField]
    private AudioClip c_shootClip;
    public enum EffectsCips
    {
        Knifing,
        Hitted,
        Sliding,
        Jumping,
        Shooting
    }

    private static SoundManager c_singletonInstance = null;
    public static SoundManager Instance
    {
        get
        {
            return c_singletonInstance;
        }
    }

    private bool c_soundActive = true;
    public bool SoundActive
    {
        set
        {
            c_soundActive = value;
            c_backgroundMusic.mute = !value;
            c_effectsSounds.mute = !value;
        }
        get
        {
            return c_soundActive;
        }
    }

    void Awake()
    {
        if (c_singletonInstance != null && c_singletonInstance != this)
            Destroy(gameObject);
        else
        {
            c_singletonInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(EffectsCips p_clip)
    {
        switch (p_clip)
        {
            case EffectsCips.Knifing:
                c_effectsSounds.clip = c_knifeClip;
                break;
            case EffectsCips.Hitted:
                c_effectsSounds.clip = c_hitClip;
                break;
            case EffectsCips.Sliding:
                c_effectsSounds.clip = c_slideClip;
                break;
            case EffectsCips.Jumping:
                c_effectsSounds.clip = c_jumpClip;
                break;
            case EffectsCips.Shooting:
                c_effectsSounds.clip = c_shootClip;
                break;
            default:
                break;
        }
        c_effectsSounds.Play();
    }

    
}