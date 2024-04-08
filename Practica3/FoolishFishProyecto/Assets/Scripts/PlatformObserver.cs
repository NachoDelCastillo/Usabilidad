using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObserver : MonoBehaviour
{
    // Singleton
    public static PlatformObserver Instance { get; private set; }

    // Variables
    float[] platformPositions;
    Transform fishMovement;

    private void Awake()
    {
        Instance = this;

        // Obtener una referencia del pescado
        fishMovement = FindObjectOfType<FishMovement>().transform;

        // Obtener todos los hijos del GameObject actual
        Transform[] allChildObj = GetComponentsInChildren<Transform>();

        // Inicializar posiciones
        platformPositions = new float[allChildObj.Length-1];
            
        // Saltarse el primer transform ya que es el propio objeto, no sus hijos
        for (int i = 1; i < allChildObj.Length; i++)
            // Obtener la posición del hijo y almacenarla en el array
            platformPositions[i-1] = allChildObj[i].position.y;
    }

    public int GetCurrentFishPlatform()
    {
        float currentY = fishMovement.position.y;

        // Recorrer todas las plataformas para averiguar en cual esta el pescado ahora mismo
        for (int i = 0; i < platformPositions.Length; i++)
        {
            float platformY = platformPositions[i];
            if (currentY < platformY)
                return i + 1;
        }

        // Devolver -1 si la posicion del pescado es invalida
        return -1;
    }
}
