using UnityEngine;

public class Joya : MonoBehaviour
{
    [Header("Valor de Experiencia")]
    [Tooltip("Puntos que suma esta joya al ser recolectada (0.5, 1 o 2)")]
    public float puntosExperiencia = 1f;

    // Este método se activa cuando algo entra en contacto con la joya
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Verificamos que quien toca la joya sea Mateo (debe tener el Tag "Player")
        if (other.CompareTag("Player"))
        {
            // 2. Sumamos los puntos al GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarPuntos(puntosExperiencia);
            }
            
            // 3. Destruimos la joya para que desaparezca del suelo
            Destroy(gameObject);
        }
    }
}