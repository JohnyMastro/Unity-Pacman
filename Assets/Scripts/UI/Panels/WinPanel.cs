﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Inherits Panel Class
 * Manages win Panel
 */
public class WinPanel : Panel {
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.GetInstance().IsGameWon())
        {
            if (!mIsPanelActivated && !mPanel.activeSelf)
            {
                GameStateManager.GetInstance().PowerDown();
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((!mIsPanelActivated || mPanel.activeSelf) && GameStateManager.GetInstance().IsGameWon())
            {
                GameStateManager.GetInstance().HardReloadScene();
            }
        }
    }
}
