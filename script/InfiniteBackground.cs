using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Configuración de Bucle")]
    public Transform player;          // Arrastra aquí a Mateo
    public float largoCalle = 20f;    // El ancho exacto de tu sprite de calle en unidades de Unity
    private float offsetThreshold = 25f; // Distancia antes de reciclar

    void Update()
    {
        if (player != null)
        {
            // Si el jugador avanza hacia la derecha y se aleja del centro de la calle actual
            if (player.position.x - transform.position.x > offsetThreshold)
            {
                // Mover el bloque del escenario hacia adelante para que la calle no se acabe
                transform.position = new Vector3(transform.position.x + largoCalle * 2f, transform.position.y, transform.position.z);
            }
        }
    }
}