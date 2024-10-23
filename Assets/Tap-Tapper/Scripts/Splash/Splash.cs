using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

public class Splash : MonoBehaviour
{
    [Header("Asset Bundle")]
    public GameSetting gameSetting;

    public TextMeshProUGUI txtSplash;
    public int sceneIndex;
    void OnEnable()
    {
        StartSplash();
    }

    public async void StartSplash()
    {
        await ShowAnimatedSplash();
        Invoke(nameof(LoadScene), Random.Range(0.5f, 1f));
    }

    async Task ShowAnimatedSplash()
    {
        txtSplash.transform.localScale = Vector3.zero;
        await txtSplash.transform.DOScale(1f, gameSetting.splashDuration).SetEase(Ease.InBounce).AsyncWaitForCompletion();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
