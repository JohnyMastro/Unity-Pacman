﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Inherits Panel Class
 * Manages GameOver Panel
 */ 
public class GameOverPanel : Panel {
    [SerializeField]
    CanvasGroup mCanvasGroup;
    Animator mAnimator;

    // Use this for initialization
    void Start () {
        mAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //if no more life: spawn animation
        if (GameStateManager.GetInstance().IsGameOver())
        {
            if (!mIsPanelActivated && !mPanel.activeSelf)
            {
                mAnimator.SetTrigger("fadeIn");
                UpdateInteractable(true);
                GameStateManager.GetInstance().PowerDown();
                Pause();
            }
        }
    }

    public void StartNewGame()
    {
        UpdateInteractable(false);
        mAnimator.ResetTrigger("fadeIn");
        GameStateManager.GetInstance().ReinitializeGame();
    }

    void UpdateInteractable(bool isInteractable)
    {
        mCanvasGroup.interactable = isInteractable;
        mCanvasGroup.blocksRaycasts = isInteractable;
    }
}
