using UnityEngine;
using UnityEngine.UI;

public class GameVersionLabel : MonoBehaviour
{
    [SerializeField] private Text uiText;

    private void Awake()
    {
        if (uiText == null)
            uiText = GetComponent<Text>();

        uiText.text = "Game version: " + Application.version;
    }
}
