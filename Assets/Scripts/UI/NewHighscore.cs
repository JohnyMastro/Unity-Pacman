using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is used to handle the input if the player gets a gameover and has a new highscore
 * The sub GameObjects of this class will only appear on the gameover panel if the player gets a new highscore.
 */
public class NewHighscore : MonoBehaviour {
    [SerializeField]
    InputField mInputName;

    CanvasGroup mCanvasGroup;

    // Use this for initialization
    void Start () {
        mCanvasGroup = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateVisibility();
    }
    void UpdateVisibility()
    {
        bool isHighscore = GameStateManager.GetInstance().isNewHighScore();
        mCanvasGroup.alpha = isHighscore ? 1f : 0f;
        mCanvasGroup.interactable = isHighscore;
        mCanvasGroup.blocksRaycasts = isHighscore;
    }

    public void SaveNewHighscore()
    {
        GameStateManager gInstance = GameStateManager.GetInstance();
        gInstance.SaveScore(mInputName.text);
        gInstance.ReinitializeGame();
    }

}
