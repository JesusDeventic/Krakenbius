using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using krakenScripts;

public class ScoreManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private GameObject ContenedorDialogos;

    [Header("Input Nombre en ContenedorDialogos")]
    [SerializeField] private InputField inputNombre;
    [SerializeField] private Button btnConfirmar; 

    [Header("Datos")]
    [SerializeField] private float survivalSeconds = 0f;
    [SerializeField] private string gameMode = "normal";

    private string playerName = "Player";
    private string gameVersion;
    private int currentScore;
    private bool hasSubmitted = false;
    private bool entersTop10 = false;

    [System.Serializable]
    private class CheckScorePayload
    {
        public int score;
        public float survival_seconds;
        public string game_version;
        public string game_mode;
    }

    [System.Serializable]
    private class CheckScoreResponse
    {
        public bool success;
        public bool would_enter_top_10;
    }

    [System.Serializable]
    private class ScorePayload
    {
        public string player_name;
        public int score;
        public float survival_seconds;
        public string game_version;
        public string game_mode;
    }

    private void Awake()
    {
        gameVersion = Application.version;
        Debug.Log($"Game version: {gameVersion}");
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && PanelGameOver != null)
        {
            StartCoroutine(MonitorPanelActivation());
        }

        // Configurar botón confirmar del ContenedorDialogos
        if (btnConfirmar != null)
        {
            btnConfirmar.onClick.AddListener(OnConfirmarNombre);
        }
    }

    private IEnumerator MonitorPanelActivation()
    {
        while (!PanelGameOver.activeInHierarchy && !hasSubmitted)
            yield return null;

        if (!hasSubmitted)
        {
            Debug.Log("PanelGameOver activado. Comprobando top 10...");
            currentScore = KrakenControl.score;
            yield return StartCoroutine(CheckTop10());
        }
    }

    private IEnumerator CheckTop10()
    {
        CheckScorePayload checkPayload = new CheckScorePayload
        {
            score = currentScore,
            survival_seconds = survivalSeconds,
            game_version = gameVersion,
            game_mode = gameMode
        };

        string jsonCheck = JsonUtility.ToJson(checkPayload);
        Debug.Log($"JSON check top10: {jsonCheck}");

        // FIX WebGL: Usar UnityWebRequest.Post() en lugar de UploadHandler manual
        using (UnityWebRequest www = UnityWebRequest.Post(
            "https://retroteca.org/wp-json/krakenbius/v1/check-score", 
            jsonCheck, 
            "application/json"))
        {
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");
            
            yield return www.SendWebRequest();

            Debug.Log($"CheckTop10 result: {www.result}, error: {www.error}");

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error check top10: {www.error}\nResponse: {www.downloadHandler.text}");
                SubmitScore(); // Envía sin nombre si falla
                yield break;
            }

            Debug.Log($"Check top10 response RAW: {www.downloadHandler.text}");
            
            CheckScoreResponse response = JsonUtility.FromJson<CheckScoreResponse>(www.downloadHandler.text);
            entersTop10 = response.success && response.would_enter_top_10;

            if (entersTop10)
            {
                Debug.Log("¡TOP 10! Abriendo ContenedorDialogos para nombre...");
                ShowContenedorDialogos();
            }
            else
            {
                playerName = "Player";
                SubmitScore();
            }
        }
    }

    private void ShowContenedorDialogos()
    {
        if (ContenedorDialogos != null)
        {
            inputNombre.text = playerName; // "Player" por defecto
            ContenedorDialogos.SetActive(true);
            inputNombre.Select();
            inputNombre.ActivateInputField();
        }
    }

    private void OnConfirmarNombre()
    {
        playerName = string.IsNullOrWhiteSpace(inputNombre.text) ? "Player" : inputNombre.text;
        Debug.Log($"Nombre para TOP 10: {playerName}");
        
        ContenedorDialogos.SetActive(false);
        SubmitScore();
    }

    private void SubmitScore()
    {
        StartCoroutine(SubmitScoreCoroutine());
    }

    private IEnumerator SubmitScoreCoroutine()
    {
        hasSubmitted = true;

        ScorePayload payload = new ScorePayload
        {
            player_name = playerName,
            score = currentScore,
            survival_seconds = survivalSeconds,
            game_version = gameVersion,
            game_mode = gameMode
        };

        string jsonBody = JsonUtility.ToJson(payload);
        Debug.Log($"JSON submit score: {jsonBody}");

        // FIX WebGL: Usar UnityWebRequest.Post() en lugar de UploadHandler manual
        using (UnityWebRequest www = UnityWebRequest.Post(
            "https://retroteca.org/wp-json/krakenbius/v1/submit-score", 
            jsonBody, 
            "application/json"))
        {
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Error submit score: {www.error}\nResponse: {www.downloadHandler.text}");
            else
                Debug.Log($"Score {currentScore} de '{playerName}' enviado: {www.downloadHandler.text}");
        }
    }
}