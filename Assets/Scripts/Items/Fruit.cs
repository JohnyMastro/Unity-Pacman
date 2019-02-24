using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class inherits from Collectible.
 * This is a self containing system where fruits only interact with the player.
 * It will appear and disappear based on time and can provide the player extra points
 */
public class Fruit : Collectible {
    SpriteRenderer mSpriteRenderer;
    CircleCollider2D mCollider;
    AudioSource mAudioSource;

    [SerializeField]
    Sprite[] mSprites;

    float mAppearTimer = 0;
    float mAppearTimerDetla = 10f;
    bool mHasAppeared = false;

	// Use this for initialization
	void Start () {
        mPoints = 500;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mCollider = GetComponent<CircleCollider2D>();
        mAudioSource = GetComponent<AudioSource>();
        Appear(false);
    }
	
	// Update is called once per frame
	void Update () {
        AppearIfPossible();
    }

    void AppearIfPossible()
    {
        mAppearTimer += Time.deltaTime;
        if (mAppearTimer > mAppearTimerDetla)
        {
            Appear(!mHasAppeared);
            mAppearTimer = 0f;
        }

    }

    void Appear(bool shouldAppear)
    {
        mHasAppeared = shouldAppear;
        if (shouldAppear)
        {
            //show fruit and make it interactable
            int index = UnityEngine.Random.Range(0, 3);
            mSpriteRenderer.sprite = mSprites[index];//Randomize type of fruit
            mCollider.enabled = true;
        }
        else
        {
            //hide fruit
            mSpriteRenderer.sprite = null;
            mCollider.enabled = false;
        }
    }

    protected override void AddPoints()
    {
        GameStateManager.GetInstance().AddPoints(mPoints);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            Appear(false);
            AddPoints();
            InstantiatePoints(transform);
            if (!mAudioSource.isPlaying)
            {
                mAudioSource.Play();
            }

        }
    }

    public void ResetTimer()
    {
        mAppearTimer = 0f;
        mHasAppeared = false;
        Appear(false);
    }
}
