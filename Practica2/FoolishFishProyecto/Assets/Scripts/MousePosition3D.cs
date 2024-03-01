using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] FishMovement player;

    SpriteRenderer[] spriteRenderer = new SpriteRenderer[3];

    Vector2 lastPos;

    private void Start()
    {
        spriteRenderer[0] = GetComponent<SpriteRenderer>();
        spriteRenderer[1] = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer[2] = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
            transform.position = raycastHit.point;

        if (lastPos != new Vector2(transform.position.x, transform.position.z)) {
          
            player.DestroyArrow();
            lastPos = new Vector2(transform.position.x, transform.position.z); 
        }
    }

    public void ShowX()
    {
        foreach (var sprite in spriteRenderer)
            sprite.enabled = true;
    }

    public void HideX()
    {
        foreach (var sprite in spriteRenderer)
            sprite.enabled = false;
    }

}