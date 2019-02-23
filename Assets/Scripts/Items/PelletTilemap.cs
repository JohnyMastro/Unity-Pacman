using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PelletTilemap : Pellet {
    public Tilemap _Tilemap;

    // Use this for initialization
    void Start () {
        InitPelletTilemap();
    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnPlayerCollision(collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        OnPlayerCollision(collider);
    }

    protected virtual void OnPlayerCollision(Collider2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            if (_Tilemap.GetTile(_Tilemap.WorldToCell(collider.gameObject.transform.position)) != null)
            {
                _Tilemap.SetTile(_Tilemap.WorldToCell(collider.gameObject.transform.position), null);
                AddPoints();
            }
        }
    }

    protected int GetNumberOfPellets()
    {
        int mX = MazeTilemap.mX;
        int mY = MazeTilemap.mY;
        int count = 0;

        for (int x = 0; x < mX; x++)
        {
            for (int y = 0; y < mY; y++)
            {
                TileBase tile = _Tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    count++;
                }
            }
        }
        return count;
    }

    protected void InitPelletTilemap()
    {
        _Tilemap = GetComponent<Tilemap>();
        GameStateManager gInstance = GameStateManager.GetInstance();
        gInstance.AddPelletCount(GetNumberOfPellets());
    }

}
