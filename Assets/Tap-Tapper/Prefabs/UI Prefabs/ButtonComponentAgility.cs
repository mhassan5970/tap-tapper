using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonComponentAgility : MonoBehaviour
{
    public Sprite spriteDefault;
    
    public bool IsReadyToTap = false;
    public string playerName = "";

    public Image imgBtn;
    private GameManager gameManager;

    public void SetProps(Sprite btnPlayerSprite, bool IsReadyToTap, string playerName, GameManager gameManager)
    {
        imgBtn.sprite = btnPlayerSprite;
        this.IsReadyToTap = IsReadyToTap;
        this.playerName = playerName;
        this.gameManager = gameManager;
    }

    public void PressAgilityTap()
    {
        if (IsReadyToTap && gameManager.stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE)
        {
            IsReadyToTap = false;
            imgBtn.sprite = spriteDefault;
            gameManager.UpdateTaps();
            GameController.Instance.EnableRandomAgilityTap();
            if (!gameManager.IsGameStart)
            {
                gameManager.IsGameStart = true;
            }
        }
    }

    public void PressAgilityTapMultiplayer()
    {
        if (IsReadyToTap && gameManager.stateManager.GetPlayerPlayType() == PlayerPlayType.MULTIPLAYER)
        {
            IsReadyToTap = false;
            imgBtn.sprite = spriteDefault;
            if(playerName == "p1")
            {
                gameManager.UpdateTap1();
                if (!gameManager.IsGameStart)
                {
                    gameManager.IsGameStart = true;
                }
            }
            else
            {
                gameManager.UpdateTap2();
                if (!gameManager.IsGameStart)
                {
                    gameManager.IsGameStart = true;
                }
            }

            GameController.Instance.EnableRandomAgilityMultiplayer(playerName);
        }
    }
}
