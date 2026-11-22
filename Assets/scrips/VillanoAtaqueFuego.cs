using UnityEngine;
using System.Collections;

public class VillanoAtaqueFuego : MonoBehaviour
{
    private Animator animator;
    private bool estaMuerto = false;

    [Header("Referencias de Ataque")]
    [SerializeField] private GameObject prefabBolaFuego; 
    [SerializeField] private Transform puntoDisparo;     

    [Header("Sistema de Vidas (Pisotones)")]
    [SerializeField] private int vidasPisotones = 3; 

    void Start()
    {
        animator = GetComponent<Animator>();
        // El inicio automático está desactivado para que te obedezca solo al presionar la P.
    }

    // Llamado automáticamente por el Animation Event "LanzarProyectilFuego"
    public void LanzarProyectilFuego()
    {
        if (estaMuerto || prefabBolaFuego == null || puntoDisparo == null) return;

        GameObject bola = Instantiate(prefabBolaFuego, puntoDisparo.position, Quaternion.identity);
        BolaDeFuego scriptBola = bola.GetComponent<BolaDeFuego>();

        if (scriptBola != null)
        { 
            float direccionX = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 direccionFinal = new Vector2(direccionX, 0f);
            scriptBola.ConfigurarDireccion(direccionFinal);
        }
    }

    // Única condición de muerte: controlada por pisotones del jugador
    public void RecibirPisoton()
    {
        if (estaMuerto) return;

        vidasPisotones--; // Resta una vida de pisotón
        Debug.Log("¡Pisotón al villano! Vidas restantes: " + vidasPisotones);

        if (vidasPisotones <= 0)
        {
            estaMuerto = true;

            // Ejecuta la animación de muerte del villano si existe el parámetro
            if (animator != null && TieneParametro("MORIR", animator))
            {
                animator.SetTrigger("MORIR");
            }

            // Detiene físicamente al villano si usa el script de caminar
            caminar scriptCaminar = GetComponent<caminar>();
            if (scriptCaminar != null)
            {
                scriptCaminar.Morir();
            }

            this.enabled = false; // Apaga este script de ataque
        }
    }

    private bool TieneParametro(string nombreParametro, Animator anim)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == nombreParametro) return true;
        }
        return false;
    }
}
