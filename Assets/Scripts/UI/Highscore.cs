using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {
    [SerializeField]
    Text mTextScore;
    [SerializeField]
    Text mTextName;

    // Use this for initialization
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        GameStateManager gInstance  = GameStateManager.GetInstance();
        if (gInstance.mLoadedSave.name.Length > 0) {
            mTextScore.text = gInstance.mLoadedSave.score.ToString();
            mTextName.text = gInstance.mLoadedSave.name;
        }
        else
        {
            mTextScore.text ="";
            mTextName.text = "No Highscore";
        }
    }
}
