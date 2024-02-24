using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    Vector3 lookTarget;

    [SerializeField]
    bool lookPlayer;

    FishMovement fish;

    private void Awake()
    {
        Invoke("ChangeTarget", 1);

        fish = FindObjectOfType<FishMovement>();
    }

    void ChangeTarget()
    {
        int num = 200;
        float x = Random.Range(-num, num);
        float y = Random.Range(-num, num);
        float z = Random.Range(-num, num);

        lookTarget = fish.transform.position + new Vector3(0, 0, -10) + new Vector3(x, y, z);

        Invoke("ChangeTarget", Random.Range(.2f, 1f));
    }

    private void Update()
    {
        if (lookPlayer)
            transform.LookAt(fish.transform.position);
        else
        {
            Quaternion currentRotation = transform.rotation;
            transform.LookAt(lookTarget);
            Quaternion targetRotation = transform.rotation;

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 3);
        }
    }
}
