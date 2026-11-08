using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    [Header("Referencias de UI")]
    public RectTransform barraXP;
    public TextMeshProUGUI textoNivel;
    
    // Ya no importa el valor que pongas aquí, el script lo medirá solo
    public float anchoOriginalBarra = 0f; 
    
    [Header("Colores (Opcional)")]
    public Color colorNivel1 = Color.blue;
    public Color colorNivel2 = Color.cyan;
    public Color colorNivel3 = Color.yellow;
    public Color colorNivel4 = Color.orange;
    public Color colorNivel5 = Color.red;

    private Image imagenBarra;

    void Start()
    {
        if (barraXP != null)
        {
            imagenBarra = barraXP.GetComponent<Image>();
            
            // 👇 TRUCO: Forzamos al script a medir el ancho REAL del objeto en Unity
            anchoOriginalBarra = barraXP.sizeDelta.x;
            Debug.Log($"[ExperienceBar] Ancho REAL detectado: {anchoOriginalBarra}");
        }
        
        ActualizarBarraXP();
    }

    void Update()
    {
        ActualizarBarraXP();
    }

    void ActualizarBarraXP()
    {
        if (GameManager.Instance == null || barraXP == null) return;

        int nivelActual = GameManager.Instance.nivelActual;
        float puntuacionActual = GameManager.Instance.puntuacionActual;
        float[] puntosParaPasarNivel = GameManager.Instance.puntosParaPasarNivel;

        if (textoNivel != null)
        {
            textoNivel.text = $"Nivel {nivelActual}";
            
            if (imagenBarra != null)
            {
                switch (nivelActual)
                {
                    case 1: imagenBarra.color = colorNivel1; break;
                    case 2: imagenBarra.color = colorNivel2; break;
                    case 3: imagenBarra.color = colorNivel3; break;
                    case 4: imagenBarra.color = colorNivel4; break;
                    case 5: imagenBarra.color = colorNivel5; break;
                }
            }
        }

        float porcentaje = 0f;

        if (nivelActual >= 5)
        {
            porcentaje = 1f;
        }
        else
        {
            float puntosNivelActual = (nivelActual == 1) ? 0 : puntosParaPasarNivel[nivelActual - 2];
            float puntosSiguienteNivel = puntosParaPasarNivel[nivelActual - 1];
            float rangoNivel = puntosSiguienteNivel - puntosNivelActual;
            float progresoActual = puntuacionActual - puntosNivelActual;
            
            porcentaje = Mathf.Clamp01(progresoActual / rangoNivel);
        }

        // Aplicar el porcentaje al ancho REAL que medimos al inicio
        Vector2 nuevoTamano = barraXP.sizeDelta;
        nuevoTamano.x = anchoOriginalBarra * porcentaje;
        barraXP.sizeDelta = nuevoTamano;
    }
}