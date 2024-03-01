using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    Transform imgContainer;

    List<SpriteRenderer> images = new List<SpriteRenderer>();

    private void Awake()
    {
        for (int i = 0; i < imgContainer.childCount; i++)
        {
            SpriteRenderer img = imgContainer.GetChild(i).GetComponent<SpriteRenderer>();

            if (img != null)
                images.Add(img);

            img.DOFade(0, .1f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishMovement fish = collision.GetComponent<FishMovement>();
        if (fish != null)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].DOFade(1, 2);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FishMovement fish = collision.GetComponent<FishMovement>();
        if (fish != null)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].DOFade(0, 2);
            }
        }
    }
}
