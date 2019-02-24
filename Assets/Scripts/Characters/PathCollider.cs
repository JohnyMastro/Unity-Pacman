using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is used for child GameObjects for pawns that will permit the pawn to moving only in free directions 
 */
public class PathCollider : MonoBehaviour {
    //Here mDirection refers to the orientation of the collider
    public Direction mDirection;
    private bool _isFree = true;

    public bool isFree
    {
        get { return _isFree; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            _isFree = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            _isFree = true;
        }
    }
}
