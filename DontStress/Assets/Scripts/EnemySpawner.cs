using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Queue<Vector2> path;

    private float timer = 0;
    public float spawnRate;

    public GameObject enemyTroupe;

    public Tilemap map;

    // Update is called once per frame
    void FixedUpdate()
    {
        spawn();
    }

    void spawn()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject insatnce = Instantiate(enemyTroupe, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            EnemyScript script = insatnce.GetComponent<EnemyScript>();
            // Vector2 temp = path.Peek();
            // script.objective = map.CellToWorld(new Vector3Int((int) temp.x, (int) temp.y, 0));
            Queue<Vector2> pathClone = new Queue<Vector2>(path);
            script.path = pathClone;
            script.map = map;
            timer = 0;
        }
    }
}
