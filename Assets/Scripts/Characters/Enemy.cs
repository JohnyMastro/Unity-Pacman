using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This class inherits Pawn class and gives ghosts behaviour
 */
public class Enemy : Pawn {
    //used to determin sprite type based on status
    enum SpriteType {STANDARD, SCARED, DEAD};

    float mOriginalSpeed;

    Direction mOriginalDirection;

    //roughly the distance of on step in any direction
    const float mStepDistance = 0.8f;

    //Distance to which the enemy may aggro on the player
    const float mLatchingDistance = 2.5f;

    //If the enemy should chase the player
    bool mIsLatched = false;

    bool mIsAlive = true;

    //Player got a powerup
    bool mIsScared = false;

    Color mOriginalColor;

    [SerializeField]
    Sprite[] mSprites =new Sprite[Enum.GetNames(typeof(SpriteType)).Length];

    //get reference of player to know where Pacman is in relation to the enemy
    Player mPlayer;

    //Where the dead enemies go to respawn
    Vector3Int mSpawnPosition;

    //timer for how long the power up should be
    float mIsScaredTimer = 0f;

    //Power up length
    const float mIsScaredDeltaTimer = 10f;

    //Death sound
    AudioSource mAudioSource;

    // Use this for initialization
    void Start () {
        mSpeed *= 0.7f;
        mOriginalSpeed = mSpeed;
        mPoints = 300;
        mOriginalPosition = transform.position;
        mOriginalDirection = mDirection;

        GetAndSortPathColliders();

        mPlayer = FindObjectOfType<Player>();
        mSpawnPosition = RespawnTilemap.FindRespawnTile();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mOriginalColor = mSpriteRenderer.color;
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.GetInstance().mIsPaused)
        {
            return;
        }
        CheckPlayerStatus();
        AIControls();
        ScaredPolling();
        RespawnIfPossible();
        UpdateSpriteAndSpeed();
    }

    /**
     * Let Enemy decide what is the best move in it's current state
     */
    void AIControls()
    {
        if (!mIsAlive)
        {   
            if (IsCloseEnoughToCellCenter())//if dead, go to respawn point
            {
                mDirection = DecideNextMoveBFS(mSpawnPosition);
            }
        }
        else if (mIsLatched)//if the player is close by
        {
            if (mIsScared)//if the player is powered up and close, the enemy will try to run away
            {
                mDirection = GetFarthestDirection();
            }
            else if (IsCloseEnoughToCellCenter())//if the enemy is alive, not scared, and close enough to player it will chase the player
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
     * to approximate it's position to successfully use breadth first search path finding
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
     * This its to randomly generate and directions if enemy is docile
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
    }

    /**
     * Notify function, called from GameStateManager that the player is powered up
     */
    public void Frighten()
    {
        mIsScared = true;
        mIsScaredTimer = 0;
    }

    /**
     * Revert enemy back to normal
     */
    public void CalmDown()
    {
        mIsScared = false;
        mIsScaredTimer = 0;
        mAnimator.ResetTrigger("blink");

    }

    /**
     * Check whether the enemy has been scared long enough
     */
    void ScaredPolling()
    {
        if (mIsScared)
        {
            mIsScaredTimer += Time.deltaTime;
            if (mIsScaredTimer > mIsScaredDeltaTimer)
            {
                CalmDown();
                GameStateManager.GetInstance().PowerDown();
            }
            else if(mIsScaredDeltaTimer - mIsScaredTimer <= 1)
            {
                mAnimator.SetTrigger("blink");
            }
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
     * The KeyValuePair is used to keep track of the original chosen direction from the first level search.
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

    /**
     * This is used to update the sprite and speed of the enemy.
     * This could've been done using a animator state machine
     * but this was more practical in this case
     */
    void UpdateSpriteAndSpeed()
    {
        if(!mIsAlive)
        {
            mSpriteRenderer.sprite = mSprites[(int)SpriteType.DEAD];
            mSpriteRenderer.color = Color.white;
            mSpeed = mOriginalSpeed * 2.7f;
        }
        else if (mIsScared)
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

    /**
     * Checks and attempts to respawn dead enemy
     * If it's at the respawn point, it can respawn.
     */
    void RespawnIfPossible()
    {
        if (!mIsAlive)
        {
            Vector3Int currentCellPosition = MazeTilemap._MazeTilemap.WorldToCell(transform.position);
            if (currentCellPosition == mSpawnPosition)
            {
                mIsAlive = true;
                CalmDown();
            }
        }
    }

    /**
     * This function is used to evaluate a 1 level deep choice.
     * The goal is to make the enemy run away from the player.
     * This is used to determine a direction if the enemy is scared (player is powered up)
     * to get as far as possible from the player.
     */
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

    /**
     * This returns a one level deep check and current possible directions.
     * It returns a list of possible directions and their evaluation.
     */
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

    /**
     * This is used to evaluate a direction based on the current position;
     * This is based on the distance to the player. The higher the value the better the evaluation.
     */
    float EvaluateDirectionChoice(Direction direction)
    {
        Vector2 directionVector = GetVectorFromDirection(direction);
        directionVector *= mStepDistance;
        Vector2 newPosition = new Vector2(transform.position.x + directionVector.x, transform.position.y + directionVector.y);
        float distanceFromStepToPlayer = Vector2.Distance(mPlayer.transform.position, newPosition);

        return distanceFromStepToPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            if (mIsScared)
            {
                Die();
            }
            else if (mIsAlive)
            {
                mPlayer.Die();
            }
        }
    }

    public override void Die()
    {
        if (mIsAlive)
        {
            mIsAlive = false;
            AddPoints();
            InstantiatePoints(transform);
            CalmDown();
            if (!mAudioSource.isPlaying)
            {
                mAudioSource.Play();
            }
        }
    }

    public override void ResetPawn()
    {
        transform.position = mOriginalPosition;
        mDirection = mOriginalDirection;
        mIsAlive = true;
        CalmDown();
        UpdateSpriteAndSpeed();
    }

    protected override void AddPoints()
    {
        GameStateManager instance = GameStateManager.GetInstance();
        instance.AddPoints(mPoints);
    }
}
