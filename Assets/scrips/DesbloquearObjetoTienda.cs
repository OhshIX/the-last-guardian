using UnityEngine;

public class DesbloquearObjetoTienda : MonoBehaviour
{
    [Header("Debe ser el mismo ID que pusiste en BotonCompra")]
    public string idCompra = "item_1";

    [Header("Si está marcado, solo aparece si ya se compró")]
    public bool ocultarSiNoComprado = true;

    void Awake()
    {
        bool comprado = PlayerPrefs.GetInt("Comprado_" + idCompra, 0) == 1;

        if (ocultarSiNoComprado)
        {
            gameObject.SetActive(comprado);
        }
    }
}