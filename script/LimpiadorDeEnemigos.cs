using UnityEngine;

public class LimpiadorDeEnemigos : MonoBehaviour
{
    [Header("Configuración de limpieza")]
    [Tooltip("Metros extra fuera de la cámara antes de destruir al enemigo")]
    public float margenSeguridad = 5f; 

    private Camera camaraPrincipal;
    private Vector3 limiteSuperior;
    private Vector3 limiteInferior;
    private Vector3 limiteIzquierdo;
    private Vector3 limiteDerecho;

    void Start()
    {
        camaraPrincipal = Camera.main;
        ActualizarLimites();
    }

    void Update()
    {
        // Actualizamos los límites por si la cámara se mueve siguiendo al jugador
        ActualizarLimites();

        // Si el enemigo se sale de los límites + el margen de seguridad, se destruye
        if (transform.position.x > limiteDerecho.x + margenSeguridad ||
            transform.position.x < limiteIzquierdo.x - margenSeguridad ||
            transform.position.y > limiteSuperior.y + margenSeguridad ||
            transform.position.y < limiteInferior.y - margenSeguridad)
        {
            Destroy(gameObject);
        }
    }

    void ActualizarLimites()
    {
        if (camaraPrincipal == null) return;
        
        float distanciaZ = Mathf.Abs(camaraPrincipal.transform.position.z);
        limiteIzquierdo = camaraPrincipal.ViewportToWorldPoint(new Vector3(0, 0, distanciaZ));
        limiteDerecho = camaraPrincipal.ViewportToWorldPoint(new Vector3(1, 0, distanciaZ));
        limiteSuperior = camaraPrincipal.ViewportToWorldPoint(new Vector3(0, 1, distanciaZ));
        limiteInferior = camaraPrincipal.ViewportToWorldPoint(new Vector3(0, 0, distanciaZ));
    }
}