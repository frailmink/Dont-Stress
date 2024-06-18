using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathManager : MonoBehaviour
{
    public int numPoints;
    public int rangeX, rangeY;
    private Vector2 pathStart;
    private Vector2 pathEnd = Vector2.zero;

    public Tilemap map;
    public TileBase pathTile;

    private Queue<Vector2> path;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the path queue
        path = new Queue<Vector2>();
        int posX = Random.Range(0, 2) * 2 - 1;
        int posY = Random.Range(0, 2) * 2 - 1;

        pathStart = new Vector2(Random.Range(0, 10) * posX, Random.Range(9, 10) * posY);
        path.Enqueue(pathStart);
        int x, y;

        for (int i = 0; i < numPoints; i++)
        {
            x = Random.Range(0, 10) * posX;
            y = Random.Range(-8, 8);
            path.Enqueue(new Vector2(x, y));
        }

        path = OrderQueue();

        path.Enqueue(pathEnd);
        CreatePath();
    }

    private Queue<Vector2> OrderQueue()
    {
        Queue<Vector2> q = new Queue<Vector2>(path);

        Vector2 start = q.Dequeue();
        List<Vector2> list = new List<Vector2>(q);
        list.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(a, start);
            float distanceB = Vector2.Distance(b, start);
            return distanceA.CompareTo(distanceB);
        });
        Queue<Vector2> sortedQueue = new Queue<Vector2>();
        sortedQueue.Enqueue(start);
        for (int i = 0; i < list.Count; i++)
        {
            sortedQueue.Enqueue(list[i]);
        }

        return sortedQueue;
    }

    private void CreatePath()
    {
        Vector2 firstPoint = path.Dequeue();
        Vector2 secondPoint;
        while (path.Count != 0)
        {
            secondPoint = path.Dequeue();
            int startX = (int)firstPoint.x;
            int endX = (int)secondPoint.x;
            int startY = (int)firstPoint.y;
            int endY = (int)secondPoint.y;

            // Create path along X-axis from startX to endX
            for (int x = Mathf.Min(startX, endX); x <= Mathf.Max(startX, endX); x++)
            {
                map.SetTile(new Vector3Int(x, startY, 0), pathTile);
            }

            // Create path along Y-axis from startY to endY
            for (int y = Mathf.Min(startY, endY); y <= Mathf.Max(startY, endY); y++)
            {
                map.SetTile(new Vector3Int(endX, y, 0), pathTile);
            }

            // Update the firstPoint to be the secondPoint for the next iteration
            firstPoint = secondPoint;
        }
    }
}
