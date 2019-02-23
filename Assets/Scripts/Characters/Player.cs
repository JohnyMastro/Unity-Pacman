using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is to give the player control over pacman
public class Player : Pawn {
    [SerializeField]
    AudioClip mWakkaClip;
    [SerializeField]
    AudioClip mDieClip;

    AudioSource mAudioSource;
    Transform mPacmanSpriteTransform;

    bool mIsMoving = false;

    bool mIsDead = false;

    bool mIsAsleep = true;
    // Use this for initialization
    void Start () {
        GetAndSortPathColliders();
        AssignPacmanSprite();
        mIsDead = false;
        mIsAsleep = true;
        mDirection = Direction.RIGHT;
        mOriginalPosition = transform.position;
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.clip = mWakkaClip;
    }

   // Update is called once per frame
	void Update () {
        if (GameStateManager.GetInstance().mIsPaused || mIsDead)
        {
            return;
        }
        Direction newDirection = PlayerControl();
        if (!mIsAsleep)
        {
            PlayerWantsToMove(newDirection);
        }
    }

    //used to poll Player's input
    Direction PlayerControl()
    {
        Direction newDirection = mDirection;
        //right
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            newDirection = AssignNewDirection(Direction.RIGHT);
        }
        //left
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            newDirection = AssignNewDirection(Direction.LEFT);
        }
        //Down
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            newDirection = AssignNewDirection(Direction.DOWN);

        }
        //Up
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            newDirection = AssignNewDirection(Direction.UP);
        }
        return newDirection;
    }
    Direction AssignNewDirection(Direction direction)
    {
        mIsAsleep = false;
        return direction;
    }
    void PlayerWantsToMove(Direction direction)
    {
        bool isNewDirectionFree = isPathFree(direction);
        bool isCurrrentDirectionFree = isPathFree(mDirection);

        mIsMoving = false;
        if (isNewDirectionFree)
        {
            mDirection = direction;
        }

        if(isNewDirectionFree || isCurrrentDirectionFree)
        {
            mIsMoving = true;
            Move(mDirection);
        }

        if (mIsMoving)
        {
            if (!mAudioSource.isPlaying)
            {
                mAudioSource.Play();
            }
            Vector2Int directionVector = GetVectorFromDirection(mDirection);
            if (directionVector.y != 0)
            {
                mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, directionVector.y) * 90);
            }
            else if (directionVector.x != 0)
            {
                mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, directionVector.x - 1) * 90);
            }
        }
        mAnimator.SetBool("move", mIsMoving);
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
            mIsAsleep = true;
            mIsDead = true;
            mIsMoving = false;
            mAnimator.SetBool("move", mIsMoving);
            mPacmanSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            mAnimator.SetTrigger("die");
            mAudioSource.clip = mDieClip;
            mAudioSource.Stop();
            mAudioSource.Play();
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
        mIsDead = false;
        mIsMoving = false;
        mDirection = Direction.LEFT;
        transform.position = mOriginalPosition;
        mAudioSource.clip = mWakkaClip;
        mAnimator.ResetTrigger("die");
    }

    protected override void AddPoints()
    {

    }
}
