using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : Pawn {
    //Time interval for when the enemy can make a decision
    const float mDeltaDecisionTime = 0.2f;

    //time count until next enemy decision
    float mDecisionTime = 0f;

    //roughly the distance of on step in any direction
    const float mStepDistance = 0.8f;

    //Distance to which the enemy may aggro on the player
    const float mLatchingDistance = 2.5f;

    //If the enemy should chase the player
    bool mIsLatched = false;

    bool mIsAlive = true;

    //Player got a powerup
    bool mIsScared = false;

    //get reference of player to know where Pacman is in relation to the enemy
    Player mPlayer;

	// Use this for initialization
	void Start () {
        GetAndSortPathColliders();
        mPlayer = FindObjectOfType<Player>();
        mSpeed *= 0.85f;

        //TileBase tiletest = MazeTilemap._MazeTilemap.GetTile(new Vector3Int(0, 0, 0));
        ////MazeTilemap._MazeTilemap.SetTile(ConvertDirectionToPosition(MazeTilemap._MazeTilemap.WorldToCell(transform.position), Direction.DOWN), null);
        //MazeTilemap._MazeTilemap.SetTile(new Vector3Int(1, 1, 0), tiletest);
        //MazeTilemap.print();
    }

    // Update is called once per frame
    void Update () {
        CheckPlayerDistance();
        mDecisionTime += Time.deltaTime;

        if (mIsLatched && IsCloseEnoughToCellCenter())//if the player is close by, follow it
        {
            mDirection = DecideNextMove();
            mDecisionTime = 0;
        }
        if (!MoveIfPossible(mDirection))//couldnt move
        {
            if (isDocile())
            {
                mDirection = GetRandomDirection();
                MoveIfPossible(mDirection);
            }
        }
      //  Debug.Log(mDirection);
    }

    /**
     * This function is used to check if the enemy is close enough to the center of the tilemap cell
     * to approximate it's position for to successfully use breadth first search path finding
     */
    bool IsCloseEnoughToCellCenter()
    {
        bool isCloseEnough = false;
        Vector3Int currentCellPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
        Vector3 cellWordlPostion = MazeTilemap._MazeTilemap.GetCellCenterWorld(currentCellPosition);

        float distanceToCellCenter = Vector3.Distance(transform.position, cellWordlPostion);
        if (distanceToCellCenter < 0.05f)
        {
            isCloseEnough = true;
        }
        return isCloseEnough;
    }

    /**
     * This its to randomly generate and directions in case of wall collision
     * or if the pathfinding algorithm fails (which it shouldn't)
     */
    Direction GetRandomDirection()
    {
        Direction newDirection;
        do
        {
            newDirection = (Direction)UnityEngine.Random.Range(0, 4);
        }
        while (newDirection == mDirection && !isPathFree(newDirection));
        return newDirection;
    }

    /**
     * Checks the if the player is close enough to latch onto
     */
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(mPlayer.transform.position, transform.position);
        mIsLatched = distanceToPlayer < mLatchingDistance;
    }

    /**
     * Decide the best move to catch the player.
     * This uses a basic breadth first search
     */
    Direction DecideNextMove(){
        Vector3Int enemyPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
        Vector3Int playerPosition = MazeTilemap._MazeTilemap.WorldToCell(mPlayer.transform.position);

        Queue<KeyValuePair<Direction, Vector3Int>> queue = new Queue<KeyValuePair<Direction, Vector3Int>>();
        Queue<KeyValuePair<Direction, Vector3Int>> dequeued = new Queue<KeyValuePair<Direction, Vector3Int>>();

        EnqueueEmptyPositions(new KeyValuePair<Direction,Vector3Int> (mDirection, enemyPosition), ref  queue,ref dequeued, true);

        KeyValuePair<Direction, Vector3Int> current;
        Direction chosenDirection = mDirection;
        while (queue.Count != 0)
        {
            current = queue.Dequeue();
            if(current.Value == playerPosition)
            {
                chosenDirection = current.Key;
                break;
            }
            dequeued.Enqueue(current);
            EnqueueEmptyPositions(current, ref queue, ref dequeued);
        }
        queue.Clear();
        dequeued.Clear();
        return chosenDirection;
    }

    /**
     * Enqueues new possible moves for the enemy based on the given position.
     * The KeyValuePair isused to keep track of the original chosen direction from the first level search.
     * This was done to easily get the winning original root decision.
     */
    void EnqueueEmptyPositions(KeyValuePair<Direction, Vector3Int> directionPosition, ref Queue<KeyValuePair<Direction, Vector3Int>> queue, ref Queue<KeyValuePair<Direction, Vector3Int>> dequeued, bool isFirstLevel = false )
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int newPosition = ConvertDirectionToPosition(directionPosition.Value, direction);
            if (MazeTilemap.isPositionEmpty(newPosition))
            {
                if (isFirstLevel)//if this is a root decision, assign the direction into the keyvalue pair
                {
                    queue.Enqueue(new KeyValuePair<Direction, Vector3Int>(direction, newPosition));
                }
                else
                {
                    KeyValuePair<Direction, Vector3Int> newDirectionPosition = new KeyValuePair<Direction, Vector3Int>(directionPosition.Key, newPosition);
                    if (!queue.Contains(newDirectionPosition) && !dequeued.Contains(newDirectionPosition))//check if already visited
                    {
                        queue.Enqueue(newDirectionPosition);
                    }
                }
            }
        }
    }

    Vector3Int ConvertDirectionToPosition(Vector3Int position, Direction direction)
    {
        Vector2Int directionVector = GetVectorFromDirection(direction);
        return new Vector3Int(position.x + directionVector.x, position.y + directionVector.y, position.z);
     }



    Direction GetClosestDirection()
    {
        List<KeyValuePair<Direction, float>> freeDirections = GetFreeDirections();
        KeyValuePair<Direction, float> choice = freeDirections[0];
        foreach (KeyValuePair<Direction, float> entry in freeDirections)
        {
            if(entry.Value< choice.Value)
            {
                choice = entry;
            }
        }
        return choice.Key;
    }

    List<KeyValuePair<Direction,float>> GetFreeDirections(){
        List<KeyValuePair<Direction, float>> freeDirections =new List<KeyValuePair<Direction, float>>();
        foreach(PathCollider pathCollider in mPathColliders)
        {
            if (pathCollider.isFree)
            {
                float directionValue = EvaluateDirectionChoice(pathCollider.mDirection);
                freeDirections.Add(new KeyValuePair<Direction, float>(pathCollider.mDirection, directionValue));
            }
        }
        return freeDirections;
    }

    float EvaluateDirectionChoice(Direction direction)
    {
        Vector2 directionVector = GetVectorFromDirection(direction);
        directionVector *= mStepDistance;
        Vector2 newPosition = new Vector2(transform.position.x + directionVector.x, transform.position.y + directionVector.y);
        float distanceFromStepToPlayer = Vector2.Distance(mPlayer.transform.position, newPosition);

        return distanceFromStepToPlayer;
    }

    public bool isDocile()
    {
        return !mIsLatched && mIsAlive && !mIsScared;
    }

    public override void Die()
    {

    }
}
