﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, DOWN, LEFT, RIGHT };

//Base abstract class to be inherited by AI and Playable Characters
public abstract class Pawn : MonoBehaviour {
    protected Direction mDirection = Direction.LEFT;
    protected float mSpeed = 0.05f;
    protected PathCollider[] mPathColliders;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(Direction direction)
    {
        Vector2 directionVector;
        switch (direction)
        {
            case Direction.UP:
                directionVector = Vector2.up;
                break;
            case Direction.DOWN:
                directionVector = Vector2.down;
                break;
            case Direction.LEFT:
                directionVector = Vector2.left;
                break;
            case Direction.RIGHT:
                directionVector = Vector2.right;
                break;
            default:
                directionVector = Vector2.zero;
                break;
        }
        mDirection = direction;
        directionVector *= mSpeed;
        transform.position = new Vector2(transform.position.x + directionVector.x, transform.position.y + directionVector.y);
    }
    protected void GetAndSortPathColliders()
    {
        PathCollider[] tmpPathColliders = GetComponentsInChildren<PathCollider>();
        mPathColliders = new PathCollider[4];
        foreach(PathCollider tmpPathCollider in tmpPathColliders)
        {
            mPathColliders[(int)tmpPathCollider.mDirection] = tmpPathCollider;
        }
    }

    //will permit player to move in that direction if that cell is free
    protected void MoveIfPossible(Direction direction)
    {
        if (isPathFree(direction))
        {
            Move(direction);
        }
    }

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
}
