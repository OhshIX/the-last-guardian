using UnityEngine;

public class CieloInfinito : MonoBehaviour
{
    public float velocidadX = 0.03f;
    private Renderer rend;
    private Vector2 desplazamiento;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        desplazamiento.x += velocidadX * Time.deltaTime;
        rend.material.mainTextureOffset = desplazamiento;
    }
}
