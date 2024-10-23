using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GamePlayMode
{
    SPEED,
    ENDURANCE,
    AGILITY,
    REACTION
}
public enum PlayerPlayType
{
    SINGLE,
    MULTIPLAYER
}


public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    [Header("References")]
    public Services services;

    [Header("Game Stats")]
    public GamePlayMode gamePlayMode = GamePlayMode.SPEED;
    public PlayerPlayType playerPlayType = PlayerPlayType.SINGLE;

    [Header("Selected Time")]
    public int selectedGameTimer = 0;

    
    [SerializeField] PlayerData playerData = new PlayerData();

    public delegate void OnDataLoaded();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance.gameObject);
        }
    }


    #region playtype and modes
    public void SetGameMode(string gamePlayMode)
    {
        this.gamePlayMode = GetGamePlayMode(gamePlayMode);

        if(this.gamePlayMode  == GamePlayMode.ENDURANCE)
        {
            this.playerPlayType = PlayerPlayType.SINGLE;
            LoadScene();
        }
    }

    public void SetPlayerPlayMode(string playerPlayType) { 
        this.playerPlayType = GetPlayerPlayType(playerPlayType);
    }

    public PlayerPlayType GetPlayerPlayType()
    {
        return this.playerPlayType;
    }

    public void ResetPlayerPlayMode()
    {
        this.playerPlayType = PlayerPlayType.SINGLE;
        this.selectedGameTimer = 0;
    }

    public void SelectTimer(int value)
    {
        this.selectedGameTimer = value;
        LoadScene();
    }

    PlayerPlayType GetPlayerPlayType(string playerType)
    {
        PlayerPlayType selectedPlayerPlayType = playerType switch
        {
            "single" => PlayerPlayType.SINGLE,
            "multiplayer" => PlayerPlayType.MULTIPLAYER,
            _ => PlayerPlayType.SINGLE,
        };

        return selectedPlayerPlayType;
    }
    GamePlayMode GetGamePlayMode(string gamePlayMode)
    {
        GamePlayMode selectedgamePlayMode = gamePlayMode switch
        {
            "speed" => GamePlayMode.SPEED,
            "endurance" => GamePlayMode.ENDURANCE,
            "agility" => GamePlayMode.AGILITY,
            "reaction" => GamePlayMode.REACTION,
            _ => GamePlayMode.SPEED,
        };

        return selectedgamePlayMode;
    }
    public GamePlayMode GetGamePlayMode()
    {
        return this.gamePlayMode;
    }

    #endregion

    void LoadScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadLeaderBoard()
    {
        services.ShowLeaderBoard();
    }

    #region player data
    public void LoadPlayerData(OnDataLoaded onCompleted)
    {
        string saveString = SaveSystem.Load();
        Debug.Log(saveString);
        if (!string.IsNullOrEmpty(saveString))
        {
            playerData = JsonUtility.FromJson<PlayerData>(saveString);
        }
        else
        {
            playerData = new PlayerData();
            SaveSystem.Init(playerData);
        }

        onCompleted.Invoke();
    }
    public void SavePlayerData()
    {
        SaveSystem.Save(playerData);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    #endregion
    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
    private void OnApplicationPause(bool pause)
    {
        SavePlayerData();
    }
}
