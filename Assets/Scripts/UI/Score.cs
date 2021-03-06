﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is used to update and display the player's current score.
 */
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
        mText.text = GameStateManager.GetInstance().mScore.ToString();
    }
}
