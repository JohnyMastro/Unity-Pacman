using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesCounter : MonoBehaviour {
    [SerializeField]
    GameObject[] mLifeImages;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        UpdateLifeCounter();
    }

    void UpdateLifeCounter()
    {
        int lives = GameStateManager.GetInstance().mLives;
        mLifeImages[0].SetActive(true);
        mLifeImages[1].SetActive(true);
        mLifeImages[2].SetActive(true);

        switch (lives)
        {
            case 0:
                mLifeImages[0].SetActive(false);
                goto case 1;
            case 1:
                mLifeImages[1].SetActive(false);
                goto case 2;
            case 2:
                mLifeImages[2].SetActive(false);
                break;
        }
    }
}
