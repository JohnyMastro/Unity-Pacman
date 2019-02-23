using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
