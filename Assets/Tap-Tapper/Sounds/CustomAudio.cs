using UnityEngine;
[CreateAssetMenu(fileName = "CustomAudio", menuName = "CustomAudio/Create")]
public class CustomAudio : ScriptableObject
{
    public SoundType SoundType = SoundType.SOUND_EFFECT;
    [SerializeField] AudioClip AudioClip;
    [Range(0f, 1f)]
    [SerializeField] float AudioVolume;

    public AudioClip GetAudioClip()
    {
        return AudioClip;
    }
    public float GetAudioVolume()
    {
        return AudioVolume;
    }
}
public enum SoundType
{
    SOUND_EFFECT,
    SOUND_THEME
}
