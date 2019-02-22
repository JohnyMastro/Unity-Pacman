using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PelletTilemap : Pellet {
    public Tilemap _PelletTilemap;

    // Use this for initialization
    void Start () {
        _PelletTilemap = GetComponent<Tilemap>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Vector3 collisionPosition = Vector3.zero;
            ContactPoint2D contact = collision.contacts[0];
            collisionPosition.x = contact.point.x - 0.01f * contact.normal.x;
            collisionPosition.y = contact.point.y - 0.01f * contact.normal.y;
            _PelletTilemap.SetTile(_PelletTilemap.WorldToCell(collisionPosition), null);
            AddPoints();
        }
    }

}
