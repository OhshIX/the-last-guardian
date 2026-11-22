using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Configuración de Muerte Universal")]
    private const string TRIGGER_MUERTE = "MORIR"; 

    [Header("Configuración de Daño")]
    public float fuerzaEmpuje = 50f;
    public Vector2 direccionEmpuje = Vector2.right;
    public Color colorGolpe = Color.red;
    public float tiempoColorCambio = 2f; 
    public AudioClip Dano;

    private bool _yaMurio = false;
    private Color colorOriginal;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_yaMurio) return; 

        if (other.gameObject.CompareTag("Player"))
        {
            // VERIFICAR PISADA DESDE ARRIBA (Muerte de enemigos ambientales comunes)
            foreach (ContactPoint2D puntoContacto in other.contacts)
            {
                // Si el jugador cae desde arriba
                if (puntoContacto.normal.y < -0.5f) 
                {
                    // === MODIFICACIÓN PROFESIONAL: FILTRAR Y RESPETAR AL VILLANO ===
                    // Si el objeto que pisamos es el Villano Multiplayer, le cedemos el control a su propio sistema de 3 vidas
                    VillanoAtaqueFuego ataqueVillano = GetComponent<VillanoAtaqueFuego>();
                    if (ataqueVillano != null)
                    {
                        ataqueVillano.RecibirPisoton(); // Ejecuta 1 pisotón de los 3 necesarios
                        
                        // Hacemos rebotar al jugador que cayó encima del jefe
                        Rigidbody2D rbJefe = other.gameObject.GetComponent<Rigidbody2D>();
                        if (rbJefe != null)
                        {
                            caminar scriptJugador = other.gameObject.GetComponent<caminar>();
                            float fuerzaRebote = (scriptJugador != null) ? scriptJugador.fuerzaSalto * 0.8f : 8f;
                            rbJefe.velocity = new Vector2(rbJefe.velocity.x, fuerzaRebote);
                        }
                        return; // Salimos de este script para que NO destruya al villano de inmediato
                    }

                    // LÓGICA PARA ENEMIGOS COMUNES DEL MAPA (Rocas, magos, etc.)
                    _yaMurio = true; 

                    // Hacer rebotar al jugador
                    Rigidbody2D rbJugador = other.gameObject.GetComponent<Rigidbody2D>();
                    if (rbJugador != null)
                    {
                        caminar scriptJugador = other.gameObject.GetComponent<caminar>();
                        float fuerzaRebote = (scriptJugador != null) ? scriptJugador.fuerzaSalto * 0.8f : 8f;
                        rbJugador.velocity = new Vector2(rbJugador.velocity.x, fuerzaRebote);
                    }

                    // Activar la animación de muerte unificada ("Morir")
                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger(TRIGGER_MUERTE); 
                    }

                    // Desactivar físicas inmediatas para el enemigo común
                    Collider2D collider = GetComponent<Collider2D>();
                    if (collider != null) { collider.enabled = false; }

                    if (TryGetComponent<EnemigoPatrulla>(out var patrulla)) { patrulla.enabled = false; }
                    if (TryGetComponent<AnimadorEnemigo>(out var animMovimiento)) { animMovimiento.enabled = false; }
                    
                    return; 
                }
            }

            // Si el choque no fue desde arriba, el enemigo le hace daño al jugador
            EjecutarDanoAlJugador(other);
        }
    }

    // MÉTODO PARA DESTRUIR AL ENEMIGO COMÚN (Llamado por el Animation Event al final del clip de muerte)
    public void Morir()
    {
        Destroy(gameObject);
    }

    private void EjecutarDanoAlJugador(Collision2D other)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerderVida();
        }
        
        if (AudioManager.Instance != null && Dano != null)
        {
            AudioManager.Instance.ReproducirSonido(Dano);
        }

        Rigidbody2D rbJugador = other.gameObject.GetComponent<Rigidbody2D>();
        if (rbJugador != null)
        {
            Vector2 direccionAjustada = transform.TransformDirection(direccionEmpuje).normalized;
            rbJugador.AddForce(direccionAjustada * fuerzaEmpuje, ForceMode2D.Impulse);
        }

        SpriteRenderer srJugador = other.gameObject.GetComponent<SpriteRenderer>();
        if (srJugador != null)
        {
            colorOriginal = srJugador.color;
            srJugador.color = colorGolpe;
            StartCoroutine(RestaurarColorDespuesDeTiempo(srJugador));
        }
    }

    private IEnumerator RestaurarColorDespuesDeTiempo(SpriteRenderer srJugador)
    {
        yield return new WaitForSeconds(tiempoColorCambio);
        if (srJugador != null)
        {
            srJugador.color = colorOriginal;
        }
    }
}
