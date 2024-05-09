using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{


    public void MoveBar(float time)
    {

        transform.DOScale(1, time);
    }

    public void StopMovingBar()
    {
        transform.DOKill();
    }

    public void InstantiateJumpButton()
    {

    }
}
