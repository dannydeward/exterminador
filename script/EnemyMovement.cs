using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 2f;
    
    // Ahora es privado. Solo se asigna desde fuera mediante SetTarget()
    private Transform objetivoMateo;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // ⚠️ IMPORTANTE: Ya NO buscamos a Mateo aquí.
        // Esperamos a que el EnemyManager nos lo pase con SetTarget().
        
        // Por seguridad, si por alguna razón no se asignó el objetivo,
        // podemos hacer un fallback buscando por Tag (solo una vez, como respaldo)
        if (objetivoMateo == null)
        {
            GameObject playerByTag = GameObject.FindGameObjectWithTag("Player");
            if (playerByTag != null)
            {
                objetivoMateo = playerByTag.transform;
            }
            else
            {
                Debug.LogWarning("EnemyMovement: No se encontró a Mateo. El enemigo se quedará quieto.");
            }
        }
    }

    // 🎯 NUEVO MÉTODO: El EnemyManager llamará a este método al nacer el enemigo
    public void SetTarget(Transform target)
    {
        objetivoMateo = target;
    }

    void Update()
    {
        if (objetivoMateo != null)
        {
            // 1. Moverse suavemente hacia Mateo
            transform.position = Vector2.MoveTowards(transform.position, objetivoMateo.position, moveSpeed * Time.deltaTime);

            // 2. Rotar al insecto para que mire a Mateo
            Vector2 direccion = (objetivoMateo.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 90));
        }
    }
}