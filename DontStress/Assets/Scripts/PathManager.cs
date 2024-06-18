using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathManager : MonoBehaviour
{
    public int numPoints;
    // public int rangeX, rangeY;
    private Vector2 pathStart;
    private Vector2 pathEnd = Vector2.zero;

    public Tilemap map;
    public TileBase pathTile, red, green;

    private Queue<Vector2> path;

    // Start is called before the first frame update
    void Start()
    {
        Run();
    }

    public void Run()
    {
        // Initialize the path queue
        path = new Queue<Vector2>();
        int posX = Random.Range(0, 2) * 2 - 1;
        int posY = Random.Range(0, 2) * 2 - 1;

        pathStart = new Vector2(Random.Range(9, 10) * posX, Random.Range(0, 10) * posY);
        path.Enqueue(pathStart);
        int x, y;

        for (int i = 0; i < numPoints; i++)
        {
            x = Random.Range(2, 10) * posX;
            y = Random.Range(-8, 8);
            path.Enqueue(new Vector2(x, y));
        }

        path = OrderQueue(pathStart, pathEnd);

        path.Enqueue(pathEnd);
        CreatePath(pathStart, pathEnd);
    }

    private Queue<Vector2> OrderQueue(Vector2 start, Vector2 end)
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

    private void CreatePath(Vector2 start, Vector2 end)
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

            map.SetTile(new Vector3Int((int) firstPoint.x, (int) firstPoint.y, 0), red);
            map.SetTile(new Vector3Int((int) secondPoint.x, (int) secondPoint.y, 0), red);

            // Update the firstPoint to be the secondPoint for the next iteration
            firstPoint = secondPoint;
        }
        map.SetTile(new Vector3Int((int)start.x, (int)start.y, 0), green);
        map.SetTile(new Vector3Int((int)end.x, (int)end.y, 0), green);
    }
}
