using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

static class Calculador {
    public static Vector3 calcular(Transform playerTransform, Vector3 targetCursor)
    {

        Vector3 velocity = Vector3.zero;

        Vector3 posDiff = playerTransform.position - targetCursor;

        if (posDiff.y < 0 ) { return velocity; }

        velocity.y = -Mathf.Sqrt(posDiff.y * 2 * -Physics2D.gravity.y);

        float timeToReachApex = velocity.y / -Physics2D.gravity.y;

        velocity.x = posDiff.x / timeToReachApex;

        if (velocity.x > 50)
        { velocity.x = 50; }
        else if (velocity.x < -50)
        { velocity.x = -50; }

        //Debug.Log("Velocidad de salto" + velocity);
        //Debug.Log("Posicion mouse" + targetCursor);

        // Aplica la velocidad al proyectil (puede ser un objeto, bala, etc.)
        // por ejemplo: rigidbody.velocity = velocity;
        return velocity;
    }
}
