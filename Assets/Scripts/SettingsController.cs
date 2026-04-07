using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    [Header("Audio Controll Buttons")]
    public Toggle musicButton;
    public Text musicSelector;
    public Toggle effectsButton;
    public Text effectsSelector;

    [Header("Music Mixer")]
    public AudioMixer audioMixer;

    private int musicState;
    private int effectsState;

    // Estado actual música (1=ON, 0=OFF) persistido en PlayerPrefs
    public int MusicState
    {
        get { return musicState; }
        set { musicState = value; }
    }

    // Estado actual efectos (1=ON, 0=OFF) persistido en PlayerPrefs
    public int EffectsState
    {
        get { return effectsState; }
        set { effectsState = value; }
    }

    // Carga preferencias de audio desde PlayerPrefs y sincroniza toggles/volume al iniciar
    void Start()
    {
        #region CheckAudioPrefs
        if (PlayerPrefs.HasKey("music"))
        {
            MusicState = PlayerPrefs.GetInt("music");
            musicButton.isOn = (MusicState == 1);
        }
        else
        {
            MusicState = musicButton.isOn ? 1 : 0;
            PlayerPrefs.SetInt("music", MusicState);
        }

        if (PlayerPrefs.HasKey("effects"))
        {
            EffectsState = PlayerPrefs.GetInt("effects");
            effectsButton.isOn = (EffectsState == 1);
        }
        else
        {
            EffectsState = effectsButton.isOn ? 1 : 0;
            PlayerPrefs.SetInt("effects", EffectsState);
        }
        #endregion
        PlayerPrefs.Save();
    }

    void Update()
    {
    }

    // Alterna música: reproduce SFX click > guarda PlayerPrefs > ajusta volumen mixer (0/-80dB) > muestra/oculta icono X
    public void toggleMusic()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        if (musicButton.isOn)
        {
            PlayerPrefs.SetInt("music", 1);
            audioMixer.SetFloat("MusicVolume", 0f);
            musicSelector.gameObject.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
            audioMixer.SetFloat("MusicVolume", -80f);
            musicSelector.gameObject.SetActive(false);
        }
    }

    // Alterna efectos: reproduce SFX click > guarda PlayerPrefs > ajusta volumen mixer (0/-80dB) > muestra/oculta icono X
    public void toggleEffects()
    {
        ((AudioSource)GameObject.Find("Mouse_Effect").GetComponent<AudioSource>()).Play();
        if (effectsButton.isOn)
        {
            PlayerPrefs.SetInt("effects", 1);
            audioMixer.SetFloat("EffectsVolume", 0f);
            effectsSelector.gameObject.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("effects", 0);
            audioMixer.SetFloat("EffectsVolume", -80f);
            effectsSelector.gameObject.SetActive(false);
        }
    }
}