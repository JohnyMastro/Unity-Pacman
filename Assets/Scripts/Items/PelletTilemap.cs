using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * inherits Pellet class
 * This manages collision and behviours of the tilemap that holds standard pellets
 */
public class PelletTilemap : Pellet {
    public Tilemap _Tilemap;
    AudioSource mAudioSourceEat;

    // Use this for initialization
    void Start () {
        InitPelletTilemap();
    }
    private void Awake()
    {
        mAudioSourceEat = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnPlayerCollisionWithAudio(collider);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        OnPlayerCollisionWithAudio(collider);
    }

    void OnPlayerCollisionWithAudio(Collider2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            if (_Tilemap.GetTile(_Tilemap.WorldToCell(collider.gameObject.transform.position)) != null)
            {
                OnPlayerCollision(collider);
                if (!mAudioSourceEat.isPlaying)
                {
                    mAudioSourceEat.Play();
                }
            }
        }
    }

    protected virtual void OnPlayerCollision(Collider2D collider)
    {
        _Tilemap.SetTile(_Tilemap.WorldToCell(collider.gameObject.transform.position), null); //Destroy pellet tile
        AddPoints();
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

    /**
     * Adds the number of standard pellets to the pellets count in the GameStateManager
     */
    protected void InitPelletTilemap()
    {
        _Tilemap = GetComponent<Tilemap>();
        GameStateManager gInstance = GameStateManager.GetInstance();
        gInstance.AddPelletCount(GetNumberOfPellets());
    }

}
