using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddJumpButtons : MonoBehaviour
{
    double maxPos = 395;

    public GameObject buttonPrefab;

    public void InstantiateButton(float percentage)
    {
        Debug.Log(percentage);

        GameObject instance = Instantiate(buttonPrefab, this.gameObject.transform);
        instance.transform.localPosition = new Vector3(2.5f+(float)(maxPos * percentage), 
            instance.transform.localPosition.y, instance.transform.localPosition.z);
    }
}
