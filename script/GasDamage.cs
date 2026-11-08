using UnityEngine;

public class GasDamage : MonoBehaviour
{
    [Header("Área del Gas")]
    public Vector2 boxSize = new Vector2(1.5f, 1.5f); 

    private void OnEnable()
    {
        DestruirPlagasEnArea();
    }

    void DestruirPlagasEnArea()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("plaga"))
            {
                // Buscamos el script de recompensa en el insecto
                EnemyReward reward = hit.GetComponent<EnemyReward>();
                
                // Si tiene el script y tiene una joya asignada en el Prefab...
                if (reward != null && reward.joyaPrefab != null)
                {
                    // ¡AQUÍ ESTÁ LA MAGIA! Creamos la joya en el suelo
                    Instantiate(reward.joyaPrefab, hit.transform.position, Quaternion.identity);
                }

                // Destruimos el insecto
                Destroy(hit.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}