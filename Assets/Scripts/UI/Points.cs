using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is to control Paint Canvas prefabs.
 * When the player collects a large collectible like fruits or a power pellet
 * pointCanvas will appear in the where the where collectible was.
 */ 
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

    /**
     * This is a animation callback.
     * This ensures that the Points Canvas prefab destroys itself once it's done its animation
     */
    public void DestroyPoints()
    {
        Destroy(gameObject);
    }

    public void SetPoints(int points)
    {
        mPoints = points;
    }
}
