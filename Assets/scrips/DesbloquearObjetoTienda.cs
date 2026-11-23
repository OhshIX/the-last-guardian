using UnityEngine;

public class DesbloquearObjetoTienda : MonoBehaviour
{
    [Header("Debe ser el mismo ID que pusiste en BotonCompra (ej. item_1, item_2, item_3)")]
    public string idCompra = "item_1";

    [Header("Si está marcado, solo aparece si ya se compró")]
    public bool ocultarSiNoComprado = true;

    [Header("Solo para pruebas: oculta el objeto aunque diga comprado")]
    public bool forzarOcultarEnPruebas = false;

    void Awake()
    {
        string clave = "Comprado_" + idCompra;
        bool comprado = PlayerPrefs.GetInt(clave, 0) == 1;

        Debug.Log("[TiendaCheck] Objeto: " + gameObject.name +
                  " | Clave: " + clave +
                  " | comprado: " + comprado +
                  " | ocultarSiNoComprado: " + ocultarSiNoComprado +
                  " | forzarOcultarEnPruebas: " + forzarOcultarEnPruebas);

        if (forzarOcultarEnPruebas)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ocultarSiNoComprado)
        {
            gameObject.SetActive(comprado);
        }
    }
}