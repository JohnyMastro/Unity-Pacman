using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is used as a serializable object to save the highest score
 */
[System.Serializable]
public class Save
{
    public string name;
    public int score;

    public Save(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
