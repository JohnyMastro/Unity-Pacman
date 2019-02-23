using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is to give the player control over pacman
public class Player : Pawn {

    bool mIsPoweredUp = false;

    float mPowerUpTimer = 0f;

    const float mPowerUpDeltaTime = 7f;

    Transform mPacmanSpriteTransform;

    bool mIsMoving = false;

    bool mIsDead = false;

    // Use this for initialization
    void Start () {
        GetAndSortPathColliders();
        AssignPacmanSprite();
        mIsDead = false;
        mDirection = Direction.LEFT;
        mOriginalPosition = transform.position;
        mAnimator = GetComponent<Animator>();
    }

   // Update is called once per frame
	void Update () {
        if (GameStateManager.GetInstance().mIsPaused || mIsDead)
        {
            return;
        }
        PlayerControl();
        PowerPolling();
    }

    //used to poll Player's input
    void PlayerControl()
    {
        mIsMoving = false;
        //right
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            PlayerWantsToMove(Direction.RIGHT);
        }
        //left
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            PlayerWantsToMove(Direction.LEFT);
        }
        //Down
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            PlayerWantsToMove(Direction.DOWN);
        }
        //Up
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            PlayerWantsToMove(Direction.UP);
        }

        mAnimator.SetBool("move", mIsMoving);
    }

    void PlayerWantsToMove(Direction direction)
    {
        mIsMoving = MoveIfPossible(direction);
        if (mIsMoving)
        {
            Vector2Int directionVector = GetVectorFromDirection(direction);
            if (directionVector.y != 0){
                mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, directionVector.y) * 90);
            }
            else if (directionVector.x != 0){
                mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, directionVector.x - 1) * 90);
            }
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
        mPowerUpTimer = 0;
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
    }

    void AssignPacmanSprite()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "pacmanSprite")
            {
                mPacmanSpriteTransform = child;
                break;
            }
        }
    }

    public override void Die()
    {
        if (!mIsDead)
        {
            mIsDead = true;
            mIsMoving = false;
            mAnimator.SetBool("move", mIsMoving);
            mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            mAnimator.SetTrigger("die");
        }
    }

    public void AfterDeathCallBack()
    {
        GameStateManager gInstance = GameStateManager.GetInstance();
        gInstance.LoseLife();
        if (!gInstance.IsGameOver())
        {
            gInstance.ResetLevelOnPlayerDeath();
        }
    }
    public override void ResetPawn()
    {
        PowerDown();
        mIsDead = false;
        mIsMoving = false;
        mDirection = Direction.LEFT;
        transform.position = mOriginalPosition;
        mAnimator.ResetTrigger("die");
    }

    protected override void AddPoints()
    {

    }
}
