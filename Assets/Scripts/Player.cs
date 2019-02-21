using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is to give the player control over pacman
public class Player : Pawn {

	// Use this for initialization
	void Start () {
        mDirection = Direction.LEFT;
        GetAndSortPathColliders();
    }

   // Update is called once per frame
	void Update () {
        PlayerControl();
    }

    //used to poll Player's input
    void PlayerControl()
    {
        //right
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            MoveIfPossible(Direction.RIGHT);
        }
        //left
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            MoveIfPossible(Direction.LEFT);
        }
        //Down
        else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            MoveIfPossible(Direction.DOWN);
        }
        //Up
        else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            MoveIfPossible(Direction.UP);
        }
    }


    public override void Die()
    {

    }
}
