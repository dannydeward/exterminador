using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Arrastra a Mateo aquí
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private bool inicializado = false;

    void Start()
    {
        // Coloca la cámara instantáneamente sobre Mateo al arrancar para evitar el paneo automático
        if (target != null)
        {
            Vector3 startPos = target.position + offset;
            startPos.z = -10f;
            transform.position = startPos;
            inicializado = true;
        }
    }

    void LateUpdate()
    {
        if (target != null && inicializado)
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z = -10f; // Forzar siempre el plano Z de la cámara

            // Movimiento suave solo cuando el jugador se mueva
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}