using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Enemy : Pawn {
    enum SpriteType {STANDARD, SCARED, DEAD};

    float mOriginalSpeed;

    //Time interval for when the enemy can make a decision
    const float mDeltaDecisionTime = 1f;

    //time count until next enemy decision
    float mDecisionTime = mDeltaDecisionTime;

    //roughly the distance of on step in any direction
    const float mStepDistance = 0.8f;

    //Distance to which the enemy may aggro on the player
    const float mLatchingDistance = 2.5f;

    //If the enemy should chase the player
    bool mIsLatched = false;

    bool mIsAlive = true;

    bool mIsRespawned = false;

    //Player got a powerup
    bool mIsScared = false;

    Color mOriginalColor;

    [SerializeField]
    Sprite[] mSprites =new Sprite[Enum.GetNames(typeof(SpriteType)).Length];

    //get reference of player to know where Pacman is in relation to the enemy
    Player mPlayer;

    readonly Vector3Int mSpawnPosition = new Vector3Int(8, 10, 0);

    // Use this for initialization
    void Start () {
        GetAndSortPathColliders();
        mPlayer = FindObjectOfType<Player>();
        mSpeed *= 0.8f;
        mOriginalSpeed = mSpeed;
        mPoints = 300;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mOriginalColor = mSpriteRenderer.color;
        //mSprites[(int)SpriteType.STANDARD] = mSpriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update () {
        CheckPlayerStatus();
        AIControls();
        RespawnIfPossible();
        UpdateSpriteAndSpeed();
      //  Debug.Log(mDirection);
    }

    /**
     * Let Enemy decide what is the best move in it's current state
     */
    void AIControls()
    {
        if (!mIsAlive)
        {
            if (IsCloseEnoughToCellCenter())//if the player is close by, follow it
            {
                mDirection = DecideNextMoveBFS(mSpawnPosition);
            }
        }
        else if (mIsLatched)//if the player is close by
        {
            if (isVulnerable())//if the player is powered up and close, the enemy will try to run away
            {
                mDecisionTime += Time.deltaTime;
                //if (mDecisionTime > mDeltaDecisionTime)
                //{
                    mDirection = GetFarthestDirection();
                  //  mDecisionTime = 0f;
               // }
            }
            else if (IsCloseEnoughToCellCenter())//if the enemy is alive, not scared, and close enough to player
            {
                Vector3Int playerPosition = MazeTilemap._MazeTilemap.WorldToCell(mPlayer.transform.position);
                mDirection = DecideNextMoveBFS(playerPosition);
            }
        }

        if (!MoveIfPossible(mDirection))//couldnt move
        {
            mDirection = GetRandomDirection();
            MoveIfPossible(mDirection);
        }
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
    void CheckPlayerStatus()
    {
        float distanceToPlayer = Vector2.Distance(mPlayer.transform.position, transform.position);
        mIsLatched = distanceToPlayer < mLatchingDistance;
        if (mPlayer.IsPowerUp())
        {
            mIsScared = true;
        }
        else
        {
            mIsScared = false;
            mIsRespawned = false;
            mDecisionTime = mDeltaDecisionTime;
        }
    }

    /**
     * Decide the best move to catch the player or to go to spawn point.
     * This uses a basic breadth first search
     */
    Direction DecideNextMoveBFS(Vector3Int goalPosition)
    {
        Vector3Int enemyPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
        Queue<KeyValuePair<Direction, Vector3Int>> queue = new Queue<KeyValuePair<Direction, Vector3Int>>();
        Queue<KeyValuePair<Direction, Vector3Int>> dequeued = new Queue<KeyValuePair<Direction, Vector3Int>>();

        EnqueueEmptyPositions(new KeyValuePair<Direction,Vector3Int> (mDirection, enemyPosition), ref  queue,ref dequeued, true);

        KeyValuePair<Direction, Vector3Int> current;
        Direction chosenDirection = mDirection;
        while (queue.Count != 0)
        {
            current = queue.Dequeue();
            if(current.Value == goalPosition)
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

    void UpdateSpriteAndSpeed()
    {

        if(!mIsAlive)
        {
            mSpriteRenderer.sprite = mSprites[(int)SpriteType.DEAD];
            mSpriteRenderer.color = Color.white;
            mSpeed = mOriginalSpeed * 1.3f;
        }
        else if (isVulnerable())
        {
            mSpriteRenderer.sprite = mSprites[(int)SpriteType.SCARED];
            mSpriteRenderer.color = Color.white;
            mSpeed = mOriginalSpeed * 0.5f;
        }
        else
        {
            mSpriteRenderer.sprite = mSprites[(int)SpriteType.STANDARD];
            mSpriteRenderer.color = mOriginalColor;
            mSpeed = mOriginalSpeed;
        }
    }

    void RespawnIfPossible()
    {
        if (!mIsAlive)
        {
            Vector3Int currentCellPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
            if (currentCellPosition == mSpawnPosition)
            {
                mIsAlive = true;
                mIsScared = false;
                mIsRespawned = true;
            }
        }
    }

    Direction GetFarthestDirection()
    {
        List<KeyValuePair<Direction, float>> freeDirections = GetFreeDirections();
        KeyValuePair<Direction, float> choice = freeDirections[0];
        foreach (KeyValuePair<Direction, float> entry in freeDirections)
        {
            if(entry.Value > choice.Value)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (isVulnerable())
            {
                Die();
            }
            else if (mIsAlive)
            {
                mPlayer.Die();
            }
        }
    }
    bool isVulnerable()
    {
        return mIsScared && !mIsRespawned;
    }

    public override void Die()
    {
        mIsAlive = false;
        AddPoints();
    }

    protected override void AddPoints()
    {
        GameStateManager instance = GameStateManager.GetInstance();
        instance.AddPoints(mPoints);
    }
}
