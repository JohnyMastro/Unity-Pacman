using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, DOWN, LEFT, RIGHT };

//Base abstract class to be inherited by AI and Playable Characters
public abstract class Pawn : MonoBehaviour {
    protected Direction mDirection = Direction.LEFT;
    protected float mSpeed = 0.05f;

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

    public abstract void Die();
}
