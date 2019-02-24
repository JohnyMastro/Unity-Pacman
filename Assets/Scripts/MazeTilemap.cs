using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType {EMPTY, WALL, PELLET}

/**
 * This manages the tilemap for the maze walls
 * The maze tile map is converted into a 2D array so that the Enemy AI may use 
 * Breadth-first-search
 */
public class MazeTilemap : MonoBehaviour {
    public static Tilemap _MazeTilemap;
    public static TileType[,] MazeMap;
    public const int mX = 17;
    public const int mY = 18;
    // Use this for initialization
    void Start () {

        _MazeTilemap = GetComponent<Tilemap>();

        MazeMap = new TileType[mX, mY];
        for (int x = 0; x < mX; x++)
        {
            for (int y = 0; y < mY; y++)
            {
                TileBase tile = _MazeTilemap.GetTile(new Vector3Int(x,y,0));
                if (tile != null)
                {
                    MazeMap[x, y] = TileType.WALL;
                }
                else
                {
                    MazeMap[x, y] = TileType.EMPTY;
                }
            }
        }
    }

    void print()
    {
        string mapString = "";
        for (int x = mY-1; x >=0; x--)
        {
            for (int y = 0; y < mX; y++)
            {
                mapString += MazeMap[y, x] + " | ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }
	
    public static bool isPositionEmpty(Vector3Int position)
    {
        return MazeMap[position.x, position.y] == TileType.EMPTY;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
