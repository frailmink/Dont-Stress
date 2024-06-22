using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class LogicManager : MonoBehaviour
{
    public GameObject EnemySpawner;

    public int numPointsForInter;
    public int numPoints;
    public Tilemap map;
    public TileBase floor, pathTile, red, green;

    public static List<Queue<Vector2>> listOfPaths;
    public static List<Vector2> listOfBottomLeftPoints;

    private PlayerInput PlayerControls;
    private InputAction shoot;
    
    private void OnEnable()
    {
        PlayerControls = new PlayerInput();
        shoot = PlayerControls.Player.Attack;
        shoot.Enable();
        shoot.performed += Fire;
    }
    
    private void OnDisable()
    {
       shoot.Disable();
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
        listOfPaths.Add(PathManager.InitialRun(new Vector2(0, 0), numPoints, map, pathTile, red, green, transform.rotation, EnemySpawner));
        listOfBottomLeftPoints.Add(new Vector2(-(GlobalVariables.squareWidth / 2), -(GlobalVariables.squareHeight / 2)));
    }

    private void NextRound()
    {
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");

        foreach (GameObject spawner in enemySpawners)
        {
            Destroy(spawner);
        }

        for (int i = 0; i < listOfPaths.Count; i++)
        {
            Vector2 pathEnd = listOfPaths[i].Peek();
            Vector2 direction = ReturnDirection(pathEnd);
            
            if (direction != Vector2.zero)
            {
                Vector2 bottomLeftPoint = new Vector2(listOfBottomLeftPoints[i].x + (GlobalVariables.squareWidth * direction.x) + direction.x, listOfBottomLeftPoints[i].y + (GlobalVariables.squareHeight * direction.y) + direction.y);

                listOfBottomLeftPoints[i] = bottomLeftPoint;

                DrawBackground(GlobalVariables.squareWidth + 1, GlobalVariables.squareHeight + 1, bottomLeftPoint);
                // List<Vector2> possiblePos = new List<Vector2> {new Vector2(1,1), new Vector2(0,1), new Vector2(1,0), new Vector2(0,0)};
                // possiblePos.Remove(new Vector2(direction.x * (-1), direction.y * (-1)));
                // 
                // int pos = Random.Range(0, possiblePos.Count);
                // 
                // int posX = (int) possiblePos[pos].x;
                // int posY = (int) possiblePos[pos].y;
                // 
                // if (posX == 0)
                // {
                //     posX = Random.Range(0, 2) * 2 - 1;
                // }
                // if (posY == 0)
                // {
                //     posY = Random.Range(0, 2) * 2 - 1;
                // }

                // int tempVal = Random.Range(0, 2);

                // Vector2 topRightPoint = new Vector2(bottomLeftPoint.x + GlobalVariables.squareWidth, bottomLeftPoint.y + GlobalVariables.squareHeight);
                // Debug.Log(bottomLeftPoint.x);
                // Debug.Log(bottomLeftPoint.y);
                // if (tempVal == 0)
                // {
                //     int tempX = (GlobalVariables.squareWidth) + (int)bottomLeftPoint.x;
                //     int tempY = Random.Range(0, (GlobalVariables.squareHeight / 2)) * posY + (int)bottomLeftPoint.y;
                // 
                //     Debug.Log(tempVal);
                // 
                //     Debug.Log(tempX);
                //     Debug.Log(tempY);
                // 
                //     listOfX.Add(tempX);
                //     listOfY.Add(tempY);
                // 
                //     pathStart = new Vector2(tempX, tempY);
                //     path.Enqueue(pathStart);
                // } else
                // {
                //     int tempY = (GlobalVariables.squareHeight) + (int)bottomLeftPoint.y;
                //     int tempX = Random.Range(0, (GlobalVariables.squareWidth / 2)) * posX + (int)bottomLeftPoint.x;
                // 
                //     Debug.Log(tempVal);
                // 
                //     Debug.Log(tempX);
                //     Debug.Log(tempY);
                // 
                //     listOfX.Add(tempX);
                //     listOfY.Add(tempY);
                // 
                //     pathStart = new Vector2(tempX, tempY);
                //     path.Enqueue(pathStart);
                // }

                List<int> listOfX = new List<int>();
                List<int> listOfY = new List<int>();

                Vector2 randomPoint = GetRandomPointOnSquareEdge(GlobalVariables.squareWidth, GlobalVariables.squareHeight, direction);

                // Vector2 tempVec = direction;
                // 
                // if (tempVec.x == 0)
                // {
                //     tempVec.x = 1;
                // } else if (tempVec.y == 0)
                // {
                //     tempVec.y = 1;
                // }

                Vector2 pathStart = new Vector2(bottomLeftPoint.x + randomPoint.x, bottomLeftPoint.y +  randomPoint.y);

                listOfX.Add((int) pathStart.x);
                listOfY.Add((int) pathStart.y);

                Queue<Vector2> path = new Queue<Vector2>();

                path.Enqueue(pathStart);

                path = PathManager.CreatePoints(listOfX, listOfY, path, pathEnd, pathStart, numPointsForInter, bottomLeftPoint);

                path = PathManager.OrderQueue(pathStart, pathEnd, path);

                path.Enqueue(pathEnd);
                Queue<Vector2> fullPath = PathManager.CreatePath(pathStart, pathEnd, map, path, pathTile, red, green);

                while (listOfPaths[i].Count > 0)
                {
                    fullPath.Enqueue(listOfPaths[i].Dequeue());
                }

                listOfPaths[i] = fullPath;

                PathManager.CreateEnemySpawner(EnemySpawner, map, pathStart, transform.rotation, listOfPaths[i]);

                // Debug.Log(listOfPaths);
            }
        }
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
                break;
            case 1: // Right edge
                x = width;
                y = Random.Range(1, height);
                break;
            case 2: // Top edge
                x = Random.Range(1, width);
                y = height;
                break;
            case 3: // Left edge
                x = 0;
                y = Random.Range(1, height);
                break;
        }

        return new Vector2(x, y);
    }

    private List<int> RemoveEdge(Vector2 direction)
    {
        // edge (0: bottom, 1: right, 2: top, 3: left)
        List<int> temp = new List<int> { 0, 1, 2, 3 };

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
