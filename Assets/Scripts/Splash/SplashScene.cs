using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    public GameObject privacyPanel;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.Instance.mainMusic.mute = true;
        Constants.splashToHome = 1;
        Constants.isLevelCompleted = 2;
        if (PreferenceManager.GetPrivacyPolicyStatus() == 0)
            privacyPanel.SetActive(true);
        else
        {
            privacyPanel.SetActive(false);
            Invoke("LoadMainMenuScene", 5f);
        }
    }

    void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void AgreeBtnPressed()
    {
        //AdmobGA_Helper.LogGAEvent("Splash:PrivacyPolicy:Agreed");
        PreferenceManager.SetPrivacyPolicyStatus();
        privacyPanel.SetActive(false);
        Invoke("LoadMainMenuScene", 2.5f);
    }

    public void PrivacyPolicyLink()
    {
        //AdmobGA_Helper.LogGAEvent("Splash:PrivacyPolicy:BtnClick");
        Application.OpenURL("http://thegamenotch.com/privacy-policy/");
    }
}
