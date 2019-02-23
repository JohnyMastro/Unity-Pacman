using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour {
    Text mText;
    int mPoints = 0;
    // Use this for initialization
    void Start () {
        mText = GetComponentInChildren<Text>();
        mText.text = mPoints.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyPoints()
    {
        Destroy(gameObject);
    }

    public void SetPoints(int points)
    {
        mPoints = points;
    }
}
