using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    [Header("References")]
    public StateManager stateManager;
    public AudioManager audioManager;
    private void Start()
    {
        //Invoke(nameof(LoadMainMenu), Random.RandomRange(0.5f, 1.5f));
        stateManager.LoadPlayerData(LoadMainMenu);
    }


    public void LoadMainMenu()
    {
        audioManager.PlayAudioTheme("theme1");
        Invoke(nameof(InvokeScene), 0.5f);
    }

    void InvokeScene()
    {
        SceneManager.LoadScene(2);
    }
}