using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource base_1;
    public AudioSource base_2;
    public AudioClip[] audios;
    float initVolume;
    int audioIndex = 0;
    public AudioSource menu_music;
    public AudioMixer audioMixer;
    private CreateItems createItems;

    bool swapping = false;
    private int stage;

    // Cambia canción automáticamente cuando se modifica el stage
    public int Stage
    {
        get { return stage; }
        set
        {
            if (value != stage) // Solo cambia si el valor es diferente
            {
                stage = value;
                SwapSong();
            }
        }
    }

    void Awake()
    {
        initVolume = base_1.volume;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            menu_music.Play();
            audioMixer.FindSnapshot("Menu").TransitionTo(1f);
        }
        else if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            base_1.Play();
            audioMixer.FindSnapshot("Base_1").TransitionTo(1f);
            createItems = Object.FindFirstObjectByType<CreateItems>();
        }
    }

    // Inicia corrutina para bajar pitch gradualmente usando AudioMixer
    public void PitchBase()
    {
        StartCoroutine(PitchBaseCoroutine());
    }

    // Aplica snapshot de pitch bajo durante 2s y detiene ambas pistas de audio
    IEnumerator PitchBaseCoroutine()
    {
        audioMixer.FindSnapshot("Base_1_PitchDown").TransitionTo(2f);
        yield return new WaitForSeconds(2);
        base_1.Stop();
        base_2.Stop();
    }

    // Inicia transición suave (crossfade) a la siguiente canción si no está en progreso
    public void SwapSong()
    {
        if (!swapping && base_1.isPlaying)
        {
            StartCoroutine(SwapSongCoroutine());
        }
    }

    // Realiza crossfade de 1s entre canciones: fade out base_1, fade in base_2, luego intercambia roles
IEnumerator SwapSongCoroutine()
{
    audioIndex++;
    audioIndex %= audios.Length;
    base_2.Stop();
    base_2.clip = audios[audioIndex];
    base_2.Play();
    swapping = true;
    float progress = 0;
    float fadeDuration = 1f;
    while (progress < 1)
    {
        progress += Time.deltaTime / fadeDuration;
        progress = Mathf.Clamp01(progress);
        
        base_1.volume = Mathf.Lerp(initVolume, 0, progress);
        base_2.volume = Mathf.Lerp(0, initVolume, progress);
        yield return null;
    }
    
    AudioSource temp = base_1;
    base_1 = base_2;
    base_2 = temp;
    swapping = false;
}

    // Pausa música principal y secundaria si está en transición
    public void pauseMusic()
    {
        base_1.Pause();
        if (swapping)
            base_2.Pause();
    }

    // Reanuda música desde el punto donde se pausó
    public void resumeMusic()
    {
        base_1.Play();
        if (swapping)
            base_2.Play();
    }
}