using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeNivel : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [SerializeField] private bool esElUltimoNivel = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
           
            {   if (esElUltimoNivel)
                {
                    GameManager.Instance.Ganaste();
                }
                    else{
                    GameManager.Instance.CambioDeNivel();
                        }   
            }
        }
    }

}    
