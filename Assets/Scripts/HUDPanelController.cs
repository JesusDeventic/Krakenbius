using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HUDPanelController : MonoBehaviour
{
    bool isPaused = false;
    GameObject pausePanel;
    GameObject gameOverPanel;
    GameObject captionPanel;
    GameObject pauseButton;

    void Start()
    {
        pausePanel = GameObject.Find("PanelPause");
        pausePanel.SetActive(false);
        gameOverPanel = GameObject.Find("PanelGameOver");
        gameOverPanel.SetActive(false);
        captionPanel = GameObject.Find("Caption_Panel");
        captionPanel.SetActive(false);
        pauseButton = GameObject.Find("Pause_Button");
    }

    void Update()
    {
    }

    // Alterna pausa/reanudar del juego: detiene tiempo, muestra/oculta paneles, pausa/reanuda música
    public void Pause()
    {
        if (gameOverPanel.activeInHierarchy)
            return;

        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pausePanel.SetActive(false);
            pauseButton.SetActive(true);
            captionPanel.SetActive(false);
            Object.FindFirstObjectByType<AudioManager>().resumeMusic();
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            pausePanel.SetActive(true);
            pauseButton.SetActive(false);
            captionPanel.SetActive(true);
            Object.FindFirstObjectByType<AudioManager>().pauseMusic();
        }
    }

    // Reinicia tiempo y carga escena principal (menú)
    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        Debug.Log("Go home");
    }

    // Activa panel de Game Over
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Recarga escena actual del juego
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}