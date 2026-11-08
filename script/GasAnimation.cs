using UnityEngine;
using System.Collections;

public class GasAnimation : MonoBehaviour
{
    [Header("Sprites de la Animación")]
    public Sprite[] frames;  // 👈 Arrastra AQUÍ todos tus sprites en orden
    
    [Header("Configuración")]
    public float framesPorSegundo = 15f;
    
    private SpriteRenderer spriteRenderer;
    private bool estaAnimando = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Ocultar al inicio
        }
    }

    // Método público para activar la animación
    public void ReproducirAnimacion()
    {
        if (frames.Length == 0 || estaAnimando) return;
        
        StartCoroutine(AnimarGas());
    }

    IEnumerator AnimarGas()
    {
        estaAnimando = true;
        
        // Mostrar el sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        
        // Reproducir cada frame en orden
        for (int i = 0; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(1f / framesPorSegundo);
        }
        
        // Ocultar al terminar
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        estaAnimando = false;
    }
}