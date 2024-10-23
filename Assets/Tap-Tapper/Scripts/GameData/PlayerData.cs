using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [Header("Player Profile")]
    public float saveVersion = 1.0f;
    public string playerName = "Username";

    [Header("Player Stats")]
    [SerializeField] public PlayerStats playerStats = new();

    [Space]
    [Header("Sound Setting")]
    [SerializeField] PlayerSoundSetting PlayerSoundSetting = new();


    public void AddPlayerStats(PlayerScoreStats stats)
    {
        playerStats.stats.Add(stats);
    }
    public PlayerScoreStats GetCurrentStats(GamePlayMode gamePlayMode, int currentSelectedTimer)
    {
        PlayerScoreStats stats = null;

        stats = playerStats.stats.Find(x => x.gamePlayMode == gamePlayMode && x.gamePlayTimer == currentSelectedTimer);
        if (stats != null)
        {
            return stats;
        }
        else
        {
            return null;
        }
    }

    PlayerScoreStats FindExistStats(PlayerScoreStats stats)
    {
        PlayerScoreStats playerScoreStats = null;
        playerScoreStats = playerStats.stats.Find(x => x == stats);

        return playerScoreStats;
    }


    public void Music()
    {
        PlayerSoundSetting.audioThemeActive = !PlayerSoundSetting.audioThemeActive;
    }
    public void Sound()
    {
        PlayerSoundSetting.audioEffectActive = !PlayerSoundSetting.audioEffectActive;
    }

    public PlayerSoundSetting GetPlayerSoundSetting()
    {
        return PlayerSoundSetting;
    }
}

[Serializable]
public class PlayerStats
{
    public List<PlayerScoreStats> stats = new();
}

[Serializable]
public class PlayerScoreStats
{
    public GamePlayMode gamePlayMode;
    public float gamePlayTimer;
    public float gamePlayHighScore;
}

[Serializable]
public class PlayerSoundSetting
{
    public bool audioThemeActive = true;
    public bool audioEffectActive = true;
    public float volume = 1f;
}