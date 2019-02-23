using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
