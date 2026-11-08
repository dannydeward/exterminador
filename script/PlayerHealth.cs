using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("UI (Interfaz)")]
    public RectTransform barraDeVida;
    
    // 👇 Ponlo en 0, el script medirá el tamaño real automáticamente
    public float anchoOriginalBarra = 0f;  

    [Header("Efecto de Daño")]
    public float duracionEfectoSegundos = 0.05f;
    public Color colorDano = Color.black;

    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    private bool esInvulnerable = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }

        // 👇 TRUCO: Forzamos al script a medir el ancho REAL de la barra de vida
        if (barraDeVida != null)
        {
            anchoOriginalBarra = barraDeVida.sizeDelta.x;
            Debug.Log($"[PlayerHealth] Ancho REAL de la barra de vida detectado: {anchoOriginalBarra}");
        }

        ActualizarBarraUI();
    }

    public void RecibirDano(int cantidad)
    {
        if (esInvulnerable) return;

        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        Debug.Log($"¡Mateo recibió {cantidad} de daño! Vida restante: {vidaActual}");

        ActualizarBarraUI();
        StartCoroutine(EfectoDanoFlash());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void ActualizarBarraUI()
    {
        if (barraDeVida != null)
        {
            float porcentaje = (float)vidaActual / vidaMaxima;
            
            Vector2 nuevoTamano = barraDeVida.sizeDelta;
            nuevoTamano.x = anchoOriginalBarra * porcentaje;
            barraDeVida.sizeDelta = nuevoTamano;

            Debug.Log($"[UI Vida] Vida: {vidaActual}/{vidaMaxima} ({porcentaje * 100}%) -> Ancho barra: {nuevoTamano.x}");
        }
    }

    IEnumerator EfectoDanoFlash()
    {
        esInvulnerable = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorDano;
            yield return new WaitForSeconds(duracionEfectoSegundos);
            spriteRenderer.color = colorOriginal;
        }
        esInvulnerable = false;
    }

    void Morir()
    {
        Debug.Log("<color=red>--- ¡MATEO HA SIDO DERROTADO! ---</color>");
        Time.timeScale = 0f;
    }

    public bool EstaVivo()
    {
        return vidaActual > 0;
    }

     // 👇 Agrega este método nuevo
    public void CurarAlMaximo()
    {
        vidaActual = vidaMaxima;
        ActualizarBarraUI();
        Debug.Log("❤️ ¡Vida restaurada al máximo por cambio de nivel!");
    }
}