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

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        Debug.Log("Go home");
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}