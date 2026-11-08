using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GasAttack : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject gasHitboxObject;
    public Transform firePoint;
    
    // 👇 NUEVO: Referencia al script de movimiento del jugador
    public PlayerMovement playerMovement;

    [Header("Configuración del Gas")]
    public float activeDuration = 0.4f;
    private bool isAttacking = false;

    void Start()
    {
        if (gasHitboxObject != null)
        {
            gasHitboxObject.SetActive(false);
        }

        // 👇 Si no se asignó en el inspector, lo buscamos automáticamente
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // 👇 Solo leemos la tecla de espacio, NO el movimiento
            if (keyboard.spaceKey.wasPressedThisFrame && !isAttacking)
            {
                StartCoroutine(ActivarGas());
            }
        }
    }

    IEnumerator ActivarGas()
    {
        isAttacking = true;

        if (gasHitboxObject != null && firePoint != null && playerMovement != null)
        {
            // 1. Mover el GasHitbox al FirePoint
            gasHitboxObject.transform.position = firePoint.position;

            // 👇 NUEVO: Leemos la dirección directamente del PlayerMovement
            Vector2 direccion = playerMovement.UltimaDireccion;

            // 👇 NUEVO: Calculamos el ángulo con Atan2 (funciona con diagonales también)
            float anguloZ = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            gasHitboxObject.transform.rotation = Quaternion.Euler(0, 0, anguloZ);

            // 3. Encender el gas
            gasHitboxObject.SetActive(true);
        }

        yield return new WaitForSeconds(activeDuration);

        if (gasHitboxObject != null)
        {
            gasHitboxObject.SetActive(false);
        }
        isAttacking = false;
    }
}