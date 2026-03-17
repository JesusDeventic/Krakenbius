using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour {

	// References Panel
    [Header("UI Panels")]
	public GameObject mainPanel;
	public GameObject settingsPanel;
    public GameObject manPanel;
    public GameObject rankingPanel;
    public GameObject titlePanel;

    public GameObject exitButton;
    public GameObject twitterButton;
    public GameObject confirmExitPanel;

    [Header("AudioManager")]
    public AudioManager audioManager;

#if !UNITY_ANDROID
    void Start()
    {
        exitButton.SetActive(false);
        twitterButton.SetActive(false);
    }
#endif
    //*********************************************** BUTTON RETURN MAIN MENU *****************************************************
    public void HomeButton() 
	{
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        mainPanel.SetActive(true);
        titlePanel.SetActive(true);
		settingsPanel.SetActive(false);
        manPanel.SetActive(false);
    }

	//*********************************************** BUTTONS MAIN MENU *****************************************************
	public void SettingsButton ()
	{
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        manPanel.SetActive(false);
    }
    
public void ManButton()
{
    ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
    
    // If ranking was opened before, close it first
    if (rankingPanel.activeInHierarchy)
    {
        rankingPanel.SetActive(false);
    }
    
    // Now show man panel and hide others
    mainPanel.SetActive(false);
    settingsPanel.SetActive(false);
    manPanel.SetActive(true);
}

    public void PlayButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        //Destroy(GameObject.Find("MainPanel(Clone)"));
        SceneManager.LoadScene("GameScene");
    }

    public void RankingButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        rankingPanel.SetActive(!rankingPanel.activeInHierarchy);
        titlePanel.SetActive(!titlePanel.activeInHierarchy);
    }

    public void ExitButton()
    {
        confirmExitPanel.SetActive(true);
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        confirmExitPanel.SetActive(false);
    }

    public void GoToTwitter()
    {
        Application.OpenURL("");
    }
}
