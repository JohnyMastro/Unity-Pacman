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
       // _MazeTilemap.SetTile(new Vector3Int(1, 1, 0), tiletest);
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
