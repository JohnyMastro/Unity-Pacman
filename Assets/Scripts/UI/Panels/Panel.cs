using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Panel class is the base class for different types of panels
 * This is used to manage Overlay Panels to the game.
 */
public class Panel : MonoBehaviour {
    [SerializeField]
    protected GameObject mPanel;

    //Used to ensure that there is only one panel active at a time
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

    /**
     * This sets the current panel to be active or not.
     */
    protected void SetPanel(bool isActive)
    {
        mIsPanelActivated = isActive;
        mPanel.SetActive(isActive);
    }
}
