using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

/**
 * inherits PelletTilemap class
 * This manages collision and behviours of the tilemap that holds power pellets
 */
public class PowerPelletTilemap : PelletTilemap
{
    AudioSource mAudioSource;
    // Use this for initialization
    void Start () {
        mPoints = 200;
        InitPelletTilemap();
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnPlayerCollision(collider);
    }

    protected override void OnPlayerCollision(Collider2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            if (_Tilemap.GetTile(_Tilemap.WorldToCell(collider.gameObject.transform.position)) != null)
            {
                base.OnPlayerCollision(collider);

                GameStateManager.GetInstance().PowerUp();//Notify enemies
                InstantiatePoints(collider.gameObject.transform);
                if (!mAudioSource.isPlaying)
                {
                    mAudioSource.Play();
                }
            }
        }

    }


}
