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

    void Start() 
    {
        animator = GetComponent<Animator>();
        posicionPrevia = transform.position;
    }

    void Update() 
    {
        if (estaMuerto || animator == null) return;

        // Lógica limpia de la roca que camina (se eliminó el comentario incompleto de esEnemigoMago)
        if (esEnemigoRoca) 
        {
            float movimientoX = Mathf.Abs(transform.position.x - posicionPrevia.x) / Time.deltaTime;
            
            // OPTIMIZACIÓN: Se eliminó el "else if" repetido que hacía exactamente la misma validación
            if (TieneParametro("VELOCIDAD", animator)) 
            {
                animator.SetFloat("VELOCIDAD", movimientoX);
            }
        }

        posicionPrevia = transform.position;
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
}
