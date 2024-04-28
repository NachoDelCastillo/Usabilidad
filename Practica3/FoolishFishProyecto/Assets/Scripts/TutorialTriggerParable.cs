using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialTriggerParable : MonoBehaviour
{
    [SerializeField] FishMovement player;
    [SerializeField]
    GameObject arrowPrefab;
    GameObject arrow;

    [SerializeField]
    float x_;
    [SerializeField]
    float y_;
    [SerializeField]
    float z_;

    bool arrow_performed;

    LineRenderer lr;
    SpriteRenderer image;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponentInChildren<LineRenderer>();
        arrow_performed = false;
        rb = player.GetComponent<Rigidbody2D>();
        image = GetComponentInChildren<SpriteRenderer>();
        if (image != null)
            image.enabled = false;
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        FishMovement fish = collision.gameObject.GetComponent<FishMovement>();
        if (fish != null && !fish.IsOnAir())
        {
            if (image != null)
                image.enabled = true;
            Vector3 mousePos = new Vector3(x_, y_, z_);
            Vector2 velocity = (Vector2)Calculador.calcular(transform, mousePos);

            lr.enabled = true;

            Vector2[] trajectory = Plot(rb, (Vector2)transform.position, velocity, 1000);

            List<Vector3> pos = new List<Vector3>();

            for (int i = 0; i < trajectory.Length; i++)
            {
                if (!GameplayManager.Instance.IsUnderWater(trajectory[i].y)) break;
                else
                    pos.Add(trajectory[i]);
            }
            int tam = pos.Count - 1;
            if (tam > 0)
            {
                lr.positionCount = tam;
                Vector3[] positions = new Vector3[tam];

                for (int i = 0; i < tam; i++)
                    positions[i] = pos[i];

                lr.SetPositions(positions);

                if (!arrow_performed)
                {
                    arrow = Instantiate<GameObject>(arrowPrefab, pos[tam], Quaternion.identity);
                    arrow_performed = true;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FishMovement>() != null)
        {
            lr.enabled = false;
            arrow_performed = false;
            if (image != null)
                image.enabled = false;
            if (arrow != null)
                Destroy(arrow.gameObject);
        }
    }

    Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = -Physics2D.gravity * rigidbody.mass * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }
}
