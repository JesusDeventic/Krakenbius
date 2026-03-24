using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Clase intermediaria para mostrar diálogo ranking al final partida
public class DialogRankingController : MonoBehaviour {

    public Text dialogText;
    public InputField newNick;
    public Button sendButton;
    public GameObject contenedor;
    int score;

    void Start(){
        score = krakenScripts.KrakenControl.score;  // Captura score final del juego
        contenedor.SetActive(false);  // Oculta panel de ranking inicialmente
    }

    // Valida input nick en tiempo real: habilita botón enviar si no está vacío
    public void CheckValidNick()
    {
        sendButton.interactable = !string.IsNullOrEmpty(newNick.text);
    }
    
    // Muestra panel diálogo para verificar ranking TOP10 (método comentado)
    public void FinalOfMatch(){
        //StartCoroutine (manager.CheckInRankingReq(score,contenedor));
    }

    // Envía nuevo nick y score al ranking (método comentado)
    public void OkButtonClick(){
        //StartCoroutine (manager.UpdateRanking(newNick.text,contenedor));
    }

    // Oculta panel de ranking sin guardar
    public void CancelButtonClick(){
        contenedor.SetActive (false);
    }
}