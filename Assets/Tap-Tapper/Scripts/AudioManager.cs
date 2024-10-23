using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("References")]
    public StateManager stateManager;

    private AudioSource audiosrc_SoundEffect;
    public AudioSource audiosrc_BackgroundTheme;

    [Header("On/Off - Sprites")]
    public Sprite spriteOn;
    public Sprite spriteOff;

    [Header("Themes")]
    public CustomAudio themeAudio1;

    [Space]
    [Header("Sound Effects")]
    public CustomAudio uiClick;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audiosrc_SoundEffect = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(string soundName)
    {
        PlayAudioEffect(soundName);
    }
    public void PlayAudioEffect(string audioEffect)
    {
        PlayerSoundSetting playerSoundSetting = stateManager.GetPlayerData().GetPlayerSoundSetting();

        if (playerSoundSetting.audioEffectActive)
        {
            CustomAudio selectedAudio = GetAudioEffect(audioEffect);
            if (selectedAudio != null)
            {
                audiosrc_SoundEffect.PlayOneShot(selectedAudio.GetAudioClip(), selectedAudio.GetAudioVolume());
            }
            else
            {
                Debug.Log("No Sound found");
            }
        }
    }

    public void PlayAudioEffect(AudioClip audioEffect)
    {
        PlayerSoundSetting playerSoundSetting = stateManager.GetPlayerData().GetPlayerSoundSetting();

        if (playerSoundSetting.audioEffectActive)
        {
            audiosrc_SoundEffect.PlayOneShot(audioEffect);
        }
    }
    CustomAudio GetAudioEffect(string audioEffect)
    {
        CustomAudio selectedAudio = audioEffect switch
        {
            "click" => uiClick,
            _ => null,
        };

        return selectedAudio;
    }

    public void StopAudioTheme()
    {
        if(audiosrc_BackgroundTheme != null)
        {
            audiosrc_BackgroundTheme.Stop();
        }
    }
    public void PlayAudioTheme(string audioTheme)
    {
        PlayerSoundSetting playerSoundSetting = stateManager.GetPlayerData().GetPlayerSoundSetting();

        if (playerSoundSetting.audioThemeActive)
        {
            CustomAudio selectedTheme = GetAudioTheme(audioTheme);
            if (selectedTheme != null)
            {
                audiosrc_BackgroundTheme.clip = selectedTheme.GetAudioClip();
                audiosrc_BackgroundTheme.volume = selectedTheme.GetAudioVolume();
            }
            else
            {
                Debug.Log("No theme sound found");
            }

            audiosrc_BackgroundTheme.Play();
        }
        else
        {
            audiosrc_BackgroundTheme.Stop();
        }
    }
    public void PlayAudioTheme(CustomAudio audioTheme)
    {
        PlayerSoundSetting playerSoundSetting = stateManager.GetPlayerData().GetPlayerSoundSetting();

        if (playerSoundSetting.audioThemeActive)
        {
            CustomAudio selectedTheme = audioTheme;
            if (selectedTheme != null)
            {
                audiosrc_BackgroundTheme.clip = selectedTheme.GetAudioClip();
                audiosrc_BackgroundTheme.volume = selectedTheme.GetAudioVolume();
            }
            else
            {
                Debug.Log("No theme sound found");
            }

            audiosrc_BackgroundTheme.Play();
        }
        else
        {
            audiosrc_BackgroundTheme.Stop();
        }
    }
    CustomAudio GetAudioTheme(string audioTheme)
    {
        CustomAudio selectedAudio = audioTheme switch
        {
            "theme1" => themeAudio1,
            _ => null,
        };

        return selectedAudio;
    }
}
