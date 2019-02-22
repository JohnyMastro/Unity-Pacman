using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    [SerializeField]
    GameObject mPausePanel;

    void Start()
    {
        mPausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("pause");
            TogglePauseGame();
        }
    }

    void TogglePauseGame()
    {
        bool isPaused = !GameStateManager.GetInstance().mIsPaused;
        GameStateManager.GetInstance().mIsPaused = isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            mPausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            mPausePanel.SetActive(false);
        }
    }
}
