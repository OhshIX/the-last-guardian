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

    public VillanoAtaqueFuego scriptFuego;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_yaMurio) return; 

        if (other.gameObject.CompareTag("Player"))
        {
            // 1. VERIFICAR PISADA DESDE ARRIBA (Muerte de cualquier enemigo)
            foreach (ContactPoint2D puntoContacto in other.contacts)
            {
                if (puntoContacto.normal.y < -0.5f) 
                {
                    _yaMurio = true; 

                    // Hacer rebotar al jugador que cayó encima
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

                    // Desactivar físicas y colisiones inmediatas para no dañar al jugador mientras cae
                    Collider2D collider = GetComponent<Collider2D>();
                    if (collider != null) { collider.enabled = false; }

                    // Apagar scripts de movimiento de patrulla base si el enemigo los tiene
                    if (TryGetComponent<EnemigoPatrulla>(out var patrulla)) { patrulla.enabled = false; }

                    // Apagar el script calculador de animaciones de movimiento si lo tiene
                    if (TryGetComponent<AnimadorEnemigo>(out var animMovimiento)) { animMovimiento.enabled = false; }
                    
                    // === SOLUCIÓN GENÉRICA INTELIGENTE PARA EL VILLANO ===
                    // Si el objeto pisado tiene el script de ataque del villano en bucle, apaga sus disparos
                    if (TryGetComponent<VillanoAtaqueFuego>(out var ataqueVillano)) 
                    { 
                        scriptFuego.DesactivarVillano(); 
                    }
                    
                    return; 
                }
            }


            EjecutarDanoAlJugador(other);
        }
    }

    // MÉTODO PARA DESTRUIR AL ENEMIGO (Llamado únicamente por el Animation Event azul al final del clip de muerte)
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