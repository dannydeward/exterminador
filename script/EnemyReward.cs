using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [Header("Recompensa del Insecto")]
    [Tooltip("Valor de experiencia que da este enemigo (0.5, 1 o 2)")]
    public float puntosExperiencia = 0.5f;

    [Tooltip("El prefab de la joya que soltará al morir")]
    public GameObject joyaPrefab;
}