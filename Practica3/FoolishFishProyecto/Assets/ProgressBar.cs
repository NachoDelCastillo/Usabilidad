using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    public void MoveBar(double time)
    {

        transform.DOScale(1, (float)time);
    }

    public void StopMovingBar()
    {
        transform.DOKill();
    }
}
