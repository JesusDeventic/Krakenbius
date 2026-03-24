using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour
{

    // References Panel
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject manPanel;
    public GameObject rankingPanel;
    public GameObject titlePanel;

    public GameObject exitButton;
    public GameObject confirmExitPanel;

    [Header("AudioManager")]
    public AudioManager audioManager;

#if !UNITY_ANDROID
    void Start()
    {
    }
#endif

    // Reproduce SFX y muestra panel principal, oculta otros
    public void HomeButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        mainPanel.SetActive(true);
        titlePanel.SetActive(true);
        settingsPanel.SetActive(false);
        manPanel.SetActive(false);
    }

    // Reproduce SFX, cierra ranking si está abierto y muestra panel de ajustes
    public void SettingsButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        if (rankingPanel.activeInHierarchy)
            rankingPanel.SetActive(false);

        mainPanel.SetActive(false);
        manPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // Reproduce SFX, cierra ranking si está abierto y muestra panel manual
    public void ManButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();

        if (rankingPanel.activeInHierarchy)
            rankingPanel.SetActive(false);

        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        manPanel.SetActive(true);
    }

    // Reproduce SFX y carga escena del juego
    public void PlayButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        SceneManager.LoadScene("GameScene");
    }

    // Reproduce SFX y alterna visibilidad del panel de ranking y título
    public void RankingButton()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        rankingPanel.SetActive(!rankingPanel.activeInHierarchy);
        titlePanel.SetActive(!titlePanel.activeInHierarchy);
    }

    // Muestra panel de confirmación de salida
    public void ExitButton()
    {
        confirmExitPanel.SetActive(true);
    }

    // Cierra la aplicación completamente
    public void ConfirmExit()
    {
        Application.Quit();
    }

    // Oculta panel de confirmación de salida
    public void CancelExit()
    {
        confirmExitPanel.SetActive(false);
    }
}