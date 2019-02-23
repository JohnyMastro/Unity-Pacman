using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : Panel {
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
                Pause();

            }
        }
    }

    public void StartNewGame()
    {
        mAnimator.ResetTrigger("fadeIn");
        GameStateManager.GetInstance().ReinitializeGame();
    }
}
