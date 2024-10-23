using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public StateManager stateManager;

    [Header("Panels")]
    public GameObject panelMenu;
    public GameObject panelPlayerOptions;
    public GameObject panelGameOptions;


    [Header("UI - References")]
    public Image imgSound;
    public Image imgMusic;

    private void Start()
    {
        stateManager = FindObjectOfType<StateManager>();
        UpdateSoundSprites();
    }

    public void SelectGameMode(string gamePlayMode)
    {
        AudioManager.Instance.PlaySoundEffect("click");
        stateManager.SetGameMode(gamePlayMode.ToLower());
    }

    public void SelectPlayerPlayMode(string playerPlayType) {
        AudioManager.Instance.PlaySoundEffect("click");
        stateManager.SetPlayerPlayMode(playerPlayType.ToLower());

        if(stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
        {
            SelectTime(0);
        }
    }

    public void ResetPlayerPlayMode()
    {
        AudioManager.Instance.PlaySoundEffect("click");
        stateManager.ResetPlayerPlayMode();
    }

    public void SelectTime(int value)
    {
        stateManager.SelectTimer(value);
    }

    void UpdateSoundSprites()
    {
        imgSound.sprite = stateManager.GetPlayerData().GetPlayerSoundSetting().audioEffectActive ? AudioManager.Instance.spriteOn : AudioManager.Instance.spriteOff;
        imgMusic.sprite = stateManager.GetPlayerData().GetPlayerSoundSetting().audioThemeActive ? AudioManager.Instance.spriteOn : AudioManager.Instance.spriteOff;
    }

    public void SoundButton()
    {
        stateManager.GetPlayerData().Sound();
        if (stateManager.GetPlayerData().GetPlayerSoundSetting().audioEffectActive)
        {
            AudioManager.Instance.PlayAudioEffect("click");
        }
        UpdateSoundSprites();
    }

    public void MusicButton()
    {
        stateManager.GetPlayerData().Music();
        if (stateManager.GetPlayerData().GetPlayerSoundSetting().audioThemeActive)
        {
            AudioManager.Instance.PlayAudioTheme("theme1");
        }
        else
        {
            AudioManager.Instance.StopAudioTheme();
        }

        UpdateSoundSprites();
    }

    public void ShowLeaderBoard()
    {
        stateManager.LoadLeaderBoard();
    }

}
