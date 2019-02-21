using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn {

	// Use this for initialization
	void Start () {
        GetAndSortPathColliders();
    }

    // Update is called once per frame
    void Update () {
        MoveIfPossible(mDirection);
    }

    public override void Die()
    {

    }
}
