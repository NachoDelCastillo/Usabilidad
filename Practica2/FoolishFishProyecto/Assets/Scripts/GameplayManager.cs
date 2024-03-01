using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameplayManager : MonoBehaviour
{
    // Singleton
    static public GameplayManager Instance { get; private set; }
    private void Awake()
    { Instance = this; }


    [SerializeField]
    Transform waterObj;
    [SerializeField]
    Transform waterLevelsContainer;
    float[] waterLevels;
    float waterLevel;
    float startingWaterLevel;

    float gfxWaterLevel;

    private void Start()
    {
        waterLevel = waterObj.transform.position.y;
        startingWaterLevel = waterLevel;

        int childCount = waterLevelsContainer.childCount;
        waterLevels = new float[childCount];
        for (int i = 0; i < childCount; i++)
            waterLevels[i] = waterLevelsContainer.GetChild(i).transform.position.y;
    }

    private void Update()
    {
        gfxWaterLevel = Mathf.Lerp(gfxWaterLevel, waterLevel, Time.deltaTime * 10);

        waterObj.transform.position = new Vector2(0, gfxWaterLevel);
    }

    public float GetWaterLevel()
    { return waterLevel; }

    public bool IsUnderWater(float Ypos)
    { return Ypos < waterLevel; }

    public float FirstWaterLevelPosY()
    { return waterLevels[0]; }

    public void ResetWaterLevel()
    {
        SetWaterLevel(startingWaterLevel);
    }


    public void RecalculateWaterLevel(float fishPosY)
    {
        if (waterLevels == null || waterLevels.Length == 0)
            return;

        for (int i = 0; i < waterLevels.Length; i++)
        {
            // Encontrar el primer nivel de agua que pase al pez
            float thisWaterLevel = waterLevels[i];
            if (thisWaterLevel > fishPosY)
            {
                SetWaterLevel(thisWaterLevel);
                break;
            }
        }
    }

    public void SetWaterLevel(float newWaterLevel)
    {
        if (waterLevel == newWaterLevel)
            return;

        waterLevel = newWaterLevel;

        AudioManager_PK.GetInstance().Play("Ola", UnityEngine.Random.Range(0.9f, 1.1f));

        if (newWaterLevel > 20)
            gfxWaterLevel = waterLevel - 20;
    }
}
