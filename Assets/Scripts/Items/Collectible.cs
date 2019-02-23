using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour {
    protected int mPoints = 100;
    protected GameObject mPointsPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    protected void InstantiatePoints(Transform desiredTransform)
    {
        mPointsPrefab = Resources.Load<GameObject>("Prefabs/PointsCanvas");
        if (mPointsPrefab != null)
        {
            GameObject pointsInstance = Instantiate(mPointsPrefab, desiredTransform.position, desiredTransform.rotation) as GameObject;
            Points points = pointsInstance.GetComponent<Points>();
            points.SetPoints(mPoints);
        }
    }
    protected abstract void AddPoints();

}
