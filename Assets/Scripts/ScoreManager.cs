using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private string playerName = "Jesus";
    [SerializeField] private int score = 0;
    [SerializeField] private string gameVersion = "1.0";
    [SerializeField] private string gameMode = "normal";
    
    private bool hasSubmitted = false;
    
    private void Start()
    {
        // Verifica que estamos en GameScene y que el panel existe
        if (SceneManager.GetActiveScene().name == "GameScene" && panelGameOver != null)
        {
            // Monitorea cuando se active el PanelGameOver
            StartCoroutine(MonitorPanelActivation());
        }
    }
    
    private IEnumerator MonitorPanelActivation()
    {
        // Espera hasta que el PanelGameOver se active
        while (!panelGameOver.activeInHierarchy && !hasSubmitted)
        {
            yield return null;
        }
        
        if (!hasSubmitted)
        {
            Debug.Log("PanelGameOver activado. Enviando puntuación...");
            SubmitScore();
        }
    }
    
    private void SubmitScore()
    {
        StartCoroutine(SubmitScoreCoroutine());
    }
    
    private IEnumerator SubmitScoreCoroutine()
    {
        hasSubmitted = true;
        
        var scoreData = new
        {
            player_name = playerName,
            score = score,
            game_version = gameVersion,
            game_mode = gameMode
        };
        
        string jsonBody = JsonUtility.ToJson(scoreData);
        
        using (UnityWebRequest www = new UnityWebRequest("https://retroteca.org/wp-json/krakenbius/v1/submit-score", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");
            
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al enviar puntuación: {www.error}\nResponse: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"Puntuación enviada exitosamente: {www.downloadHandler.text}");
            }
        }
    }
}
