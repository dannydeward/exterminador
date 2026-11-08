using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxLayer : MonoBehaviour
{
    [Header("Configuración de Velocidad (Parallax)")]
    [Tooltip("0 = Estático (fondo lejano como el cielo). 1 = Se mueve exactamente con la cámara.")]
    [Range(0f, 1f)] public float factorParallaxX = 0.5f;
    [Range(0f, 1f)] public float factorParallaxY = 0.2f;

    private Transform camaraTransform;
    private Vector3 ultimaPosicionCamara;
    private float longitudSpriteX;

    void Start()
    {
        camaraTransform = Camera.main.transform;
        ultimaPosicionCamara = camaraTransform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Obtenemos el ancho exacto del sprite en unidades del mundo
        longitudSpriteX = spriteRenderer.bounds.size.x;
    }

    void LateUpdate()
    {
        if (camaraTransform == null) return;

        // 1. Calcular cuánto se movió la cámara desde el último fotograma
        Vector3 deltaMovimiento = camaraTransform.position - ultimaPosicionCamara;

        // 2. Mover la capa aplicando el factor de parallax
        transform.position += new Vector3(deltaMovimiento.x * factorParallaxX, deltaMovimiento.y * factorParallaxY, 0f);

        ultimaPosicionCamara = camaraTransform.position;

        // 3. Reposicionamiento suave (Loop infinito sin tirones)
        // Si la cámara se aleja de la capa más allá de su longitud, la adelantamos/atrasamos en bucle
        float distanciaCamaraX = camaraTransform.position.x - transform.position.x;
        
        if (Mathf.Abs(distanciaCamaraX) >= longitudSpriteX)
        {
            float offsetDireccion = (distanciaCamaraX > 0) ? longitudSpriteX : -longitudSpriteX;
            transform.position = new Vector3(transform.position.x + (offsetDireccion * 2f), transform.position.y, transform.position.z);
        }
    }
}