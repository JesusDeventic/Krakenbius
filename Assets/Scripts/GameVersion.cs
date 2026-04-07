using UnityEngine;
using UnityEngine.UI;

public class GameVersionLabel : MonoBehaviour
{
    [SerializeField] private Text uiText;

    private void Awake()
    {
        // Busca Text component si no está asignado en inspector
        if (uiText == null)
            uiText = GetComponent<Text>();

        // Muestra versión actual de la aplicación en el UI Text
        uiText.text = "Game version: " + Application.version;
    }
}