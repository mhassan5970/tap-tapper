using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool IsGameStart;
    public bool IsGamePaused;
    public bool IsTimeOver;
    public bool IsAway = false;

    bool IsPlayerDataSaved = false;

    public bool IsReactionStarted = false;

    [Header("Asset Bundles")]
    public GameSetting gameSetting;

    PlayerData playerData;

    [Space]
    [Header("References")]
    public StateManager stateManager;
    [SerializeField] public PlayerScoreStats playerScoreStats;

    [Header("GamePlay - Stats")]
    public int taps = 0;
    public float gamePlayTimer;

    [Header("UI - Background Image")]
    public Image imgCanvasBg;

    [Header("UI - References")]
    public TextMeshProUGUI txtTapper;
    public TextMeshProUGUI txtTimer;
    public TextMeshProUGUI txtTaps;
    public TextMeshProUGUI txtHighScore;
    public TextMeshProUGUI txtTimeOverScore;

    public TextMeshProUGUI txtCounter;

    public int taps1 = 0;
    public int taps2 = 0;
    public TextMeshProUGUI txtTaps1;
    public TextMeshProUGUI txtTaps2;
    public TextMeshProUGUI txtTabNowMultiplayer;
    public bool IsTapOnePressed = false;
    public bool IsTapTwoPressed = false;


    [Header("UI - Panels")]
    public GameObject panelGamePlayStats;
    public GameObject panelTimeOver;
    public GameObject panelStartMultiplayer;

    [Header("UI - Buttons")]
    public Button btnStartReaction;
    public Button btnStartMultiplayer;
    public Button btnEndEnduranceGame;

    [Header("GamePlay - Panels")]
    public GameObject panelSingleTap;
    public GameObject panelAgilityTap;

    public GameObject panelMiddleMultiplayer;

    public GameObject bottomPanelSingle;
    public GameObject bottomPanelMultiplayer;

    [Space]
    public float awayDuration = 1f;
    public float awayDurationTimer;


    //Endurance Mode
    private int enduranceTapCount = 5;
    private int enduranceTapCountMax = 5;



    private void Start()
    {

        stateManager = FindObjectOfType<StateManager>();
        UpdateCanvasBackground();
        playerData = stateManager.GetPlayerData();

        awayDurationTimer = awayDuration;

        if(stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE)
        {
            gamePlayTimer = stateManager.selectedGameTimer;
           

            if (stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
            {
                gamePlayTimer = gameSetting.enduranceGameTime;

            }

            if(stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
            {
                txtTaps.gameObject.SetActive(false);
                btnStartReaction.onClick.AddListener(delegate { CallStartReactionMode(); });
            }

            txtHighScore.text = LoadPlayerGamePlayStats();
        }
        else
        {
            gamePlayTimer = stateManager.selectedGameTimer;
            txtTimer.gameObject.SetActive(false);
        }
        txtTimer.text = "Time\n" + ((int)gamePlayTimer).ToString();

        
        btnStartMultiplayer.onClick.AddListener(delegate { MultiplayerCountDown(); });

        ActivateTapPanel();
    }

    string LoadPlayerGamePlayStats()
    {
        string prepardText = "";
        
        playerScoreStats = stateManager.GetPlayerData().GetCurrentStats(stateManager.GetGamePlayMode(), stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE ? gameSetting.enduranceGameTime : stateManager.selectedGameTimer);

        if(playerScoreStats != null)
        {
            if(stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
            {
                prepardText =  playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
            else
            {
                prepardText = playerScoreStats.gamePlayTimer.ToString() + " Time\n" + playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
            return prepardText;
        }
        else
        {
            playerScoreStats = new();
            playerScoreStats.gamePlayTimer = stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE ? gameSetting.enduranceGameTime : stateManager.selectedGameTimer;
            playerScoreStats.gamePlayHighScore = 0;
            playerScoreStats.gamePlayMode = stateManager.GetGamePlayMode();
            stateManager.GetPlayerData().AddPlayerStats(playerScoreStats);
            if (stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
            {
                prepardText = playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
            else
            {
                prepardText = playerScoreStats.gamePlayTimer.ToString() + " Time\n" + playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
            return prepardText;
        }
    }


    void UpdateCanvasBackground()
    {
        foreach (GameModeBackGrounds gmbg in gameSetting.backGrounds) {
            if(gmbg.playMode == stateManager.GetGamePlayMode())
            {
                imgCanvasBg.sprite = gmbg.spriteBackGrounds[Random.Range(0, gmbg.spriteBackGrounds.Length)];
                AudioManager.Instance.PlayAudioTheme(gmbg.themeAudio);
                break;

            }
        }
    }


    private void Update()
    {
        CheckAwayTimer();
        CheckGamePlayTime();
        UpdateReactionTime();
    }

    void ActivateTapPanel()
    {
        if (stateManager.GetGamePlayMode() == GamePlayMode.AGILITY)
        {
            panelAgilityTap.SetActive(true);
            GameController.Instance.CreateAgility();

            if (stateManager.GetPlayerPlayType() == PlayerPlayType.MULTIPLAYER)
            {
                bottomPanelSingle.SetActive(false);
                bottomPanelMultiplayer.SetActive(stateManager.GetGamePlayMode() == GamePlayMode.REACTION ? false : true);
                panelStartMultiplayer.SetActive(true);
            }
            else
            {
                bottomPanelSingle.SetActive(true);
                bottomPanelMultiplayer.SetActive(false);
            }
        }
        else
        {
            if(stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE)
            {
                panelSingleTap.SetActive(true);

                bottomPanelSingle.SetActive(true);
                bottomPanelMultiplayer.SetActive(false);
            }
            else
            {
                panelMiddleMultiplayer.SetActive(true);
                bottomPanelSingle.SetActive(false);
                bottomPanelMultiplayer.SetActive(stateManager.GetGamePlayMode() == GamePlayMode.REACTION ? false : true);
                panelStartMultiplayer.SetActive(true);
            }
        }
    }

    public void UpdateTaps()
    {
        if (IsTimeOver || (!IsReactionStarted && stateManager.GetGamePlayMode() == GamePlayMode.REACTION)) return;

        if (!IsGameStart)
        {
            IsGameStart = true;
        }

        AudioManager.Instance.PlayAudioEffect("click");

        if(stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
        {
            if (!IsReactionStarted) return;

            IsReactionStarted = false;
            txtTimer.gameObject.SetActive(true);
            UpdateReactionScore();
            IsTimeOver = true;

            if (!IsPlayerDataSaved)
            {
                IsPlayerDataSaved = true;
                TimeOver();
            }

            
            return;
        }

        if(stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE)
        {
            if (IsAway)
            {
                IsAway = false;
                btnEndEnduranceGame.gameObject.SetActive(false);
            }
            txtTimer.gameObject.SetActive(true);
            awayDurationTimer = awayDuration;

            taps++;
            UpdateSinglePlayerTaps(taps);
            EnduranceGiveTime();
        }
    }

    void MultiplayerCountDown()
    {
        btnStartMultiplayer.gameObject.SetActive(false);
        txtCounter.gameObject.SetActive(true);

       

        StartCoroutine(StartMultiplayerGame());
    }
    IEnumerator StartMultiplayerGame()
    {
        for(int i = 3; i > 0; i--)
        {
            txtCounter.text = (i).ToString();
            yield return new WaitForSeconds(1);
        }
        panelStartMultiplayer.gameObject.SetActive(false);

        if (stateManager.GetGamePlayMode() != GamePlayMode.REACTION) { 
            IsGameStart = true;
            txtTimer.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(StartReactionMode());
        }
    }

    void CallStartReactionMode()
    {
        btnStartReaction.onClick.RemoveAllListeners();
        StartCoroutine(StartReactionMode());
    }

    IEnumerator StartReactionMode()
    {
        txtTapper.text = "";
        yield return new WaitForSeconds(Random.Range(20f, 25f));
        txtTapper.text = "Tap";
        if(stateManager.GetPlayerPlayType() == PlayerPlayType.MULTIPLAYER)
        {
            txtTabNowMultiplayer.gameObject.SetActive(true);
            txtTabNowMultiplayer.text = "Tap";
        }
        IsReactionStarted = true;
    }

    void UpdateReactionTime()
    {
        if (IsReactionStarted)
        {
            gamePlayTimer += Time.deltaTime;
            txtTimer.text = "Time\n" + gamePlayTimer.ToString();
        }
        else
        {
            if(stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
            {
                txtHighScore.text = playerScoreStats.gamePlayHighScore.ToString() + " Time";
            }
        }
    }

    public void UpdateTap1()
    {
        if (IsTimeOver) return;

        if (stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
        {
            if (!IsReactionStarted) return;

            IsReactionStarted = false;
            txtTimer.gameObject.SetActive(true);
            IsTimeOver = true;
            IsTapOnePressed = true;

            if (!IsPlayerDataSaved)
            {
                IsPlayerDataSaved = true;
                TimeOver();
            }
            return;
        }
        if (!IsGameStart)
        {
            IsGameStart = true;
        }

        AudioManager.Instance.PlayAudioEffect("click");


        if (stateManager.GetPlayerPlayType() == PlayerPlayType.MULTIPLAYER)
        {
            if (IsAway)
            {
                IsAway = false;
            }

            awayDurationTimer = awayDuration;

            taps1++;

            txtTaps1.text = "Taps\n" + taps1.ToString();
        }
    }

    public void UpdateTap2()
    {
        if (IsTimeOver) return;

        if (stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
        {
            if (!IsReactionStarted) return;

            IsReactionStarted = false;
            txtTimer.gameObject.SetActive(true);
            IsTimeOver = true;
            IsTapTwoPressed = true;

            if (!IsPlayerDataSaved)
            {
                IsPlayerDataSaved = true;
                TimeOver();
            }
            return;
        }

        if (!IsGameStart)
        {
            IsGameStart = true;
        }

        AudioManager.Instance.PlayAudioEffect("click");


        if (stateManager.GetPlayerPlayType() == PlayerPlayType.MULTIPLAYER)
        {
            if (IsAway)
            {
                IsAway = false;
            }

            awayDurationTimer = awayDuration;

            taps2++;

            txtTaps2.text = "Taps\n" + taps2.ToString();
        }
    }

    void EnduranceGiveTime()
    {
        if(stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
        {
            if(enduranceTapCount <= 0)
            {
                enduranceTapCount = enduranceTapCountMax;
                gamePlayTimer += 1;
            }
            else
            {
                enduranceTapCount--;
            }
        }
    }

    void UpdateSinglePlayerTaps(int value)
    {
        txtTaps.text = "Taps: " + value.ToString();
        txtTapper.text = value.ToString();
        UpdateHighScore(value);
        Debug.Log("Tap React pressed");
    }

    void UpdateHighScore(int currentTaps)
    {
        if(currentTaps > playerScoreStats.gamePlayHighScore)
        {
            playerScoreStats.gamePlayHighScore = currentTaps;
            if (stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
            {
                txtHighScore.text = playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
            else
            {
                txtHighScore.text = playerScoreStats.gamePlayTimer.ToString() + " Time\n" + playerScoreStats.gamePlayHighScore.ToString() + " HighScore";
            }
        }
       
    }

    void UpdateReactionScore()
    {
        if(gamePlayTimer < playerScoreStats.gamePlayHighScore)
        {
            playerScoreStats.gamePlayHighScore = gamePlayTimer;
        }
    }

    public void EndEnduranceGame()
    {
        gamePlayTimer = 0f;
    }

    void CheckAwayTimer()
    {
        if (IsGamePaused || !IsGameStart || IsTimeOver) return;

        if(awayDurationTimer > 0f && IsGameStart)
        {
            awayDurationTimer -= Time.deltaTime;
        }
        else
        {
            IsAway = true;
            if(stateManager.GetGamePlayMode() == GamePlayMode.ENDURANCE)
            {
                btnEndEnduranceGame.gameObject.SetActive(true);
            }
            //txtTapper.text = "Hurry Up! Tap Me!";
        }
    }

    void CheckGamePlayTime()
    {
        if (IsGamePaused || !IsGameStart || IsTimeOver && stateManager.GetGamePlayMode() == GamePlayMode.REACTION) return;

        if(gamePlayTimer > 0f)
        {
            gamePlayTimer -= Time.deltaTime;

            txtTimer.text = "Time\n"+((int)gamePlayTimer).ToString();
        }
        else
        {
            IsTimeOver = true;

            if (!IsPlayerDataSaved)
            {
                IsPlayerDataSaved = true;
                TimeOver();
            }
        }
    }


    void TimeOver()
    {
        if (stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE) { 
            txtTimeOverScore.text = "Score: "+taps.ToString();

            if(stateManager.GetGamePlayMode() == GamePlayMode.REACTION)
            {
                txtTimeOverScore.text = txtTimer.text;
            }
        }
        else
        {
            if(stateManager.GetGamePlayMode() != GamePlayMode.REACTION)
            {
                txtTimeOverScore.text = "Red Player\n" + taps1.ToString() + "\nBlue Player\n"+taps2.ToString();
            }
            else
            {
                if (IsTapOnePressed)
                {
                    txtTimeOverScore.text = "Red Player\n" + txtTimer.text + "\n Blue Player\n" + taps2.ToString();
                    
                }
                if (IsTapTwoPressed)
                {
                    txtTimeOverScore.text = "Red Player\n" + taps1.ToString() + "\n Blue Player\n" + txtTimer.text;

                }

            }
        }

        panelGamePlayStats.SetActive(false);
        panelTimeOver.SetActive(true);
        
        SaveSystem.Save(playerData);
    }
}