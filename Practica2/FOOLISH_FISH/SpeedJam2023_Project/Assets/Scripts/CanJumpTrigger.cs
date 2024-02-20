using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanJumpTrigger : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera tutorialCamera;

    [SerializeField]
    CinemachineVirtualCamera gameplayCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishMovement fish = collision.GetComponent<FishMovement>();
        if (fish != null)
        {
            fish.CanJumpNow();

            tutorialCamera.gameObject.SetActive(false);
            gameplayCamera.gameObject.SetActive(true);
        }
    }
}
