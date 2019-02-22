using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : Collectible {

	// Use this for initialization
	void Start () {
        mPoints = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void AddPoints()
    {
        GameStateManager instance = GameStateManager.GetInstance();
        instance.AddPoints(mPoints);
        instance.PelletWasEaten();
    }
}
