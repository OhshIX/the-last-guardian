using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI puntos;
	public GameObject[] vidas;
    
	private void Start()
	{
		// 🌟 AUTO-REGISTRO: Al iniciar la escena, le avisa al GameManager cuál es el HUD activo
		if (GameManager.Instance != null)
		{
			GameManager.Instance.hud = this;
			
			// Forzamos la actualización visual inicial de puntos
			GameManager.Instance.SumarPuntos(0);
		}
	}

	// Método público para refrescar todos los corazones según las vidas actuales
	public void ActualizarCorazones(int vidasActuales)
	{
		if (vidas != null)
		{
			for (int i = 0; i < vidas.Length; i++)
			{
				if (vidas[i] != null)
				{
					if (i < vidasActuales)
						vidas[i].SetActive(true);  // Enciende el corazón si corresponde
					else
						vidas[i].SetActive(false); // Apaga el corazón si excede las vidas
				}
			}
		}
	}

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