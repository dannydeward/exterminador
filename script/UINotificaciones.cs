using UnityEngine;
using TMPro;
using System.Collections;

public class UINotificaciones : MonoBehaviour
{
    public static UINotificaciones Instance;

    [Header("Contador de Dinero")]
    public TextMeshProUGUI textoDinero; // Arrastra aquí el texto de dinero

    [Header("Panel de Notificaciones")]
    public GameObject panelNotificacion; // El objeto que contiene el texto (debe empezar desactivado)
    public TextMeshProUGUI textoNotificacion; // El texto dentro del panel

    private Coroutine rutinaNotificacion;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        ActualizarDineroUI();
    }

    // Método para actualizar el texto del dinero
    public void ActualizarDineroUI()
    {
        if (textoDinero != null && GameManager.Instance != null)
        {
            textoDinero.text = "$" + GameManager.Instance.dineroActual;
        }
    }

    // Método para mostrar mensajes temporales
    public void MostrarMensaje(string mensaje, float duracion)
    {
        // Si ya hay un mensaje mostrándose, lo cancelamos para no encimarlos
        if (rutinaNotificacion != null)
        {
            StopCoroutine(rutinaNotificacion);
        }

        textoNotificacion.text = mensaje;
        panelNotificacion.SetActive(true);
        
        // Aseguramos que el dinero esté actualizado al mostrar el aviso
        ActualizarDineroUI();

        rutinaNotificacion = StartCoroutine(OcultarMensaje(duracion));
    }

    IEnumerator OcultarMensaje(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        panelNotificacion.SetActive(false);
    }
}