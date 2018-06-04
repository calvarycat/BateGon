using UnityEngine;
[ExecuteInEditMode]
public class GameConstants : MonoBehaviour
{
    public const string gameVersion = "1.0.0";
    static GameConstants _instance;
    public static GameConstants Instance { get { return _instance; } }

    public AudioClip clickSound;
    public AudioClip deadSound;
    public AudioClip collectSound;
    public AudioClip jumpSound;
    void Awake()
    {
        _instance = this;
    }
    public void Init()
    {
    }
    public static void PlaySoundClick()
    {
        AudioManager.PlaySound(Instance.clickSound);
    }
    public static void PlaySoundDead()
    {
        AudioManager.PlaySound(Instance.deadSound);
    }
    public static void PlaySoundCollect()
    {
        AudioManager.PlaySound(Instance.collectSound);
    }
    public static void PlaySoundJump()
    {
        AudioManager.PlaySound(Instance.jumpSound);
    }
}
