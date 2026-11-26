using UnityEngine;

public class RotarLuna : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float velocidadGiro = 5f; // Puedes cambiar la velocidad desde el Inspector

    void Update()
    {
        // Hace que el objeto 3D gire en su eje Y (vertical) de forma suave con el tiempo
        transform.Rotate(Vector3.up * velocidadGiro * Time.deltaTime);
    }
}

