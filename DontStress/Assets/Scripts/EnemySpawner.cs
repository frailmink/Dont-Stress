using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnChance;
        public float health;
        public float speed;
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public Queue<Vector2> path;
        [HideInInspector]
        public float timer = 0;
        [HideInInspector]
        public int spawnCount = 0;
    }

    public List<SpawnPoint> spawnPoints;
    public List<EnemyType> enemyTypes;
    public float spawnRate;
    public int maxSpawns;
    public int totalSpawnCount { get; private set; }

    public Tilemap map;

    void FixedUpdate()
    {
        if (totalSpawnCount < maxSpawns)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.timer < spawnRate)
            {
                sp.timer += Time.deltaTime;
            }
            else if (totalSpawnCount < maxSpawns)
            {
                EnemyType selectedEnemy = ChooseEnemyType();
                GameObject instance = Instantiate(selectedEnemy.prefab, sp.point.position, sp.point.rotation);
                EnemyScript enemyScript = instance.GetComponent<EnemyScript>();

                Queue<Vector2> pathClone = new Queue<Vector2>(sp.path);
                enemyScript.path = pathClone;
                enemyScript.map = map;
                enemyScript.maxHealth = selectedEnemy.health;
                enemyScript.speed = selectedEnemy.speed;

                sp.timer = 0;
                sp.spawnCount++;
                totalSpawnCount++;
            }
        }
    }

    private EnemyType ChooseEnemyType()
    {
        float totalChance = 0;
        foreach (EnemyType enemy in enemyTypes)
        {
            totalChance += enemy.spawnChance;
        }

        float randomPoint = Random.value * totalChance;

        for (int i = 0; i < enemyTypes.Count; i++)
        {
            if (randomPoint < enemyTypes[i].spawnChance)
            {
                return enemyTypes[i];
            }
            randomPoint -= enemyTypes[i].spawnChance;
        }

        return enemyTypes[enemyTypes.Count - 1];
    }

       public int GetSpawnCount(int index)
    {
        if (index >= 0 && index < spawnPoints.Count)
        {
            return spawnPoints[index].spawnCount;
        }
        return 0;
    }
}
    // public void SpawnEnemies()
    // {
    //     foreach (SpawnPoint sp in spawnPoints)
    //     {
    //         if (sp.timer < spawnRate)
    //         {
    //             sp.timer += Time.deltaTime;
    //         }
    //         else if (totalSpawnCount < maxSpawns)
    //         {
    //             GameObject instance = Instantiate(enemyTroupe, sp.point.position, sp.point.rotation);
    //             EnemyScript enemyScript = instance.GetComponent<EnemyScript>();

    //             Queue<Vector2> pathClone = new Queue<Vector2>(sp.path);
    //             enemyScript.path = pathClone;
    //             enemyScript.map = map;
    //             enemyScript.maxHealth = health;
    //             enemyScript.speed = speed;

    //             sp.timer = 0;
    //             totalSpawnCount++;
    //         }
    //     }
    // }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class EnemySpawner : MonoBehaviour
// {
//     #region declaring variables
//     public Queue<Vector2> path;

//     private float timer = 0;
//     public float spawnRate; //how quick it spawns 

//     public int maxSpawns;
//     public int spawnCount; //number of enemies spawned per spawnpoint
//     public static float health = 31;
//     public static float speed = 2;

//     public GameObject enemyTroupe;

//     public Tilemap map;
//     #endregion

//     void FixedUpdate()
//     {
//         if (spawnCount < maxSpawns)
//         {
//             spawn();
//         }
//     }

//     public void spawn()
//     {
//         if (timer < spawnRate)
//         {
//             timer += Time.deltaTime;
//         }
//         else
//         {
//             GameObject insatnce = Instantiate(enemyTroupe, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
//             EnemyScript enemyScript = insatnce.GetComponent<EnemyScript>();
//             // Vector2 temp = path.Peek();
//             // script.objective = map.CellToWorld(new Vector3Int((int) temp.x, (int) temp.y, 0));
//             Queue<Vector2> pathClone = new Queue<Vector2>(path);
//             enemyScript.path = pathClone;
//             enemyScript.map = map;
//             enemyScript.maxHealth = health;
//             enemyScript.speed = speed;
//             timer = 0;
//             spawnCount++;
//         }
//     }
// }