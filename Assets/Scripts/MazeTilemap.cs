using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType {EMPTY, WALL, PELLET}

public class MazeTilemap : MonoBehaviour {
    public static Tilemap _MazeTilemap;
    public static TileType[,] MazeMap;
    int mX;
    int mY;
    // Use this for initialization
    void Start () {

        _MazeTilemap = GetComponent<Tilemap>();

        mX = 17;
        mY = 18;
        MazeMap = new TileType[mX, mY];
        //TileBase tiletest = _MazeTilemap.GetTile(new Vector3Int(0, 0, 0));

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
        print();
    }

    void print()
    {
        string mapString = "";
        for (int x = 0; x < mX; x++)
        {
            for (int y = 0; y < mY; y++)
            {
                mapString += MazeMap[x, y] + " | ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
