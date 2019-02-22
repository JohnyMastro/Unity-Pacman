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

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnPlayerCollision(collision);
    }

    protected virtual void OnPlayerCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Vector3 collisionPosition = Vector3.zero;
            ContactPoint2D contact = collision.contacts[0];
            collisionPosition.x = contact.point.x - 0.01f * contact.normal.x;
            collisionPosition.y = contact.point.y - 0.01f * contact.normal.y;
            _Tilemap.SetTile(_Tilemap.WorldToCell(collisionPosition), null);
            AddPoints();
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
