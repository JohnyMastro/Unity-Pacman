using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Singleton-like GameManager
 */
public class GameStateManager : MonoBehaviour {
    private static GameStateManager mInstance;
    private int _Score = 0;
    private int _NumOfPellets = 0;

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
        Debug.Log(_NumOfPellets);
		if(_NumOfPellets <= 0)
        {
            //win
            Debug.Log("win");
        }
	}
}
