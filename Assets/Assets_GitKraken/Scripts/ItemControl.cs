using UnityEngine;
using System.Collections;

public class ItemControl : MonoBehaviour
{
    public GameObject kraken;
    public float deathDistance = 1f;
    public float speed = 1f;
    float initScale = 0.5f;
    float finalScale = 0.2f;
    Vector3 initPos;
    Vector3 finalPos;
    float timeToDie;
    float spendTime = 0;
    public GameObject effect;

    // Inicializa trayectoria: spawnea en círculo r=√50, rota aleatoriamente y calcula path hacia Kraken
    public void Start()
    {
        speed = CreateItems.speed;

        float r = Random.Range(0f, 360f);
        this.transform.position = new Vector3(Mathf.Sqrt(50), 0, 0);
        this.transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, r);
        initPos = transform.position;

        Vector3 direction = (kraken.transform.position - initPos);
        finalPos = kraken.transform.position - deathDistance * direction.normalized;
        timeToDie = (initPos - finalPos).magnitude / speed;

        this.transform.rotation = Quaternion.identity;
    }

    // Anima item cada frame: reduce escala, mueve hacia Kraken y autodestruye al llegar
    void Update()
    {
        spendTime += Time.deltaTime;

        // Escala de 0.5 > 0.2 progresivamente
        this.transform.localScale = Vector3.Lerp(initScale * Vector3.one, finalScale * Vector3.one, spendTime / timeToDie);

        // Movimiento lineal initPos > finalPos
        this.transform.position = Vector3.Lerp(initPos, finalPos, spendTime / timeToDie);

        // Autodestrucción al completar trayectoria
        if (spendTime >= timeToDie)
        {
            Destroy(this.gameObject);
        }
    }

    // Maneja colisión con Kraken: efecto especial para "Rebase", explosión normal para otros
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (this.transform.gameObject.tag == "Rebase")
        {
            Instantiate(effect, kraken.transform.position + new Vector3(0, 3.5f, 1f), Quaternion.identity);
        }
        else
        {
            Explosion();
        }

        Destroy(this.gameObject);
    }

    // Instancia prefab de explosión en posición actual del item
    public void Explosion()
    {
        Instantiate(effect, this.transform.position, Quaternion.identity);
    }
}