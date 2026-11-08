using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject proyectilPrefab;  // 👈 Arrastra aquí el Prefab de la Bala de Humo
    public Transform puntoDeDisparo;    // 👈 (Opcional) Desde dónde sale. Si lo dejas vacío, sale del centro de Mateo.
    public float cadenciaDeDisparo = 0.4f; // Segundos de espera entre cada disparo
    
    private float timerDisparo;

    void Update()
    {
        timerDisparo += Time.deltaTime;

        // Disparar con Click Izquierdo del Mouse o Barra Espaciadora
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            if (timerDisparo >= cadenciaDeDisparo)
            {
                Disparar();
                timerDisparo = 0f;
            }
        }
    }

    void Disparar()
    {
        if (proyectilPrefab == null)
        {
            Debug.LogWarning("¡No has asignado el Prefab de la bala en el Inspector!");
            return;
        }

        // 1. Definir de dónde sale la bala
        Vector3 posicionSalida = puntoDeDisparo != null ? puntoDeDisparo.position : transform.position;

        // 2. Calcular la dirección hacia el mouse
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0; // Asegurar que esté en el plano 2D del juego
        
        Vector2 direccionDisparo = (posicionMouse - posicionSalida).normalized;

        // 3. Crear la bala en el juego
        GameObject nuevaBala = Instantiate(proyectilPrefab, posicionSalida, Quaternion.identity);
        
        // 4. Decirle a la bala hacia dónde debe ir
        ProyectilHumo scriptBala = nuevaBala.GetComponent<ProyectilHumo>();
        if (scriptBala != null)
        {
            scriptBala.Inicializar(direccionDisparo);
        }
    }
}