using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
public class Services : MonoBehaviour
{
    void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        Debug.Log("Starting Play Service");
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log("Checking sign in status");
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            Debug.Log("name "+name);
            Debug.Log("id "+id);
            Debug.Log("ImgUrl "+ImgUrl);
        }
        else
        {
            Debug.Log("Signed failed");

            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }


    public void ShowLeaderBoard()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated()) {
            //Social.ShowLeaderboardUI();
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Not Authenticated");
            SignIn();
        }
    }

}
