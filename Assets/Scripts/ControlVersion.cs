using UnityEngine;
using UnityEngine.UI;

public class ControlsTextUpdater : MonoBehaviour
{
    [SerializeField] private Text controlsText; // Asigna el Text de "Controls" en inspector

    private void Awake()
    {
        // Busca Text si no asignado
        if (controlsText == null)
            controlsText = GameObject.Find("Controls")?.GetComponent<Text>();

        if (controlsText == null)
        {
            Debug.LogWarning("Text 'Controls' no encontrado");
            return;
        }

        UpdatePlatformControls();
    }

    public void UpdatePlatformControls()
    {
        if (controlsText == null) return;

        if (Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            controlsText.text = "A    D\n.\n<-   ->";
        }
        else if (Application.platform == RuntimePlatform.Android || 
                 Application.platform == RuntimePlatform.IPhonePlayer)
        {
            controlsText.text = "Touch screen\n.\nTap to rotate";
        }
        else
        {
            controlsText.text = "Controls";
        }
    }
}