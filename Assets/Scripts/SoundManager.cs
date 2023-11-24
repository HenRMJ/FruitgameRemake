using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]private FMODUnity.StudioEventEmitter BounceSound, FusionSound;


    public void PlayBounceSound()
    {
        BounceSound.Play();
    }

    public void PlayFusionSound()
    {
        FusionSound.Play();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
