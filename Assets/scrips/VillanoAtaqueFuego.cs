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

    [Header("Tiempos de Combate")]
    [SerializeField] private float tiempoEntreAtaques = 2.5f; 

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BucleAtaqueJefe());
    }

    IEnumerator BucleAtaqueJefe()
    {
        yield return new WaitForSeconds(1.5f);

        while (!estaMuerto && !jugadorEstaMuerto)
        {
            if (animator != null)
            {
                if (TieneParametro("ATAQUE", animator)) animator.SetTrigger("ATAQUE");
                else if (TieneParametro("ataque", animator)) animator.SetTrigger("ataque");
            }

            yield return new WaitForSeconds(tiempoEntreAtaques);
        }
    }

    public void LanzarProyectilFuego()
{
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

    public void DesactivarVillano() 
    {
        estaMuerto = true;
        StopAllCoroutines();
        this.enabled = false;
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