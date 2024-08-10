using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class LogicManager : MonoBehaviour
{   
    #region declaring variables
    public GameObject upgradeCards;
    public TowerCardScript towerCardScript;
    private bool isWaitingForRoundStart = false;

    public int numPathPointsRemoved;
    public int pathEndChance;
    public int branchChance;
    public GameObject enemySpawner;

    public int numPointsForInter;
    public int numPoints;
    public Tilemap map;
    public TileBase floor, pathTile, red, green, taken;

    public static List<Queue<Vector2>> listOfPaths;
    public static List<Vector2> listOfBottomLeftPoints;

    private PlayerInput PlayerControls;
    private InputAction shoot;

    private Vector2 pathStart;
    private List<int> temp = new List<int> { 0, 1, 2, 3 };

    private int waveNumber = 1;
    #endregion
    
    private void OnEnable()
    {
        PlayerControls = new PlayerInput();
        shoot = PlayerControls.Testing.N;
        shoot.Enable();
        shoot.performed += Fire;
    }
    
    private void OnDisable()
    {
        if (shoot != null)
        {
            shoot.Disable();
        }
    }
    
    private void Fire(InputAction.CallbackContext context)
    {
        NextRound();
    }

    // Start is called before the first frame update
    void Start()
    {
        listOfPaths = new List<Queue<Vector2>>();
        listOfBottomLeftPoints = new List<Vector2>();
        DrawBackground((GlobalVariables.squareWidth) + 1, (GlobalVariables.squareHeight) + 1, new Vector2(-(GlobalVariables.squareWidth / 2), -(GlobalVariables.squareHeight / 2)));
        listOfPaths.Add(PathManager.InitialRun(new Vector2(0, 0), numPoints, map, pathTile, red, green, transform.rotation, enemySpawner));
        listOfBottomLeftPoints.Add(new Vector2(-(GlobalVariables.squareWidth / 2), -(GlobalVariables.squareHeight / 2)));
        towerCardScript = upgradeCards.GetComponentInChildren<TowerCardScript>();
    }

    public void Update()
    {
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        bool allFinished = true;
        foreach (GameObject spawner in enemySpawners)
        {
            EnemySpawner script = spawner.GetComponent<EnemySpawner>();
            if (script.totalSpawnCount != script.maxSpawns)
            {
                allFinished = false;
            }
        }

        // if (allFinished && enemies.Length == 0)
        // {
        //     StartCoroutine(DelayedNextRound());
        // }
        if (allFinished && enemies.Length == 0 && !isWaitingForRoundStart)
        {
            StartCoroutine(DelayedNextRound());
        }
    }

    private IEnumerator DelayedNextRound()
    {
        isWaitingForRoundStart = true;
        yield return new WaitForSeconds(5f); 
        // upgradeCards.SetActive(true);
        // towerCardScript.NewCards();
        NextRound();
        isWaitingForRoundStart = false;
    }

    private void NextRound()
    {
        // EnemySpawner.speed *= 10f;
        // EnemySpawner.health *= 1f;
        waveNumber++;
        WaveManager.Instance.AddWave();
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");

        foreach (GameObject spawner in enemySpawners)
        {
            EnemySpawner script = spawner.GetComponent<EnemySpawner>();
            foreach (EnemySpawner.EnemyType enemyType in script.enemyTypes)
            {
                enemyType.speed *= 1.1f;
                enemyType.health *= 1.5f;
            }
            Destroy(spawner);
        }

        List<int> listOfIsToSkip = new List<int>();
        for (int i = 0; i < listOfPaths.Count; i++)
        {
            if (!listOfIsToSkip.Contains(i))
            {
                Vector2 pathEnd = listOfPaths[i].Peek();
                Vector2 direction = ReturnDirection(pathEnd);
            
                if (direction != Vector2.zero)
                {
                    Vector2 bottomLeftPoint = new Vector2(listOfBottomLeftPoints[i].x + (GlobalVariables.squareWidth * direction.x) + direction.x, listOfBottomLeftPoints[i].y + (GlobalVariables.squareHeight * direction.y) + direction.y);

                    listOfBottomLeftPoints[i] = bottomLeftPoint;

                    DrawBackground(GlobalVariables.squareWidth + 1, GlobalVariables.squareHeight + 1, bottomLeftPoint);

                    List<int> listOfX = new List<int>();
                    List<int> listOfY = new List<int>();
                    temp = new List<int> { 0, 1, 2, 3 };

                    int branch = Random.Range(0, branchChance);

                    if (branch == 0)
                    {
                        // // Queue<Vector2> tempQ = new Queue<Vector2>(listOfPaths[i]);
                        // int num = numPointsForInter;
                        // int rand = Random.Range(1, num);
                        // num -= rand;
                        // listOfPaths[i] = CreatePath(bottomLeftPoint, direction, listOfX, listOfY, pathEnd, listOfPaths[i], num);
                        // 
                        // PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[i]);
                        // 
                        // // dequeue random num of points
                        // Vector2 topRightCorner = new Vector2(bottomLeftPoint.x + GlobalVariables.squareWidth + 1, bottomLeftPoint.y + GlobalVariables.squareHeight + 1);
                        // Queue<Vector2> Q = RandomPathEnd(topRightCorner, listOfPaths[i]);
                        // pathEnd = Q.Peek();
                        // 
                        // Queue<Vector2> temporaryQueue = CreatePath(bottomLeftPoint, direction, listOfX, listOfY, pathEnd, Q, rand);
                        // 
                        // listOfPaths.Add(temporaryQueue);
                        // listOfBottomLeftPoints.Add(bottomLeftPoint);
                        // listOfIsToSkip.Add(listOfPaths.Count - 1);
                        // PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[listOfPaths.Count - 1]);

                        int num = numPointsForInter;
                        int rand = Random.Range(1, num);
                        num -= rand;
                        // listOfPaths[i] = CreatePath(bottomLeftPoint, direction, listOfX, listOfY, pathEnd, listOfPaths[i], num);

                        Vector2 randomPoint = GetRandomPointOnSquareEdge(GlobalVariables.squareWidth, GlobalVariables.squareHeight, direction);

                        pathStart = new Vector2(bottomLeftPoint.x + randomPoint.x, bottomLeftPoint.y + randomPoint.y);

                        listOfX.Add((int)pathStart.x);
                        listOfY.Add((int)pathStart.y);
                        listOfX.Add((int)pathEnd.x);
                        listOfY.Add((int)pathEnd.y);

                        Queue<Vector2> path = new Queue<Vector2>();

                        //path.Enqueue(pathStart);

                        path = PathManager.CreatePoints(listOfX, listOfY, path, pathEnd, pathStart, num, bottomLeftPoint);

                        // path.Enqueue(pathEnd);

                        // Get a random point for the other branch path
                        int randPointOnPath = Random.Range(1, num + 1);
                        Queue<Vector2> tempQ = new Queue<Vector2>(path);
                        for (int x = 0; x < randPointOnPath; x++)
                        {
                            tempQ.Dequeue();
                        }

                        Vector2 pathEndForBranch = tempQ.Peek();

                        Queue<Vector2> fullPath = PathManager.CreatePath(pathStart, pathEnd, map, path, pathTile, red, green);

                        listOfPaths[i] = EnqueueWholeQueue(listOfPaths[i], fullPath);

                        tempQ = new Queue<Vector2>(listOfPaths[i]);
                        Vector2 temp;
                        do
                        {
                            temp = tempQ.Dequeue();
                        } while (temp != pathEndForBranch);

                        // tempQ.Enqueue(pathEndForBranch);
                        
                        PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[i]);

                        Queue<Vector2> temporaryQueue = CreatePath(bottomLeftPoint, direction, listOfX, listOfY, pathEndForBranch, tempQ, rand);

                        listOfPaths.Add(temporaryQueue);
                        listOfBottomLeftPoints.Add(bottomLeftPoint);
                        listOfIsToSkip.Add(listOfPaths.Count - 1);
                        PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[listOfPaths.Count - 1]);
                    }
                    else
                    {
                        listOfPaths[i] = CreatePath(bottomLeftPoint, direction, listOfX, listOfY, pathEnd, listOfPaths[i], numPointsForInter);
                        PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[i]);
                    }
                } else
                {
                    pathStart = listOfPaths[i].Peek();
                    PathManager.CreateEnemySpawner(enemySpawner, map, pathStart, transform.rotation, listOfPaths[i]);
                }
            }
        }
    }

    private Queue<Vector2> RandomPathEnd(Vector2 topRightCorner, Queue<Vector2> Q)
    {
        Queue<Vector2> tempQ = new Queue<Vector2>(Q);
        Vector2 point;
        int count = 0;
        do
        {
            point = tempQ.Dequeue();
            count++;
        } while ((point.x < topRightCorner.x) && (point.y < topRightCorner.y) && count != numPathPointsRemoved);

        int randInt;
        do
        {
            randInt = Random.Range(0, pathEndChance);
            point = tempQ.Dequeue();
        } while ((point.x < topRightCorner.x) && (point.y < topRightCorner.y) && randInt != 0);

        return tempQ;
    }

    private Queue<Vector2> CreatePath(Vector2 bottomLeftPoint, Vector2 direction, List<int> listOfX, List<int> listOfY, Vector2 pathEnd, Queue<Vector2> Q, int num)
    {
        Vector2 randomPoint = GetRandomPointOnSquareEdge(GlobalVariables.squareWidth, GlobalVariables.squareHeight, direction);

        pathStart = new Vector2(bottomLeftPoint.x + randomPoint.x, bottomLeftPoint.y + randomPoint.y);

        listOfX.Add((int)pathStart.x);
        listOfY.Add((int)pathStart.y);
        listOfX.Add((int)pathEnd.x);
        listOfY.Add((int)pathEnd.y);

        Queue<Vector2> path = new Queue<Vector2>();

        path.Enqueue(pathStart);

        path = PathManager.CreatePoints(listOfX, listOfY, path, pathEnd, pathStart, num, bottomLeftPoint);

        // path = PathManager.OrderQueue(pathStart, pathEnd, path);

        path.Enqueue(pathEnd);
        Queue<Vector2> fullPath = PathManager.CreatePath(pathStart, pathEnd, map, path, pathTile, red, green);

        fullPath = EnqueueWholeQueue(Q, fullPath);

        return fullPath;
    }

    private Queue<Vector2> EnqueueWholeQueue(Queue<Vector2> DequeuingQ, Queue<Vector2> QueueingQ)
    {
        Queue<Vector2> tempQ = QueueingQ;
        while (DequeuingQ.Count > 0)
        {
            tempQ.Enqueue(DequeuingQ.Dequeue());
        }

        return tempQ;
    }

    Vector2 GetRandomPointOnSquareEdge(int width, int height, Vector2 direction)
    {
        List<int> edges = RemoveEdge(direction);

        if (edges.Count == 0)
        {
            return Vector2.zero;
        }

        // Randomly select an edge from the remaining edges
        int randomEdge = edges[Random.Range(0, edges.Count)];

        int x = 0, y = 0;

        switch (randomEdge)
        {
            case 0: // Bottom edge
                x = Random.Range(1, width);
                y = 0;
                temp.Remove(0);
                break;
            case 1: // Right edge
                x = width;
                y = Random.Range(1, height);
                temp.Remove(1);
                break;
            case 2: // Top edge
                x = Random.Range(1, width);
                y = height;
                temp.Remove(2);
                break;
            case 3: // Left edge
                x = 0;
                y = Random.Range(1, height);
                temp.Remove(3);
                break;
        }

        return new Vector2(x, y);
    }

    private List<int> RemoveEdge(Vector2 direction)
    {
        // edge (0: bottom, 1: right, 2: top, 3: left)

        if (direction.x == 1)
        {
            temp.Remove(3);
        } else if (direction.x == -1)
        {
            temp.Remove(1);
        } else if (direction.y == 1)
        {
            temp.Remove(0);
        } else
        {
            temp.Remove(2);
        }

        return temp;
    }

    private Vector2 ReturnDirection(Vector2 point)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (map.GetTile(new Vector3Int((int)point.x + x, (int)point.y + y, 0)) == null && x != y && x != -y)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    public void DrawBackground(int width, int height, Vector2 startCoord)
    {
        for (int y = (int) startCoord.y; y < (height + startCoord.y); y++)
        {
            for (int x = (int) startCoord.x; x < (width + startCoord.x); x++)
            {
                map.SetTile(new Vector3Int(x, y, 0), floor);
            }
        }
    }
}
