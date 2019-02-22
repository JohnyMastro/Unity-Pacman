using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
