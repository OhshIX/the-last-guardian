using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : MonoBehaviour
{
    public float duracion = 5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caminar jugador = other.GetComponent<caminar>();
            if (jugador != null)
            {
                // Sincronizado con tu función exacta de "caminar"
                jugador.ActivarInmunidad(duracion);
                Destroy(gameObject);
            }
        }
    }
}
