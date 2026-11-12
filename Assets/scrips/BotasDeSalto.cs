using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotasDeSalto : MonoBehaviour
{
    public float saltoPotenciado = 20f;
    public float duracion = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caminar jugador = other.GetComponent<caminar>();
            if (jugador != null)
            {
                jugador.ActivarSaltoPotenciado(saltoPotenciado, duracion);
                Destroy(gameObject); // Eliminar el power-up
            }
        }
    }
}
