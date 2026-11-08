using UnityEngine;

public class ProyectilHumo : MonoBehaviour
{
    [Header("Configuración de la Bala")]
    public float velocidad = 15f;       // Qué tan rápido viaja el humo
    public float tiempoVida = 3f;       // Se destruye sola si no golpea nada en 3 segundos

    private Vector2 direccion;

    // Este método lo llamamos desde el jugador para decirle hacia dónde ir
    public void Inicializar(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
        Destroy(gameObject, tiempoVida); // Temporizador de seguridad
    }

    void Update()
    {
        // Mover la bala en la dirección asignada
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si la bala toca una plaga
        if (other.CompareTag("plaga"))
        {
            // 1. Soltar la joya (la misma lógica que usaba el gas)
            EnemyReward reward = other.GetComponent<EnemyReward>();
            if (reward != null && reward.joyaPrefab != null)
            {
                Instantiate(reward.joyaPrefab, other.transform.position, Quaternion.identity);
            }

            // 2. Destruir al enemigo
            Destroy(other.gameObject);
            
            // 3. Destruir la bala de humo
            Destroy(gameObject);
        }
    }
}