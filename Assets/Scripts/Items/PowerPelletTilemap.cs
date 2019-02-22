using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PowerPelletTilemap : PelletTilemap
{
	// Use this for initialization
	void Start () {
        mPoints = 200;
        InitPelletTilemap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnPlayerCollision(collision);
    }
    protected override void OnPlayerCollision(Collision2D collision)
    {
        base.OnPlayerCollision(collision);
        if(collision.gameObject.tag == "player")
        {
            collision.gameObject.GetComponent<Player>().PowerUp();
        }

    }

}
