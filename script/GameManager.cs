using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progresión de Niveles")]
    public int nivelActual = 1;
    public float puntuacionActual = 0f; 
    public float[] puntosParaPasarNivel = { 100f, 300f, 600f, 1200f }; 

    [Header("Sistema de Dinero")]
    public int dineroActual = 0;
    
    // Variables internas para controlar las propinas
    private float xpAcumuladaEnNivel = 0f;
    private float siguienteMetaPropina = 20f; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SumarPuntos(float puntos)
    {
        puntuacionActual += puntos;
        xpAcumuladaEnNivel += puntos;

        // 👇 Lógica de la Propina: Cada 20 XP acumulados en el nivel actual
        if (xpAcumuladaEnNivel >= siguienteMetaPropina)
        {
            dineroActual += 10;
            siguienteMetaPropina += 20f; // La próxima meta será a los 40, luego 60, etc.
            
            Debug.Log("💰 ¡Propina recibida! +$10");
            if (UINotificaciones.Instance != null)
            {
                UINotificaciones.Instance.MostrarMensaje("¡Has recibido $10 de propina!", 2f);
            }
        }
        
        VerificarProgresoDeNivel();
    }

    void VerificarProgresoDeNivel()
    {
        if (nivelActual >= 5) return;

        if (puntuacionActual >= puntosParaPasarNivel[nivelActual - 1])
        {
            SubirDeNivel();
        }
    }

    void SubirDeNivel()
    {
        nivelActual++;
        
        // 👇 Lógica de la Paga: Nivel 2 = $200, Nivel 3 = $400, Nivel 4 = $800, etc.
        // Fórmula: 100 * (2 elevado a la potencia de (nivelActual - 1))
        int paga = 100 * Mathf.RoundToInt(Mathf.Pow(2, nivelActual - 1));
        dineroActual += paga;

        // 👇 Restituir la vida de Mateo al subir de nivel
        PlayerHealth jugador = FindObjectOfType<PlayerHealth>();
        if (jugador != null)
        {
            jugador.CurarAlMaximo();
        }

        // Reiniciar contadores de propina para el nuevo nivel
        xpAcumuladaEnNivel = 0f;
        siguienteMetaPropina = 20f;

        Debug.Log($"🎉 ¡NIVEL {nivelActual}! Paga recibida: ${paga}");

        // 👇 Mostrar aviso de nivel y mercado
        if (UINotificaciones.Instance != null)
        {
            string mensaje = $"¡NIVEL {nivelActual}!\n\nAhora puedes usar tu paga\npara comprar más armamento.";
            UINotificaciones.Instance.MostrarMensaje(mensaje, 4f);
        }

        if (nivelActual == 5) IniciarNivelFinal();
    }

    void IniciarNivelFinal()
    {
        Debug.Log("🚨 NIVEL 5: ¡Rumbo al Parque!");
    }
}