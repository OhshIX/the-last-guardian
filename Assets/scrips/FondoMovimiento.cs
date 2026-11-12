using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    // Referencia a la cámara principal (se puede asignar desde el Inspector)
    [SerializeField] private Transform camara;

    // Velocidad del parallax (0 = no se mueve, 1 = se mueve igual que la cámara)
    [SerializeField] private float velocidadParallax = 0.5f;

    // Guarda la posición original del fondo
    private Vector3 posicionInicial;

    private void Start()
    {
        // Si no se asignó la cámara, toma automáticamente la cámara principal
        if (camara == null)
        {
            camara = Camera.main.transform;
        }

        // Guarda la posición inicial del fondo para calcular el desplazamiento
        posicionInicial = transform.position;
    }

    private void LateUpdate()
    {
        // Calcula el desplazamiento del fondo según la posición de la cámara y la velocidad del parallax
        float desplazamientoX = camara.position.x * velocidadParallax;
        float desplazamientoY = camara.position.y * velocidadParallax;

        // Aplica el efecto de parallax manteniendo la profundidad original (eje Z)
        transform.position = new Vector3(
            posicionInicial.x + desplazamientoX,
            posicionInicial.y + desplazamientoY,
            transform.position.z
        );
    }
}
