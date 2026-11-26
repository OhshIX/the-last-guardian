using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Vida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance == null)
                return;

            bool vidaRecuperada = GameManager.Instance.RecuperarVida();

            if (vidaRecuperada)
            {
                Destroy(gameObject);
            }
        }
    }
}
