using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using krakenScripts;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private string playerName = "Player";
    [SerializeField] private float survivalSeconds = 0f;
    [SerializeField] private string gameVersion = "1.0";
    [SerializeField] private string gameMode = "normal";

    private bool hasSubmitted = false;

    [System.Serializable]
    private class ScorePayload
    {
        public string player_name;
        public int score;
        public float survival_seconds;
        public string game_version;
        public string game_mode;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && PanelGameOver != null)
        {
            StartCoroutine(MonitorPanelActivation());
        }
    }

    private IEnumerator MonitorPanelActivation()
    {
        while (!PanelGameOver.activeInHierarchy && !hasSubmitted)
            yield return null;

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

        // OBTIENE EL SCORE DEL JUEGO
        int currentScore = KrakenControl.score;

        ScorePayload payload = new ScorePayload
        {
            player_name = playerName,
            score = currentScore,
            survival_seconds = survivalSeconds,
            game_version = gameVersion,
            game_mode = gameMode
        };

        string jsonBody = JsonUtility.ToJson(payload);
        Debug.Log($"JSON enviado: {jsonBody}"); 

        using (UnityWebRequest www = new UnityWebRequest("https://retroteca.org/wp-json/krakenbius/v1/submit-score", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Error al enviar puntuación: {www.error}\nResponse: {www.downloadHandler.text}");
            else
                Debug.Log($"Puntuación {currentScore} enviada: {www.downloadHandler.text}");
        }
    }
}
