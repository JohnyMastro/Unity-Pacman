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
    const float mLatchingDistance = 2f;

    bool mIsLatched = false;

    bool mIsAlive = true;

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

        if (mDecisionTime > mDeltaDecisionTime && mIsLatched)//if the player is close by, follow it
        {
            DecideNextMove();
            mDecisionTime = 0;
        }
        if (!MoveIfPossible(mDirection))
        {
            if (isDocile())
            {
                mDirection = GetRandomDirection();
            }
        }
       // Debug.Log(mDirection);
    }

    Direction GetRandomDirection()
    {
        Direction newDirection;
        do
        {
            newDirection = (Direction)Random.Range(0, 4);
        }
        while (newDirection == mDirection && !isPathFree(newDirection));
        return newDirection;
    }

    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(mPlayer.transform.position, transform.position);
        mIsLatched = distanceToPlayer < mLatchingDistance;
    }

    //Decide the best move to catch the player
    void DecideNextMove(){
        Vector3Int EnemyPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
        Vector3Int PlayerPosition = MazeTilemap._MazeTilemap.WorldToCell(mPlayer.transform.position);


        //mDirection = GetClosestDirection();
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
