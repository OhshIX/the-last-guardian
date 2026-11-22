using UnityEngine;
using System.Collections;

public class VillanoAtaqueFuego : MonoBehaviour
{
    private Animator animator;
    private bool estaMuerto = false;
    private bool jugadorEstaMuerto = false;

    [Header("Referencias de Ataque")]
    [SerializeField] private GameObject prefabBolaFuego; 
    [SerializeField] private Transform puntoDisparo;     

    [Header("Sistema de Vidas (Pisotones)")]
    [SerializeField] private int vidasPisotones = 3; // Única condición de muerte: 3 pisotones

    void Start()
    {
        animator = GetComponent<Animator>();
        // El inicio automático está completamente desactivado.
        // El villano solo atacará cuando tú presiones la tecla P.
    }

    // Llamado automáticamente por el Animation Event "LanzarProyectilFuego"
    public void LanzarProyectilFuego()
    {
        // NO hay contadores de ataque aquí. El villano puede disparar infinitamente.
        if (estaMuerto || jugadorEstaMuerto || prefabBolaFuego == null || puntoDisparo == null) return;

        GameObject bola = Instantiate(prefabBolaFuego, puntoDisparo.position, Quaternion.identity);
        BolaDeFuego scriptBola = bola.GetComponent<BolaDeFuego>();

        if (scriptBola != null)
        { 
            float direccionX = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 direccionFinal = new Vector2(direccionX, 0f);

            scriptBola.ConfigurarDireccion(direccionFinal);
        }
    }

    // =========================================================================
    // ÚNICA LÓGICA DE MUERTE: CONTROLADA POR PISOTONES DEL JUGADOR
    // =========================================================================
    public void RecibirPisoton()
    {
        if (estaMuerto) return;

        vidasPisotones--; // Resta una vida de pisotón

        Debug.Log("¡Pisotón detectado! Vidas del villano restantes: " + vidasPisotones);

        if (vidasPisotones <= 0)
        {
            EjecutarMuerteDefinitiva();
        }
    }

    private void EjecutarMuerteDefinitiva()
    {
        estaMuerto = true;
        StopAllCoroutines();

        // Ejecuta la animación de muerte del villano
        if (animator != null)
        {
            if (TieneParametro("MORIR", animator)) animator.SetTrigger("MORIR");
        }

        // Si el villano usa el script 'caminar', lo detenemos físicamente
        caminar scriptCaminar = GetComponent<caminar>();
        if (scriptCaminar != null)
        {
            scriptCaminar.Morir();
        }

        this.enabled = false; // Apaga este script de ataque
    }

    // Métodos de control e información externa
    public void DesactivarVillano() 
    {
        // Ya no se auto-llama desde el bucle de disparos viejos
        EjecutarMuerteDefinitiva();
    }

    public void InformarMuerteJugador()
    {
        jugadorEstaMuerto = true;
        StopAllCoroutines();
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
