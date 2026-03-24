using UnityEngine;
using System.Collections;

public class ChangeMaterialOffset : MonoBehaviour
{
    public Vector2 offsetSpeed = new Vector2(0, 0.01f);  // Velocidad de desplazamiento de textura (x,y)

    Material mat;  // Material instanciado para modificar offset independientemente

    void Start()
    {
        // Crea copia del material original para evitar afectar otros objetos con mismo material
        mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = mat;
    }

    void Update()
    {
        // Anima textura desplazando mainTextureOffset continuamente
        Vector2 offset = mat.mainTextureOffset;
        offset += offsetSpeed * Time.deltaTime;  // Incrementa offset por velocidad y tiempo

        // Wraparound infinito: reinicia coordenada cuando supera límites [0,1]
        if (Mathf.Abs(offset.x) >= 1)
            offset.x -= Mathf.Sign(offset.x);
        if (Mathf.Abs(offset.y) >= 1)
            offset.y -= Mathf.Sign(offset.y);

        mat.mainTextureOffset = offset;  // Aplica nuevo offset al material
    }
}