
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemigoPatrulla : MonoBehaviour
{
    public Transform[] puntosPatrulla;   // Puntos de patrulla asignados desde el Inspector
    public float velocidad = 2f;         // Velocidad de movimiento
    private int siguientePunto = 0;      // Índice del siguiente punto al que se moverá
    private bool mirandoDerecha = true;  // Dirección actual del enemigo
 
    private void Update()
    {
        // Si no hay puntos, no hacer nada
        if (puntosPatrulla.Length == 0)
            return;
 
        // Mover al enemigo hacia el siguiente punto
        Transform objetivo = puntosPatrulla[siguientePunto];
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);
 
        // Si llega al punto objetivo
        if (Vector2.Distance(transform.position, objetivo.position) < 0.1f)
        {
            siguientePunto++;
 
            // Si llegó al último punto, volver al primero
            if (siguientePunto >= puntosPatrulla.Length)
                siguientePunto = 0;
        }
 
        // Verifica si necesita voltearse
        if ((objetivo.position.x < transform.position.x && mirandoDerecha) ||
            (objetivo.position.x > transform.position.x && !mirandoDerecha))
        {
            Flip();
        }
    }
 
    // Método para voltear al enemigo
    void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}