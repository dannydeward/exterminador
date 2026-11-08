using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Enemigos por Nivel (SOLO los NUEVOS de cada nivel)")]
    public GameObject[] enemigosNivel1; 
    public GameObject[] enemigosNivel2; 
    public GameObject[] enemigosNivel3; 
    public GameObject[] enemigosNivel4; 
    public GameObject[] enemigosNivel5; 

    [Header("Configuración de Oleadas y Bordes")]
    // 👇 CAMBIO: 1.5 a 2 segundos para acción constante sin lag
    public float tiempoEntreOleadas = 1.8f;
    
    // 👇 CAMBIO: 15 enemigos base, suficiente para causar caos controlado
    public int cantidadPorOleada = 15;
    
    public float alturaSuelo = -1f;
    public float margenFueraDePantalla = 5f; // 👇 Aumentado a 5 para que entren caminando

    private Camera camaraPrincipal;
    private float timer;

    void Start()
    {
        camaraPrincipal = Camera.main;
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= tiempoEntreOleadas)
        {
            GenerarOleadaDesdeBordes();
            timer = 0f;
        }
    }

    void GenerarOleadaDesdeBordes()
    {
        if (camaraPrincipal == null) return;

        // 1. Obtener el nivel actual
        int nivelActual = 1;
        if (GameManager.Instance != null)
        {
            nivelActual = GameManager.Instance.nivelActual;
        }

        // 2. Crear la lista de enemigos permitidos
        List<GameObject> poolDeSpawneo = new List<GameObject>();
        if (nivelActual >= 1 && enemigosNivel1.Length > 0) poolDeSpawneo.AddRange(enemigosNivel1);
        if (nivelActual >= 2 && enemigosNivel2.Length > 0) poolDeSpawneo.AddRange(enemigosNivel2);
        if (nivelActual >= 3 && enemigosNivel3.Length > 0) poolDeSpawneo.AddRange(enemigosNivel3);
        if (nivelActual >= 4 && enemigosNivel4.Length > 0) poolDeSpawneo.AddRange(enemigosNivel4);
        if (nivelActual >= 5 && enemigosNivel5.Length > 0) poolDeSpawneo.AddRange(enemigosNivel5);

        if (poolDeSpawneo.Count == 0) return;

        // 3. Calcular los límites de la cámara
        float distanciaPantallaZ = Mathf.Abs(camaraPrincipal.transform.position.z);
        Vector3 bordeIzquierdo = camaraPrincipal.ViewportToWorldPoint(new Vector3(0, 0, distanciaPantallaZ));
        Vector3 bordeDerecho = camaraPrincipal.ViewportToWorldPoint(new Vector3(1, 0, distanciaPantallaZ));
        Vector3 bordeSuperior = camaraPrincipal.ViewportToWorldPoint(new Vector3(0, 1, distanciaPantallaZ));
        Vector3 bordeInferior = camaraPrincipal.ViewportToWorldPoint(new Vector3(0.5f, 0, distanciaPantallaZ));

        // 4. Definir cantidad de enemigos
        int cantidadReal = (nivelActual == 1) ? Random.Range(5, 9) : cantidadPorOleada + (nivelActual * 3);

        // 5. Generar enemigos desde diferentes direcciones
        for (int i = 0; i < cantidadReal; i++)
        {
            int index = Random.Range(0, poolDeSpawneo.Count);
            GameObject enemigoPrefab = poolDeSpawneo[index];

            // 👇 NUEVA DISTRIBUCIÓN: 50% Este, 50% Diagonales (Nada de Oeste)
            int direccion = CalcularDireccionAleatoria();
            
            Vector3 posicionSpawn = CalcularPosicionSpawn(direccion, bordeIzquierdo, bordeDerecho, bordeSuperior, bordeInferior);

            float variacionX = Random.Range(-1f, 1f);
            float variacionY = Random.Range(-0.5f, 0.5f);
            Vector3 posicionFinal = new Vector3(posicionSpawn.x + variacionX, posicionSpawn.y + variacionY, 0f);

            Instantiate(enemigoPrefab, posicionFinal, Quaternion.identity);
        }
    }

    // 👇 MÉTODO NUEVO: Distribución de direcciones (50% Este, resto diagonales)
    int CalcularDireccionAleatoria()
    {
        // 0-4 (50%): Este (Derecha)
        // 5: Noroeste
        // 6: Noreste
        // 7: Suroeste
        // 8: Sureste
        int azar = Random.Range(0, 9);
        
        if (azar < 5) return 1; // Este (50% de probabilidad)
        else if (azar == 5) return 4; // Noroeste
        else if (azar == 6) return 5; // Noreste
        else if (azar == 7) return 6; // Suroeste
        else return 7; // Sureste
    }

    // Calcula la posición de spawn según la dirección
    Vector3 CalcularPosicionSpawn(int direccion, Vector3 bordeIzq, Vector3 bordeDer, Vector3 bordeSup, Vector3 bordeInf)
    {
        switch (direccion)
        {
            case 1: // Este (Derecha) - 50% de los enemigos
                return new Vector3(bordeDer.x + margenFueraDePantalla, Random.Range(bordeInf.y, bordeSup.y), 0f);
            
            case 4: // Noroeste (Arriba-Izquierda)
                return new Vector3(bordeIzq.x - margenFueraDePantalla, bordeSup.y + margenFueraDePantalla, 0f);
            
            case 5: // Noreste (Arriba-Derecha)
                return new Vector3(bordeDer.x + margenFueraDePantalla, bordeSup.y + margenFueraDePantalla, 0f);
            
            case 6: // Suroeste (Abajo-Izquierda)
                return new Vector3(bordeIzq.x - margenFueraDePantalla, bordeInf.y - margenFueraDePantalla, 0f);
            
            case 7: // Sureste (Abajo-Derecha)
                return new Vector3(bordeDer.x + margenFueraDePantalla, bordeInf.y - margenFueraDePantalla, 0f);
            
            default:
                // Por defecto, Este
                return new Vector3(bordeDer.x + margenFueraDePantalla, Random.Range(bordeInf.y, bordeSup.y), 0f);
        }
    }
}