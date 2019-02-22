using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RespawnTilemap : MonoBehaviour {
    public static Tilemap _Tilemap;
    private static Vector3Int _RespawnTilePosition;

    public static Vector3Int mRespawnTilePosition
    {
        get{ return _RespawnTilePosition; }
    }

    // Use this for initialization
    void Start () {
        _Tilemap = GetComponent<Tilemap>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Vector3Int FindRespawnTile()
    {
        int mX = MazeTilemap.mX;
        int mY = MazeTilemap.mY;
        bool loopBreaker = false;
        for (int x = 0; x < mX; x++)
        {
            for (int y = 0; y < mY; y++)
            {
                Vector3Int tmpPosition = new Vector3Int(x, y, 0);
                TileBase tile = _Tilemap.GetTile(tmpPosition);
                if (tile != null)
                {
                    _RespawnTilePosition = tmpPosition;
                    loopBreaker = true;
                    break;
                }
            }
            if (loopBreaker)
            {
                break;
            }
        }
        return mRespawnTilePosition;
    }
}
