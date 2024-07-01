using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PathManager : MonoBehaviour
{

    // public GameObject EnemySpawner;
    // 
    // public int numPoints;
    // // public int rangeX, rangeY;
    // private Vector2 pathStart;
    // private Vector2 pathEnd = Vector2.zero;
    // 
    // public Tilemap map;
    // public TileBase pathTile, red, green;
    // 
    // private Queue<Vector2> path;

    // private PlayerInput PlayerControls;
    // private InputAction shoot;
    // 
    // private void OnEnable()
    // {
    //     PlayerControls = new PlayerInput();
    //     shoot = PlayerControls.Player.Attack;
    //     shoot.Enable();
    //     shoot.performed += Fire;
    // }
    // 
    // private void OnDisable()
    // {
    //    shoot.Disable();
    // }
    // 
    // private void Fire(InputAction.CallbackContext context)
    // {
    //     map.ClearAllTiles();
    //     Run();
    // }

    public static Queue<Vector2> InitialRun(Vector2 pathEnd, int numPoints, Tilemap map, TileBase pathTile, TileBase red, TileBase green, Quaternion rotation, GameObject EnemySpawner)
    {
        // Initialize the path queue
        Queue<Vector2> path = new Queue<Vector2>();
        int posX = Random.Range(0, 2) * 2 - 1;
        int posY = Random.Range(0, 2) * 2 - 1;

        List<int> listOfX = new List<int>();
        List<int> listOfY = new List<int>();

        int tempX = (GlobalVariables.squareWidth / 2) * posX;
        int tempY = Random.Range(0, (GlobalVariables.squareHeight / 2)) * posY;

        listOfX.Add(tempX);
        listOfY.Add(tempY);

        Vector2 pathStart = new Vector2(tempX, tempY);
        path.Enqueue(pathStart);
        int x, y;

        for (int i = 0; i < numPoints; i++)
        {
            do
            {
                x = Random.Range(2, (GlobalVariables.squareWidth / 2)) * posX;
            } while (listOfX.Contains(x));
            listOfX.Add(x);

            do
            {
                y = Random.Range(-((GlobalVariables.squareHeight / 2) - 2), (GlobalVariables.squareHeight / 2) - 2);
            } while (listOfY.Contains(y));
            listOfY.Add(y);

            path.Enqueue(new Vector2(x, y));
        }

        path = OrderQueue(pathStart, pathEnd, path);

        path.Enqueue(pathEnd);
        Queue<Vector2> fullPath = CreatePath(pathStart, pathEnd, map, path, pathTile, red, green);

        CreateEnemySpawner(EnemySpawner, map, pathStart, rotation, fullPath);

        return fullPath;
    }

    public static Queue<Vector2> CreatePoints(List<int> listOfX, List<int> listOfY, Queue<Vector2> path, Vector2 pathEnd, Vector2 pathStart, int numPoints, Vector2 offset)
    {
        int x, y;

        for (int i = 0; i < numPoints; i++)
        {
            do
            {
                // this spawns them in the range 2-19
                x = Random.Range(2, GlobalVariables.squareWidth - 1);
                x += (int) offset.x;
            } while (listOfX.Contains(x));
            listOfX.Add(x);

            do
            {
                y = Random.Range(2, GlobalVariables.squareHeight - 1);
                y += (int) offset.y;
            } while (listOfY.Contains(y));
            listOfY.Add(y);

            path.Enqueue(new Vector2(x, y));
        }

        path = OrderQueue(pathStart, pathEnd, path);

        path.Enqueue(pathEnd);
        return path;
    }


    //   public static void CreateEnemySpawner(GameObject EnemySpawner, Tilemap map, Vector2 pathStart, Quaternion rotation, Queue<Vector2> fullPath)
    // {
    //     GameObject instance = Instantiate(EnemySpawner, map.GetCellCenterWorld(new Vector3Int((int)pathStart.x, (int)pathStart.y, 0)), rotation);
    //     EnemySpawner enemySpawnerScript = instance.GetComponent<EnemySpawner>();
    //     Queue<Vector2> pathClone = new Queue<Vector2>(fullPath);
    //     enemySpawnerScript.path = pathClone;
    //     enemySpawnerScript.map = map;
    
    //     enemySpawnerScript.maxSpawns = 10;
    // }

    public static void CreateEnemySpawner(GameObject EnemySpawnerPrefab, Tilemap map, Vector2 pathStart, Quaternion rotation, Queue<Vector2> fullPath)
    {
        GameObject instance = Instantiate(EnemySpawnerPrefab, map.GetCellCenterWorld(new Vector3Int((int)pathStart.x, (int)pathStart.y, 0)), rotation);
        EnemySpawner enemySpawnerScript = instance.GetComponent<EnemySpawner>();

        // Create a new SpawnPoint
        EnemySpawner.SpawnPoint newSpawnPoint = new EnemySpawner.SpawnPoint();
        newSpawnPoint.point = instance.transform;
        newSpawnPoint.path = new Queue<Vector2>(fullPath);

        // Add the new SpawnPoint to the spawner's list
        if (enemySpawnerScript.spawnPoints == null)
        {
            enemySpawnerScript.spawnPoints = new List<EnemySpawner.SpawnPoint>();
        }
        enemySpawnerScript.spawnPoints.Add(newSpawnPoint);

        enemySpawnerScript.map = map;
        enemySpawnerScript.maxSpawns = 10;  // You might want to make this configurable
    }

    public static Queue<Vector2> OrderQueue(Vector2 start, Vector2 end, Queue<Vector2> path)
    {
        List<Vector2> points = new List<Vector2>(path);
        Queue<Vector2> orderedQueue = new Queue<Vector2>();

        Vector2 currentPoint = start;
        orderedQueue.Enqueue(currentPoint);

        while (points.Count > 0)
        {
            Vector2 closestPoint = points[0];
            float shortestDistance = Mathf.Abs(Vector2.Distance(currentPoint, closestPoint));

            foreach (var point in points)
            {
                float distance = Mathf.Abs(Vector2.Distance(currentPoint, point));
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = point;
                }
            }

            orderedQueue.Enqueue(closestPoint);
            points.Remove(closestPoint);
            currentPoint = closestPoint;
        }

        return orderedQueue;
    }

    public static Queue<Vector2> CreatePath(Vector2 start, Vector2 end, Tilemap map, Queue<Vector2> pathPoints, TileBase pathTile, TileBase red, TileBase green)
    {
        Queue<Vector2> fullPath = new Queue<Vector2>();

        Vector2 firstPoint = pathPoints.Dequeue();
        Vector2 secondPoint;

        while (pathPoints.Count != 0)
        {
            // fullPath.Enqueue(firstPoint);

            secondPoint = pathPoints.Dequeue();
            int startX = (int)firstPoint.x;
            int endX = (int)secondPoint.x;
            int startY = (int)firstPoint.y;
            int endY = (int)secondPoint.y;

            Queue<Vector2> tempQX = new Queue<Vector2>();

            // Create path along X-axis from startX to endX
            for (int x = Mathf.Min(startX, endX); x <= Mathf.Max(startX, endX); x++)
            {
                map.SetTile(new Vector3Int(x, startY, 0), pathTile);
                tempQX.Enqueue(new Vector2(x, startY));
            }

            if (Mathf.Min(startX, endX) == endX)
            {
                tempQX = ReverseQueue(tempQX);
            }
            fullPath = MergeQueueToPath(tempQX, fullPath);

            Queue<Vector2> tempQY = new Queue<Vector2>();
            int count = 0;

            // Create path along Y-axis from startY to endY
            for (int y = Mathf.Min(startY, endY); y <= Mathf.Max(startY, endY); y++)
            {
                if (count != 0)
                {
                    map.SetTile(new Vector3Int(endX, y, 0), pathTile);
                    tempQY.Enqueue(new Vector2(endX, y));
                }
                count++;
            }

            if (Mathf.Min(startY, endY) == endY)
            {
                tempQY = ReverseQueue(tempQY);
            }
            fullPath = MergeQueueToPath(tempQY, fullPath);

            map.SetTile(new Vector3Int((int) firstPoint.x, (int) firstPoint.y, 0), red);
            map.SetTile(new Vector3Int((int) secondPoint.x, (int) secondPoint.y, 0), red);

            // Update the firstPoint to be the secondPoint for the next iteration
            firstPoint = secondPoint;
        }
        map.SetTile(new Vector3Int((int)start.x, (int)start.y, 0), green);
        map.SetTile(new Vector3Int((int)end.x, (int)end.y, 0), green);

        return fullPath;
    }

    private static Queue<Vector2> ReverseQueue(Queue<Vector2> q)
    {
        Stack<Vector2> s = new Stack<Vector2>();

        while (q.Count > 0)
        {
            s.Push(q.Dequeue());
        }

        while (s.Count > 0)
        {
            q.Enqueue(s.Pop());
        }

        return q;
    }

    private static Queue<Vector2> MergeQueueToPath(Queue<Vector2> tempQ, Queue<Vector2> fullPath)
    {
        while (tempQ.Count > 0)
        {
            fullPath.Enqueue(tempQ.Dequeue());
        }
        return fullPath;
    }
}
