using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum used to set direction of movement
public enum Direction { UP, DOWN, LEFT, RIGHT};

/**
 * Base abstract class to be inherited by AI and Playable Characters
 * Inheriting Collectible so that enemies can add points
 */
public abstract class Pawn : Collectible {
    public Direction mDirection = Direction.LEFT;
    protected float mSpeed = 0.03f;
    protected PathCollider[] mPathColliders;
    protected SpriteRenderer mSpriteRenderer;
    protected Animator mAnimator;
    protected Vector3 mOriginalPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Used to translate gameobject to desired location based on direction type
    public void Move(Direction direction)
    {
        Vector2 directionVector = GetVectorFromDirection(direction);
        mDirection = direction;
        directionVector *= mSpeed;
        transform.position = new Vector2(transform.position.x + directionVector.x, transform.position.y + directionVector.y);
    }

    //ConvertsDirection type to Vector2Int
    protected Vector2Int GetVectorFromDirection(Direction direction)
    {
        Vector2Int directionVector;
        switch (direction)
        {
            case Direction.UP:
                directionVector = Vector2Int.up;
                break;
            case Direction.DOWN:
                directionVector = Vector2Int.down;
                break;
            case Direction.LEFT:
                directionVector = Vector2Int.left;
                break;
            case Direction.RIGHT:
                directionVector = Vector2Int.right;
                break;
            default:
                directionVector = Vector2Int.zero;
                break;
        }
        return directionVector;
    }

    /**
     * Used to Retrieve path colliders from sub-Gameobject and sorts them by direction
     */ 
    protected void GetAndSortPathColliders()
    {
        PathCollider[] tmpPathColliders = GetComponentsInChildren<PathCollider>();
        mPathColliders = new PathCollider[4];
        foreach(PathCollider tmpPathCollider in tmpPathColliders)
        {
            mPathColliders[(int)tmpPathCollider.mDirection] = tmpPathCollider;
        }
    }

    /**
     * Will permit pawn to move in that direction if that cell is free
     */
    protected bool MoveIfPossible(Direction direction)
    {
        bool didMove = false;
        if (isPathFree(direction))
        {
            Move(direction);
            didMove = true;
        }
        return didMove;
    }

    /**
     * Checks if the path collider is free for the pawn to move to
     */
    protected bool isPathFree(Direction direction)
    {
        bool canMove = true;
        if (mPathColliders != null)
        {
            if (mPathColliders[(int)direction])
            {
                canMove = mPathColliders[(int)direction].isFree;
            }
            else
            {
                Debug.LogError("GetAndSortPathColliders was not called!");
            }
        }
        else
        {
            Debug.LogError("PathColliders are NULL!");
        }
        return canMove;
    }

    public abstract void Die();

    public abstract void ResetPawn();

}
