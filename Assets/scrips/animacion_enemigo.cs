

using UnityEngine;
using System.Collections;

public class AnimadorEnemigo : MonoBehaviour 
{
    private Animator animator;
    private Vector3 posicionPrevia;
    private bool estaMuerto = false;

    [Header("Tipo de Enemigo")]
    [Tooltip("Marca esto SOLO si este objeto es el enemigo de la Roca que camina")]
    [SerializeField] private bool esEnemigoRoca = false;

    [Tooltip("Marca esto SI este objeto es el Mago que vuela o flota")]
    [SerializeField] private bool esEnemigoMago = false;

    [Header("Configuración de Ataque Mago")]
    [SerializeField] private float tiempoEntreAtaques = 3f;

    void Start() 
    {
        animator = GetComponent<Animator>();
        posicionPrevia = transform.position;

        // Si es el mago, inicia su rutina automática de ataque
        if (esEnemigoMago && !esEnemigoRoca)
        {
            StartCoroutine(RutinaAtaqueMago());
        }
    }

    void Update() 
    {
        if (estaMuerto || animator == null) return;

        // ========================================================
        // LÓGICA DE LA ROCA (TU CÓDIGO ORIGINAL SIN NINGÚN CAMBIO)
        // ========================================================
        if (esEnemigoRoca) 
        {
            float movimientoX = Mathf.Abs(transform.position.x - posicionPrevia.x) / Time.deltaTime;
            
            if (TieneParametro("VELOCIDAD", animator)) 
            {
                animator.SetFloat("VELOCIDAD", movimientoX);
            }
        }
        // ========================================================
        // NUEVA LÓGICA EXCLUSIVA PARA EL MAGO FLOTANTE
        // ========================================================
        else if (esEnemigoMago)
        {
            if (TieneParametro("VELOCIDAD", animator)) 
            {
                animator.SetFloat("VELOCIDAD", 0f); // Mantiene al mago en su animación de flotado (Idle)
            }
        }

        posicionPrevia = transform.position;
    }

    // Corrutina que hace que el mago ejecute la animación de ATAQUE de forma automática
    IEnumerator RutinaAtaqueMago()
    {
        while (!estaMuerto)
        {
            yield return new WaitForSeconds(tiempoEntreAtaques);

            if (animator != null && !estaMuerto)
            {
                if (TieneParametro("ATAQUE", animator))
                {
                    animator.SetTrigger("ATAQUE");
                }
            }
        }
    }

    public void Morir() 
    {
        if (estaMuerto) return;
        estaMuerto = true;
        
        StopAllCoroutines(); 

        if (animator != null) 
        {
            if (TieneParametro("DEAD", animator)) 
                animator.SetTrigger("DEAD");
            else if (TieneParametro("MORIR", animator)) 
                animator.SetTrigger("MORIR");
        }
        
        this.enabled = false;
    }

    private bool TieneParametro(string nombreParametro, Animator anim) 
    {
        foreach (AnimatorControllerParameter param in anim.parameters) 
        {
            if (param.name == nombreParametro) return true;
        }
        return false;
    }

        public void LanzarAtaqueEvent()
    {
        // Se deja vacío de forma intencional para silenciar la alerta de Unity
    }

}
