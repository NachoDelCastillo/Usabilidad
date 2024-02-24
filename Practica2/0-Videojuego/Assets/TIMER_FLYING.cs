using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMER_FLYING : MonoBehaviour
{

    bool finish = false;

    [SerializeField]
    Transform lastPosition;

    // 
    public void FlyToFinishLine()
    {
        //transform.parent = null;

        finish = true;
    }

    private void Update()
    {
        if (finish)
        {
            transform.DOMove(lastPosition.position, 4);
        }
    }
}
