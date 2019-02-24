using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Singleton-like GameManager
 * This Manages fields regarding the state of the game.
 */
public class GameStateManager : MonoBehaviour {
    internal AudioSource mAudioSourceMusic;
    internal AudioSource mAudioSourcePowerUp;
    private static GameStateManager mInstance;
    private int _Score = 0;
    private int _NumOfPellets = 0;
    private bool _IsPaused = false;
    private int _Lives = 3;
    private Save _LoadedSave;
    internal string mFilePath;

    //Properties
    public Save mLoadedSave
    {
        get { return _LoadedSave; }
    }

    public bool mIsPaused
    {
        get { return _IsPaused; }
        set {
            _IsPaused = value;
            mAudioSourceMusic.bypassEffects = !_IsPaused;
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

    public bool isNewHighScore()
    {
        return mLoadedSave.score < mScore;
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

    public void PelletWasEaten()
    {
        _NumOfPellets--;
    }

    public void AddPelletCount(int numOfPellets)
    {
        _NumOfPellets += numOfPellets;
    }

    /**
     * Get Singleton Instance
     */
    public static GameStateManager GetInstance()
    {
        return mInstance;
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this; //Initialize singleton instance
            mInstance.mFilePath = Application.persistentDataPath + "/Unity-Pacman.save";
            mInstance.LoadScore();
            AudioSource[] audioSources = mInstance.GetComponents<AudioSource>();
            mInstance.mAudioSourceMusic = audioSources[0];
            mInstance.mAudioSourcePowerUp = audioSources[1]; ;
        }
        else if (mInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start () {

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

    public void AddPoints(int points)
    {
        _Score += points;
        if (_Score > 0 && (_Score % 20000 == 0))
        {
            if (_Lives < 3)
            {
                _Lives++;
            }
        }
    }

    /**
     * Scare (notifiy) enemies that the player ate power up
     */
    public void PowerUp()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        
        foreach (Enemy enemy in enemies)
        {
            enemy.Frighten();
        }
        if (!mAudioSourcePowerUp.isPlaying)
        {
            mAudioSourcePowerUp.Play();
        }
    }

    public void PowerDown()
    {
        mAudioSourcePowerUp.Stop();
    }

    /**
     * Used to repopulate pellets
     */
    public void HardReloadScene()
    {
        PowerDown();
        mAudioSourceMusic.Pause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        mAudioSourceMusic.Play();
    }

    /**
      * Used to reset the level when the player losses a life.
      * This preserves the pellet tilemap
      */
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
        PowerDown();

    }
    /**
     * This is used to completely restart the game.
     * This is called after a gameover.
     */
    public void ReinitializeGame()
    {
        _Score = 0;
        _Lives = 3;
        _NumOfPellets = 0;
        HardReloadScene();
    }

    /**
     * Saving new Highscore
     */
    public void SaveScore(string name)
    {
        Save save = new Save(name,mScore);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(mFilePath);
        bf.Serialize(file, save);
        file.Close();
        _LoadedSave = save;

    }

    /**
     * Loading last saved  Highscore
     */
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
