// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WaveManager : MonoBehaviour
// {

//     public int initialEnemyCount = 10;
//     public float timeBetweenWaves = 10f;

//     public List<Transform> spawners = new List<Transform>();

//     private void Start()
//     {
//         StartCoroutine(SpawnWaves());
//     }

//     private IEnumerator SpawnWaves()
//     {
//         while (true)
//         {
//             int enemiesToSpawn = initialEnemyCount; // 10 enemies per wave
//             Debug.Log($"Spawning {enemiesToSpawn} enemies");

//             for (int i = 0; i < enemiesToSpawn; i++)
//             {
//                 foreach (Transform spawner in spawners)
//                 {
//                     EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
//                     if (enemySpawner != null)
//                     {
//                         enemySpawner.spawn(); // Trigger spawn from EnemySpawner
//                     }
//                     else
//                     {
//                         Debug.LogWarning("EnemySpawner component not found on spawner.");
//                     }
//                 }
//                 yield return new WaitForSeconds(0.5f); // spawn rate
//             }

//             yield return new WaitForSeconds(timeBetweenWaves); // 10 seconds between waves
//         }
//     }
// }
