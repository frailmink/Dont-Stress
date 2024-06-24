using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    #region declaring variables
    public Queue<Vector2> path;

    private float timer = 0;
    public float spawnRate; //how quick it spawns 

    public int maxSpawns;
    private int spawnCount; //number of enemies spawned per spawnpoint
    public static float health = 31;
    public static float speed = 2;

    public GameObject enemyTroupe;

    public Tilemap map;
    #endregion

    void FixedUpdate()
    {
        if (spawnCount < maxSpawns)
        {
            spawn();
        }
    }

    public void spawn()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject insatnce = Instantiate(enemyTroupe, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            EnemyScript enemyScript = insatnce.GetComponent<EnemyScript>();
            // Vector2 temp = path.Peek();
            // script.objective = map.CellToWorld(new Vector3Int((int) temp.x, (int) temp.y, 0));
            Queue<Vector2> pathClone = new Queue<Vector2>(path);
            enemyScript.path = pathClone;
            enemyScript.map = map;
            enemyScript.maxHealth = health;
            enemyScript.speed = speed;
            timer = 0;
            spawnCount++;
        }
    }
}