using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocidadRelampago : MonoBehaviour
{
    public float velocidadAumentada = 10f;   // Velocidad temporal
    public float duracion = 5f;              // Duraci�n del efecto

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caminar jugador = other.GetComponent<caminar>();
            if (jugador != null)
            {
                jugador.ActivarVelocidadBoost(velocidadAumentada, duracion);
                Destroy(gameObject); // Elimina el power-up despu�s de usarse
            }
        }
    }
}
