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
            // VERIFICAR PISADA DESDE ARRIBA
            foreach (ContactPoint2D puntoContacto in other.contacts)
            {
                if (puntoContacto.normal.y < -0.5f) 
                {
                    // === FILTRO INTELIGENTE PARA TU VILLANO MULTIPLAYER ===
                    // Si el objeto tiene las mecánicas del villano, delegamos el daño a sus pisotones independientes
                    VillanoAtaqueFuego ataqueVillano = GetComponent<VillanoAtaqueFuego>();
                    if (ataqueVillano != null)
                    {
                        ataqueVillano.RecibirPisoton(); 

                        // Hacer rebotar al jugador encima del jefe
                        Rigidbody2D rbJefe = other.gameObject.GetComponent<Rigidbody2D>();
                        if (rbJefe != null)
                        {
                            caminar scriptJugador = other.gameObject.GetComponent<caminar>();
                            float fuerzaRebote = (scriptJugador != null) ? scriptJugador.fuerzaSalto * 0.8f : 8f;
                            rbJefe.velocity = new Vector2(rbJefe.velocity.x, fuerzaRebote);
                        }
                        return; // Salimos para evitar que muera inmediatamente
                    }

                    // LÓGICA PARA ENEMIGOS COMUNES DEL MAPA (Rocas, magos, etc.)
                    _yaMurio = true; 

                    Rigidbody2D rbJugador = other.gameObject.GetComponent<Rigidbody2D>();
                    if (rbJugador != null)
                    {
                        caminar scriptJugador = other.gameObject.GetComponent<caminar>();
                        float fuerzaRebote = (scriptJugador != null) ? scriptJugador.fuerzaSalto * 0.8f : 8f;
                        rbJugador.velocity = new Vector2(rbJugador.velocity.x, fuerzaRebote);
                    }

                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger(TRIGGER_MUERTE); 
                    }

                    Collider2D collider = GetComponent<Collider2D>();
                    if (collider != null) { collider.enabled = false; }

                    if (TryGetComponent<EnemigoPatrulla>(out var patrulla)) { patrulla.enabled = false; }
                    if (TryGetComponent<AnimadorEnemigo>(out var animMovimiento)) { animMovimiento.enabled = false; }
                    
                    return; 
                }
            }

            EjecutarDanoAlJugador(other);
        }
    }

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
