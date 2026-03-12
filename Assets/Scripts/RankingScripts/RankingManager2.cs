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
        StartCoroutine(LoadRanking());
    }
    
    IEnumerator LoadRanking() 
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://retroteca.org/wp-json/krakenbius/v1/leaderboard?limit=10"))
        {
            www.SetRequestHeader("x-api-key", "krakenbius-apikey");
            
            // Envía la petición y espera respuesta
            yield return www.SendWebRequest();

            // Verifica si hubo algún error en la petición
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al cargar ranking: {www.error} (Código: {www.responseCode})\nRespuesta: {www.downloadHandler.text}");
            }
            else 
            {
                Debug.Log("Ranking cargado exitosamente");
                DrawRanking(www.downloadHandler.text);
            }
        }
    }

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

        foreach (Transform child in contenedorItems)
        {
            Destroy(child.gameObject);
        }

        int pos = 0;
        
        // Itera sobre cada jugador del ranking
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
            
            // Convierte score a entero (compatible con clase Player existente)
            if (int.TryParse(scoreStr, out int score))
            {
                if (prefabItemRanking != null)
                {
                    GameObject nuevoItem = Instantiate(prefabItemRanking);
                    nuevoItem.transform.SetParent(contenedorItems, false);
                    
                    // Acceso seguro a los componentes Text (índices 0,1,2)
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
