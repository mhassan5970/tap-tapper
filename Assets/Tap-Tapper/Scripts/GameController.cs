using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("References")]
    public GameManager gameManager;

    [Header("References - Parents")]
    public GameObject parentScrollView_Agility;

    public List<ButtonComponentAgility> agilityBtns;
    private ButtonComponentAgility currentActiveButton;
    private int previousIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance);
            Instance = this;
        }
    }

    public void MainMenu()
    {
        AudioManager.Instance.PlayAudioTheme("theme1");
        SceneManager.LoadScene(2);
    }

    public void Restart()
    {
        SceneManager.LoadScene(3);
    }

    public void CreateAgility()
    {
        GameObject agilityButton;

        for (int i = 0; i < 9; i++)
        {
            agilityButton = Instantiate(gameManager.gameSetting.btnAgilityTap);
            agilityButton.transform.SetParent(parentScrollView_Agility.transform);
            agilityButton.transform.localScale = new Vector3(1f, 1f, 1f);
            agilityBtns.Add(agilityButton.GetComponent<ButtonComponentAgility>());
        }

        if(gameManager.stateManager.GetPlayerPlayType() == PlayerPlayType.SINGLE)
        {
            Invoke(nameof(OnSetAgility), 0.5f);
        }
        else
        {
            StartCoroutine(StartAgilityMultiplayer());
        }
    }

    void OnSetAgility()
    {
        StartAgilityGame();
    }

    IEnumerator StartAgilityMultiplayer()
    {
        ButtonComponentAgility BCA = null;

        bool isUniqueButton = false;

        while (!isUniqueButton)
        {
            BCA = agilityBtns[Random.Range(0, agilityBtns.Count)];

            if(BCA.IsReadyToTap == false)
            {
                isUniqueButton = true;
                BCA.SetProps(gameManager.gameSetting.spritePlayer1, true, "p1", gameManager);
            }
        }

        isUniqueButton = false;
        BCA = null;
        while (!isUniqueButton)
        {
            BCA = agilityBtns[Random.Range(0, agilityBtns.Count)];

            if (BCA.IsReadyToTap == false)
            {
                isUniqueButton = true;
                BCA.SetProps(gameManager.gameSetting.spritePlayer2, true, "p2", gameManager);
            }
        }
        yield return null;
    }

    public void EnableRandomAgilityMultiplayer(string name)
    {
        ButtonComponentAgility BCA = null;

        bool isUniqueButton = false;

        while (!isUniqueButton)
        {
            BCA = agilityBtns[Random.Range(0, agilityBtns.Count)];

            if (BCA.IsReadyToTap == false)
            {
                isUniqueButton = true;
                BCA.SetProps(name == "p1" ? gameManager.gameSetting.spritePlayer1 : gameManager.gameSetting.spritePlayer2, true, name, gameManager);
            }
        }
    }


    void StartAgilityGame()
    {
        EnableRandomAgilityTap();
    }

    public void EnableRandomAgilityTap()
    {
       
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, agilityBtns.Count);
        }
        while (randomIndex == previousIndex);

        previousIndex = randomIndex; 
        currentActiveButton = agilityBtns[randomIndex];

        currentActiveButton.SetProps(gameManager.gameSetting.spritePlayer1, true, "p1", gameManager);
    }

    public void TopTappers()
    {
        Debug.Log("Show TopTappers board");
    }
}
