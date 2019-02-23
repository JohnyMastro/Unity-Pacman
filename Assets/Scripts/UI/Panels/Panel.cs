using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {
    [SerializeField]
    protected GameObject mPanel;

    protected static bool mIsPanelActivated = false;

    // Use this for initialization
    void Start ()
    {
        SetPanel(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void TogglePauseGame()
    {
        bool isPaused = !GameStateManager.GetInstance().mIsPaused;
        if (isPaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    protected void Pause()
    {
        Time.timeScale = 0;
        SetPanel(true);
        GameStateManager.GetInstance().mIsPaused = true;
    }

    protected void Unpause()
    {
        Time.timeScale = 1;
        SetPanel(false);
        GameStateManager.GetInstance().mIsPaused = false;
    }

    protected void SetPanel(bool isActive)
    {
        mIsPanelActivated = isActive;
        mPanel.SetActive(isActive);
    }
}
