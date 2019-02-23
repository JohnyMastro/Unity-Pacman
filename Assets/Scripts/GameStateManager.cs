using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Singleton-like GameManager
 */
public class GameStateManager : MonoBehaviour {
    [SerializeField]
    AudioClip mMainClip;

    AudioSource mAudioSource;
    AudioHighPassFilter mHighPassFilter;
    private static GameStateManager mInstance;
    private int _Score = 0;
    private int _NumOfPellets = 0;
    private bool _IsPaused = false;
    private int _Lives = 3;
    private Save _LoadedSave;
    internal string mFilePath;

    public Save mLoadedSave
    {
        get { return _LoadedSave; }
    }

    public bool mIsPaused
    {
        get { return _IsPaused; }
        set {
            _IsPaused = value;
            mHighPassFilter.enabled = _IsPaused;
            }
    }

    public int mLives
    {
        get { return _Lives; }
    }

    public int mScore
    {
        get { return _Score;}
    }

    public int mNumOfPellets
    {
        get { return _NumOfPellets; }
    }

    public static GameStateManager GetInstance()
    {
        return mInstance;
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
            mInstance.mFilePath = Application.persistentDataPath + "/Unity-Pacman.save";
            mInstance.LoadScore();
        }
        else if (mInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int points)
    {
        _Score += points;
    }

    public void PelletWasEaten()
    {
        _NumOfPellets--;
    }

    public void AddPelletCount(int numOfPellets)
    {
        _NumOfPellets += numOfPellets;
    }

    // Use this for initialization
    void Start () {
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.clip = mMainClip;
        mAudioSource.Play();
        mHighPassFilter = GetComponent<AudioHighPassFilter>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _Lives = 0;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            _NumOfPellets=0;
        }
    }

    public bool IsGameWon()
    {
        return _NumOfPellets <= 0;
    }

    public bool IsGameOver()
    {
        return _Lives <= 0;
    }

    public void LoseLife()
    {
        _Lives--;
    }

    /*
     * Scare (notifiy) enemies that the player ate power up
     */
    public void PowerUp()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        
        foreach (Enemy enemy in enemies)
        {
            enemy.Frighten();
        }
    }

    public void HardReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetLevelOnPlayerDeath()
    {
        Pawn[] pawns = FindObjectsOfType<Pawn>();

        foreach(Pawn pawn in pawns)
        {
            pawn.ResetPawn();
        }

        Fruit fruit = FindObjectOfType<Fruit>();
        fruit.ResetTimer();

        FindObjectOfType<StartPanel>().Reinitialize();
    }

    public void ReinitializeGame()
    {
        _Score = 0;
        _Lives = 3;
        HardReloadScene();
    }

    public bool isNewHighScore()
    {
        return mLoadedSave.score < mScore;
    }

    public void SaveScore(string name)
    {
        Save save = new Save(name,mScore);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(mFilePath);
        bf.Serialize(file, save);
        file.Close();
        _LoadedSave = save;

    }

    public void LoadScore()
    {
        if (File.Exists(mFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mFilePath, FileMode.Open);
            _LoadedSave = (Save)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            _LoadedSave = new Save("",0);
        }
    }
}
