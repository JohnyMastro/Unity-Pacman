using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Inherits Panel Class
 * Manages Pause Panel
 */
public class PausePanel : Panel {
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!mIsPanelActivated || mPanel.activeSelf)
            {
                TogglePauseGame();
            }
        }
    }
}
