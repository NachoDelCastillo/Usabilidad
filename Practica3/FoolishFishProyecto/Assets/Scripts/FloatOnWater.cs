using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOnWater : MonoBehaviour
{

    Rigidbody2D rb;

    float ogGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ogGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameplayManager.Instance.IsUnderWater(transform.position.y))
            rb.gravityScale = -ogGravity;
        else
            rb.gravityScale = ogGravity;
    }
}
