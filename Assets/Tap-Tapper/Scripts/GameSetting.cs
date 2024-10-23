using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSetting", menuName = "GameSetting/Create")]
public class GameSetting : ScriptableObject
{
    [Tooltip("Splash Screen Animation Duration")]
    [Header("Flow Animation Duration")]
    [Range(0f, 1f)]
    public float splashDuration;

    [Tooltip("Number of timer modes")]
    [Header("Gameplay Timers")]
    public int[] gameTimers;

    [Space]
    public int enduranceGameTime = 5;

    [Header("UI - Prefabs")]
    public GameObject btnAgilityTap;

    [Header("Gamemodes - Backgrounds")]
    [SerializeField] public List<GameModeBackGrounds> backGrounds;

    [Header("Agility Taps Sprite")]
    public Sprite spritePlayer1;
    public Sprite spritePlayer2;
}

[Serializable]
public class GameModeBackGrounds
{
    public GamePlayMode playMode;
    public CustomAudio themeAudio;
    public Sprite[] spriteBackGrounds;
}
