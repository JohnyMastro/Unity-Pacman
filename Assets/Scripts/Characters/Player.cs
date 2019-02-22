using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is to give the player control over pacman
public class Player : Pawn {

    bool mIsPoweredUp = false;

    float mPowerUpTimer = 0f;

    const float mPowerUpDeltaTime = 10f;

    // Use this for initialization
    void Start () {
        mDirection = Direction.LEFT;
        GetAndSortPathColliders();
    }

   // Update is called once per frame
	void Update () {
        PlayerControl();
        PowerPolling();
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
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            MoveIfPossible(Direction.LEFT);
        }
        //Down
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            MoveIfPossible(Direction.DOWN);
        }
        //Up
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            MoveIfPossible(Direction.UP);
        }
    }

    public void PowerUp()
    {
        mIsPoweredUp = true;
    }

    public bool IsPowerUp()
    {
       return mIsPoweredUp;
    }

    public void PowerDown()
    {
        mIsPoweredUp = false;
    }

    void PowerPolling()
    {
        if (mIsPoweredUp)
        {
            mPowerUpTimer += Time.deltaTime;
            if (mPowerUpTimer > mPowerUpDeltaTime)
            {
                PowerDown();
            }
        }
        else
        {
            mPowerUpTimer = 0;
        }
    }
    public override void Die()
    {

    }

    protected override void AddPoints()
    {

    }
}
