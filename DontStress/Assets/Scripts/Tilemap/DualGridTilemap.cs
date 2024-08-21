using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static TileType;

public class DualGridTilemap : MonoBehaviour
{
    protected static Vector3Int[] NEIGHBOURS = new Vector3Int[] {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 1, 0)
    };

    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, Tile> neighbourTupleToTile;

    // Provide references to each tilemap in the inspector
    public Tilemap placeholderTilemap;
    public Tilemap displayTilemap;

    // Provide the path and grass placeholder tiles in the inspector
    public Tile pathPlaceholderTile;
    public Tile grassPlaceholderTile;

    // Provide the 16 tiles in the inspector
    public Tile[] tiles;

    void Start()
    {
            // This dictionary stores the "rules", each 4-neighbour configuration corresponds to a tile
            // |_1_|_2_|
            // |_3_|_4_|
            neighbourTupleToTile = new() {
            {new (Grass, Grass, Grass, Grass), tiles[6]},
            {new (Path, Path, Path, Grass), tiles[13]}, // OUTER_BOTTOM_RIGHT
            {new (Path, Path, Grass, Path), tiles[0]}, // OUTER_BOTTOM_LEFT
            {new (Path, Grass, Path, Path), tiles[8]}, // OUTER_TOP_RIGHT
            {new (Grass, Path, Path, Path), tiles[15]}, // OUTER_TOP_LEFT
            {new (Path, Grass, Path, Grass), tiles[1]}, // EDGE_RIGHT
            {new (Grass, Path, Grass, Path), tiles[11]}, // EDGE_LEFT
            {new (Path, Path, Grass, Grass), tiles[3]}, // EDGE_BOTTOM
            {new (Grass, Grass, Path, Path), tiles[9]}, // EDGE_TOP
            {new (Path, Grass, Grass, Grass), tiles[5]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Path, Grass, Grass), tiles[2]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Path, Grass), tiles[10]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Path), tiles[7]}, // INNER_TOP_LEFT
            {new (Path, Grass, Grass, Path), tiles[14]}, // DUAL_UP_RIGHT
            {new (Grass, Path, Path, Grass), tiles[4]}, // DUAL_DOWN_RIGHT
            {new (Path, Path, Path, Path), tiles[12]},
        };
        //for (int i = -50; i < 50; i++)
        //{
        //    for (int j = -50; j < 50; j++)
        //    {
        //        Vector3Int tilePosition = new Vector3Int(i, j, 0);
        //        placeholderTilemap.SetTile(tilePosition, grassPlaceholderTile);

        //    }
        //}
        RefreshDisplayTilemap();
    }

    public void RefreshDisplayTilemap()
    {
        int minX = -100;
        int maxX = 100;
        int minY = -100;
        int maxY = 100;

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Tile tileToSet = calculateDisplayTile(tilePosition);
                displayTilemap.SetTile(tilePosition, tileToSet);
                setDisplayTile(tilePosition);
            }
        }

        displayTilemap.RefreshAllTiles();
        // Optionally: Refresh or update the tilemap if needed
        //displayTilemap.RefreshAllTiles(); 
    }

    protected Tile calculateDisplayTile(Vector3Int coords)
    {
        // 4 neighbours
        TileType topRight = getPlaceholderTileTypeAt(coords - NEIGHBOURS[0]);
        TileType topLeft = getPlaceholderTileTypeAt(coords - NEIGHBOURS[1]);
        TileType botRight = getPlaceholderTileTypeAt(coords - NEIGHBOURS[2]);
        TileType botLeft = getPlaceholderTileTypeAt(coords - NEIGHBOURS[3]);

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(topLeft, topRight, botLeft, botRight);

        return neighbourTupleToTile[neighbourTuple];
    }

    protected void setDisplayTile(Vector3Int pos)
    {
        for (int i = 0; i < NEIGHBOURS.Length; i++)
        {
            Vector3Int newPos = pos + NEIGHBOURS[i];
            displayTilemap.SetTile(newPos, calculateDisplayTile(newPos));
        }
    }
    public void SetCell(Vector3Int coords, Tile tile)
    {
        placeholderTilemap.SetTile(coords, tile);
        setDisplayTile(coords);
    }

    private TileType getPlaceholderTileTypeAt(Vector3Int coords)
    {
        if (placeholderTilemap.GetTile(coords) == grassPlaceholderTile)
            return Grass;
        else
            return Path;
    }


    // The tiles on the display tilemap will recalculate themselves based on the placeholder tilemap
    //public void RefreshDisplayTilemap()
    //{
    //    for (int i = -50; i < 50; i++)
    //    {
    //        for (int j = -50; j < 50; j++)
    //        {
    //            ////setDisplayTile(new Vector3Int(i, j, 0));

    //            Vector3Int tilePosition = new Vector3Int(i, j, 0);
    //            Tile tileToSet = calculateDisplayTile(tilePosition);
    //            setDisplayTile(tilePosition);
    //        }
    //    }
    //}

}

public enum TileType
{
    None,
    Grass,
    Path
}
