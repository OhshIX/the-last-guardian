using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI puntos;
	public GameObject[] vidas;
    
	void Update () {
		if (puntos != null && GameManager.Instance != null)
		{
			puntos.text = GameManager.Instance.PuntosTotales.ToString();
		}
	}

	public void ActualizarPuntos(int puntosTotales)
	{
		if (puntos != null)
		{
			puntos.text = puntosTotales.ToString();
		}
	} 

	public void DesactivarVida(int indice) {
		if (vidas != null && indice >= 0 && indice < vidas.Length && vidas[indice] != null)
		{
			vidas[indice].SetActive(false);
		}
	}

	public void ActivarVida(int indice) {
		if (vidas != null && indice >= 0 && indice < vidas.Length && vidas[indice] != null)
		{
			vidas[indice].SetActive(true);
		}
	}
}
