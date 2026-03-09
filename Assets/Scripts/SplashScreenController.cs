using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public void loadMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
