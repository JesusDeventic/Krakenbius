using UnityEngine;
using System.Collections;
using models;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RankingManager2 : MonoBehaviour 
{
    [Header("Configuración del Ranking")]
    public GameObject prefabItemRanking;
    
    void Start()
    {
        StartCoroutine(LoadRanking());  // Inicia carga automática del ranking TOP10 al arrancar
    }
    
    // Realiza petición HTTP GET al endpoint de leaderboard y procesa respuesta
    IEnumerator LoadRanking() 
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://retroteca.org/wp-json/krakenbius/v1/leaderboard?limit=10"))
        {
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");
            
            yield return www.SendWebRequest();

            // Manejo completo de errores HTTP con códigos y respuestas
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al cargar ranking: {www.error} (Código: {www.responseCode})\nRespuesta: {www.downloadHandler.text}");
            }
            else 
            {
                Debug.Log("Ranking cargado exitosamente");
                DrawRanking(www.downloadHandler.text);  // Parsea y renderiza TOP10
            }
        }
    }

    // Parsea JSON con SimpleJSON > Limpia contenedor anterior > Instancia y popula prefab para cada jugador TOP10
    private void DrawRanking(string respuesta_json)
    {
        SimpleJSON.JSONNode jsonData = SimpleJSON.JSON.Parse(respuesta_json);
        if (jsonData == null)
        {
            Debug.LogError("Error al parsear el JSON de respuesta");
            return;
        }

        SimpleJSON.JSONNode resultsNode = jsonData["results"];
        if (resultsNode == null)
        {
            Debug.LogError("No se encontró el campo 'results' en el JSON");
            return;
        }

        SimpleJSON.JSONArray playersArray = resultsNode.AsArray;
        if (playersArray == null)
        {
            Debug.LogError("El campo 'results' no es un array");
            return;
        }

        Transform contenedorItems = GameObject.Find("PlayersContainer")?.transform;
        if (contenedorItems == null)
        {
            Debug.LogError("¡No se encontró 'PlayersContainer' en la escena!");
            return;
        }

        // Limpia items anteriores del contenedor
        foreach (Transform child in contenedorItems)
        {
            Destroy(child.gameObject);
        }

        int pos = 0;
        
        // Itera TOP10 jugadores: extrae nick/score > valida > instancia prefab > popula Texts por índice hijo
        foreach (SimpleJSON.JSONNode playerNode in playersArray)
        {
            SimpleJSON.JSONClass playerJson = playerNode as SimpleJSON.JSONClass;
            if (playerJson == null) 
            {
                Debug.LogWarning($"Nodo inválido en posición {pos}");
                continue;
            }

            string nick = playerJson["player_name"]?.Value ?? "N/A";
            string scoreStr = playerJson["score"]?.Value ?? "0";
            
            if (int.TryParse(scoreStr, out int score))
            {
                if (prefabItemRanking != null)
                {
                    GameObject nuevoItem = Instantiate(prefabItemRanking);
                    nuevoItem.transform.SetParent(contenedorItems, false);
                    
                    // Acceso por índices de hijos del prefab (0=Pos, 1=Nick, 2=Score)
                    Text textoPosicion = nuevoItem.transform.GetChild(0).GetComponent<Text>();
                    Text textoNick = nuevoItem.transform.GetChild(1).GetComponent<Text>();
                    Text textoScore = nuevoItem.transform.GetChild(2).GetComponent<Text>();

                    if (textoPosicion != null) 
                        textoPosicion.text = (pos + 1).ToString();
                    if (textoNick != null) 
                        textoNick.text = nick.ToUpper();
                    if (textoScore != null) 
                        textoScore.text = score.ToString();
                }
                else
                {
                    Debug.LogWarning("PrefabItemRanking no está asignado en el Inspector");
                }
            }
            else
            {
                Debug.LogWarning($"Score inválido para jugador {nick}: {scoreStr}");
            }

            pos++;
        }
        
        Debug.Log($"Ranking dibujado exitosamente: {pos} jugadores");
    }
}