﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Inherits Panel Class
 * Manages start Panel
 */
public class StartPanel : Panel {
    void Start()
    {
        Pause();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!mIsPanelActivated || mPanel.activeSelf)
            {
                Unpause();
            }
        }
    }

    public void Reinitialize()
    {
        if (!mIsPanelActivated || mPanel.activeSelf)
        {
            Unpause();
        }
        Pause();
    }
}
