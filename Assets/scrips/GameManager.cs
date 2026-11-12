using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// ==========================================
// 🎮 CONTROLADOR CENTRAL: GAME MANAGER
// ==========================================
public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public HUD hud;

	public int PuntosTotales { get; private set; }

	private int vidas = 3;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			// ⚓ Nos suscribimos al evento para detectar de forma interna el cambio de nivel
			SceneManager.sceneLoaded += AlCargarEscena;
		}
		else
		{
			Debug.Log("Cuidado! Mas de un GameManager en escena.");
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		// Sincronización para cuando arranca el Nivel 1
		CoordinarHUD();
	}

	private void OnDestroy()
	{
		// 🔌 Desuscripción obligatoria para evitar fugas de memoria al cerrar el juego
		if (Instance == this)
		{
			SceneManager.sceneLoaded -= AlCargarEscena;
		}
	}

	// 🔄 Se activa automáticamente al pasar al Nivel 2, Nivel 3 o reiniciar
	private void AlCargarEscena(Scene escena, LoadSceneMode modo)
	{
		// 🌟 REINICIO POR NIVEL: Las vidas regresan a 3 y monedas a 0 en cada nuevo mapa
		vidas = 3;
		PuntosTotales = 0;

		CoordinarHUD();
	}

	// 🎛️ Conecta los componentes de la interfaz de cada nivel de manera dinámica
	private void CoordinarHUD()
	{
		hud = FindObjectOfType<HUD>();

		if (hud != null)
		{
			// Envía los puntos actuales (0) a la nueva pantalla
			hud.ActualizarPuntos(PuntosTotales);
			
			// Dibuja el estado exacto de los corazones (3 completos)
			for (int i = 0; i < 3; i++)
			{
				if (i < vidas)
					hud.ActivarVida(i);
				else
					hud.DesactivarVida(i);
			}
		}
	}

	public void SumarPuntos(int puntosASumar)
	{
		PuntosTotales += puntosASumar;
		if (hud != null)
		{
			hud.ActualizarPuntos(PuntosTotales);
		}
	}

	public void PerderVida()
	{
		// 🛡️ FILTRO FANTASMA: Si el jugador es inmune, ignoramos el daño por completo
		caminar jugador = FindObjectOfType<caminar>();
		if (jugador != null && jugador.esInmune)
		{
			return; 
		}

		// 1. Apagamos el corazón en el HUD usando el orden correcto que tu HUD requiere
		if (hud != null)
		{
			hud.DesactivarVida(vidas - 1);
		}

		// 2. Restamos la vida internamente (Disminuye exactamente 1 corazón)
		vidas -= 1;

		// 3. Evaluamos si llegó a cero para iniciar la secuencia de derrota
		if (vidas <= 0)
		{
			if (jugador != null)
			{
				StartCoroutine(SecuenciaMuerteJugador(jugador.gameObject));
			}
			else
			{
				CargarPantallaPerdiste();
			}
		}
	}

	public bool RecuperarVida()
	{
		if (vidas == 3)
		{
			return false;
		}

		if (hud != null)
		{
			hud.ActivarVida(vidas);
		}
		
		vidas += 1;
		return true;
	}

	public void LineaMortal()
	{
		caminar jugador = FindObjectOfType<caminar>();
		if (jugador != null && jugador.esInmune)
		{
			return; 
		}
		
		if (jugador != null)
		{
			StartCoroutine(SecuenciaMuerteJugador(jugador.gameObject));
		}
		else
		{
			CargarPantallaPerdiste();
		}
	}

	// ⏳ CORRUTINA: Muestra la animación "MORIR", espera a que termine y carga la escena de Game Over
	private IEnumerator SecuenciaMuerteJugador(GameObject objetoJugador)
	{
		// 1. Detener por completo el script de movimiento para que no camine estando muerto
		caminar scriptCam = objetoJugador.GetComponent<caminar>();
		if (scriptCam != null) scriptCam.enabled = false;

		// 2. Ejecutar el Trigger "MORIR" en el Animator
		Animator anim = objetoJugador.GetComponent<Animator>();
		if (anim != null)
		{
			anim.SetTrigger("MORIR"); 
		}

		// 3. Desactivar las físicas para que el cuerpo ignore colisiones o caiga al vacío estorbando
		Rigidbody2D rb = objetoJugador.GetComponent<Rigidbody2D>();
		if (rb != null) rb.simulated = false; 

		// 4. ESPERA: Ajusta este número (1.5f) según la duración en segundos de tu animación de muerte
		yield return new WaitForSeconds(1.5f);

		// 5. Destruir el objeto del jugador de la escena antes de irnos
		Destroy(objetoJugador);

		// 6. Cargar la pantalla de Game Over
		CargarPantallaPerdiste();
	}

	private void CargarPantallaPerdiste()
	{
		vidas = 3; 
		PuntosTotales = 0;
		SceneManager.LoadScene("PERDISTE");
	}

	public void CambioDeNivel()
	{
		int escenaActual = SceneManager.GetActiveScene().buildIndex;
		int siguienteEscena = escenaActual + 1;
		SceneManager.LoadScene(siguienteEscena);
	}

	public void Ganaste()
	{
		SceneManager.LoadScene(5);
	}
}

// ==========================================
// ⚔️ DETECTOR DE CHOQUES (Para tus enemigos)
// ==========================================
public class ContactoEnemigo : MonoBehaviour
{
	// Detecta si el enemigo choca físicamente con el jugador con Rigidbody2D
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.PerderVida();
			}
		}
	}

	// Detecta el choque si el enemigo usa la opción "Is Trigger" activada
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.PerderVida();
			}
		}
	}
}

