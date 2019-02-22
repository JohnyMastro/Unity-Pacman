using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text mText;

    // Use this for initialization
    void Start () {
        mText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        UpdateText();
    }

    public void UpdateText()
    {
       // Debug.Log(GameStateManager.GetInstance().mScore.ToString());
        mText.text = GameStateManager.GetInstance().mScore.ToString();
    }
}
