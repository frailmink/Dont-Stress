using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour
{
    public Queue<Vector2> path;

    public Vector2 objective;

    private Rigidbody2D rb;

    public Tilemap map;

    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure the initial objective is set at the start
        if (path != null && path.Count > 0)
        {
            Vector2 temp = path.Dequeue();
            objective = map.CellToWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, objective) > 0.1f)
        {
            Vector2 dif = objective - (Vector2)transform.position;
            rb.velocity = dif.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the enemy's movement

            if (path != null && path.Count > 0)
            {
                Vector2 temp = path.Dequeue();
                objective = map.CellToWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
            }
        }
    }
}
