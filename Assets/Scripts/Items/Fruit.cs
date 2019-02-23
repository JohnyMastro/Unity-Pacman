using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectible {
    SpriteRenderer mSpriteRenderer;
    CircleCollider2D mCollider;

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
            int index = UnityEngine.Random.Range(0, 3);
            mSpriteRenderer.sprite = mSprites[index];
            mCollider.enabled = true;
        }
        else
        {
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

        }
    }

    public void ResetTimer()
    {
        mAppearTimer = 0f;
        mHasAppeared = false;
    }
}
