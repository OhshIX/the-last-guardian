using UnityEngine;

public class RotarObjeto : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad de giro. Valores positivos giran a un lado, negativos al otro.")]
    public float velocidadRotacion = 10f;

    void Update()
    {
        // Gira el objeto continuamente en el eje Y (vertical)
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);
    }
}
