using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public int dano = 10;
    
    // 👇 NUEVO: Flag para saber si el enemigo está siendo destruido por el gas
    private bool siendoDestruidoPorGas = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 👇 Si el gas ya lo está destruyendo, NO hacer daño
        if (siendoDestruidoPorGas) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RecibirDano(dano);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 👇 Si el gas ya lo está destruyendo, NO hacer daño
        if (siendoDestruidoPorGas) return;
        
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RecibirDano(dano);
            }
        }
    }

    // 👇 NUEVO: Método público para que GasDamage pueda marcar al enemigo
    public void MarcarComoDestruidoPorGas()
    {
        siendoDestruidoPorGas = true;
    }
}