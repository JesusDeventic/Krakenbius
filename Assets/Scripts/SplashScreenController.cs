using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    // Carga escena principal del menú desde pantalla de splash/loading
    public void loadMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}