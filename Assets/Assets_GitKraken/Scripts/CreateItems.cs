using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateItems : MonoBehaviour
{
    public GameObject item_conflict;
    public GameObject item_branch;
    public GameObject item_commit;
    public GameObject item_push;
    public GameObject item_pull;
    public GameObject item_merge;
    public GameObject item_rebase;
    public GameObject stage_value;
    public GameObject nextStageEffectPrefab;
    public GameObject rebaseDummyEffectPrefab;

    public int stage; // Fase actual
    public int level; // Nivel dentro de fase
    public static float speed;
    float seconds;

    private AudioManager audioManager; // Nueva referencia

    void Start()
    {
        stage = 1;
        level = 0;
        speed = 1f;
        seconds = 1f;
        stage_value.GetComponent<Text>().text = "v " + stage.ToString() + "." + level.ToString();
        
        // Buscar AudioManager al inicio
        audioManager = Object.FindFirstObjectByType<AudioManager>();
        
        StartCoroutine(GenerateItem());
    }

    // Genera items aleatorios infinitamente con probabilidades: commit(20%), pull(9%), push(16%), merge(10%), branch(5%), rebase(3%), conflict(37%)
    IEnumerator GenerateItem()
    {
        for (; ; )
        {
            int r = Random.Range(1, 101);

            if (r >= 1 && r <= 20)
                Instantiate(item_commit);
            else if (r >= 21 && r <= 29)
                Instantiate(item_pull);
            else if (r >= 30 && r <= 45)
                Instantiate(item_push);
            else if (r >= 46 && r <= 55)
                Instantiate(item_merge);
            else if (r >= 56 && r <= 60)
                Instantiate(item_branch);
            else if (r >= 61 && r <= 63)
                Instantiate(item_rebase);
            else if (r >= 64 && r <= 100)
                Instantiate(item_conflict);

            // Progreso: cada 10 niveles > NextStage() ... cada nivel reduce tiempo de spawn (seconds -= 0.1f)
            if (krakenScripts.KrakenControl.score >= ((stage - 1) * (500 * Mathf.Pow(1.5f, 10))) + (500 * Mathf.Pow(1.5f, level + 1)))
            {
                level++;

                if (level == 10)
                {
                    NextStage();
                }
                else
                {
                    seconds -= 0.1f;
                }
                stage_value.GetComponent<Text>().text = "v " + stage.ToString() + "." + level.ToString();
                print("Level Up!! stage = " + stage + ", Level = " + level + ", Speed = " + speed + ", Seconds = " + seconds);
            }

            yield return new WaitForSeconds(seconds);
        }
    }

    // Avanza al siguiente stage: CAMBIA CANCIÓN PRIMERO, luego reinicia level=0, aumenta speed, destruye todos items
    void NextStage()
    {
        // Cambiar canción ANTES de destruir los items
        if (audioManager != null)
        {
            audioManager.Stage = stage;
        }
        
        stage++;
        level = 0;
        speed += 0.2f;
        seconds = 1f;
        ItemControl[] items = Object.FindObjectsByType<ItemControl>(FindObjectsSortMode.None);

        for (int i = items.Length - 1; i >= 0; i--)
        {
            if (items[i].gameObject.tag == "Rebase")
            {
                Instantiate(rebaseDummyEffectPrefab, items[i].transform.position, Quaternion.identity);
            }
            else
            {
                items[i].Explosion();
            }
            Destroy(items[i].gameObject);
        }
        Instantiate(nextStageEffectPrefab, transform.position + new Vector3(0, -3, -1), Quaternion.identity);
    }
}