using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour
{
    public float TimeToDie = 2;

    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Inicia cuenta regresiva y destruye GameObject tras tiempo especificado
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(TimeToDie);
        Destroy(gameObject);
    }
}