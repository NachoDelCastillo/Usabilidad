using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformIdSelector : MonoBehaviour
{

    float[] platformPositions;

    private void Awake()
    {
        // Obtener todos los hijos del GameObject actual
        Transform[] allChildObj = GetComponentsInChildren<Transform>();

        platformPositions = new float[allChildObj.Length * 3];


        for (int i = 0; i < allChildObj.Length; i++)
        {
            // Obtener la posición del hijo y almacenarla en el array
            platformPositions[i] = allChildObj[i].position.y;
        }


    }

    public int getCurrentFishPlatform()
    {
        return -1;
    }
}
