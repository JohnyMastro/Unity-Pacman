using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Singleton-like GameManager
 */
public class GameStateManager : MonoBehaviour {
    private static GameStateManager mInstance;
    private int _Score = 0;
    private int _NumOfPellets = 0;
    private bool _IsPaused = false;
    private int _Lives = 3;

    public bool mIsPaused
    {
        get { return _IsPaused; }
        set { _IsPaused = value; }
    }

    public int mLives
    {
        get { return _Lives; }
    }

    public int mScore
    {
        get { return _Score;}
    }

    public int mNumOfPellets
    {
        get { return _NumOfPellets; }
    }

    public static GameStateManager GetInstance()
    {
        return mInstance;
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
        else if (mInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int points)
    {
        _Score += points;
    }

    public void PelletWasEaten()
    {
        _NumOfPellets--;
    }

    public void AddPelletCount(int numOfPellets)
    {
        _NumOfPellets += numOfPellets;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(_NumOfPellets);
		if(_NumOfPellets <= 0)
        {
            //win
            Debug.Log("win");
        }
	}

    public bool IsGameOver()
    {
        return _Lives <= 0;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseLife()
    {
        _Lives--;
    }
    public void ReinitializeGame()
    {
        _Score = 0;
        _Lives = 3;
        ReloadScene();
    }
}
