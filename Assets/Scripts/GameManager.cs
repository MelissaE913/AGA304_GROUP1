using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public enum GamePhases
    {
        StartPhase, LevelSelect, PlayPhase, TutorialPlayPhase, OptionsPhase, ResultsPhase
    }

    public GamePhases currentGamePhase = GamePhases.StartPhase;


    #region RHYTHM GAME VARIABLES
    public int currentScore;
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;
    public int scorePerNote = 100;
    public int scorePerPerfectNote = 150;
    public int scorePerGoodNote = 125;

    //TODO: Make something in rhythm scene get updated text values
    //public Text scoreText;
    //public Text multiText;
    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCurrentPhaseBehavior();
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentGamePhase)
        {
            case GamePhases.PlayPhase:
            case GamePhases.TutorialPlayPhase:
                if (Input.anyKeyDown)
                {
                    BeatScroller theBS = GameObject.FindObjectOfType<BeatScroller>();
                    if (theBS != null)
                    {
                        if (theBS.hasStarted == false)
                        {
                            theBS.hasStarted = true;

                            GameObject musicObject = GameObject.FindGameObjectWithTag("MusicAudioSource");
                            if (musicObject)
                            {
                                AudioSource theMusic = musicObject.GetComponent<AudioSource>();
                                if (theMusic != null) { theMusic.Play(); }
                            }
                        }
                    }

                }
                break;
            case GamePhases.StartPhase:
                break;
            case GamePhases.LevelSelect:
                break;
            case GamePhases.ResultsPhase:
                break;
        }
    }
    public void SetNextPhase(GamePhases nextPhase)
    {
        EndCurrentPhaseBehavior();
        currentGamePhase = nextPhase;
        StartCurrentPhaseBehavior();
    }

    void StartCurrentPhaseBehavior()
    {
        switch (currentGamePhase)
        {
            case GamePhases.StartPhase:
                //load data
                //get microtransactions

                if (SceneManager.GetActiveScene().buildIndex != 0)
                    SceneManager.LoadScene(0);

                OnUnPaused();
                break;
            case GamePhases.PlayPhase:
                SceneManager.LoadScene(4);
                //shouldn't play music until the player hits a key
                //Debug.Log("Play the music!");
                //AudioManager.instance.PlayMusic(AudioManager.MusicTypes.Gameplay, true);

                OnUnPaused();
                break;
            case GamePhases.LevelSelect:
                SceneManager.LoadScene(1);
                break;
            case GamePhases.TutorialPlayPhase:
                SceneManager.LoadScene(3);
                break;
            case GamePhases.OptionsPhase:
                SceneManager.LoadScene(2);
                break;
            case GamePhases.ResultsPhase:
                SceneManager.LoadScene(5);
                break;
        }
    }
    void EndCurrentPhaseBehavior()
    {
        switch (currentGamePhase)
        {
            case GamePhases.StartPhase:
                break;
            case GamePhases.PlayPhase:
                break;
        }
    }

    public void OnStartGamePressed()
    {
        SetNextPhase(GamePhases.PlayPhase);
    }

    public void OnPlayQuitPressed()
    {
        SetNextPhase(GamePhases.StartPhase);
    }

    public void OnPlayExitPressed()
    {
        SetNextPhase(GamePhases.LevelSelect);
    }

    public void OnPlayOptionsPressed()
    {
        SetNextPhase(GamePhases.OptionsPhase);
    }

    public void OnLevelComplete()
    {
        SetNextPhase(GamePhases.ResultsPhase);
    }


    public void OnPaused()
    {
        Time.timeScale = 0f;
    }
    public void OnUnPaused()
    {
        Time.timeScale = 1f;
    }

    #region RHYTHM GAME METHODS
    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        currentScore += scorePerNote * currentMultiplier;
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
        //TODO - Make something in the rhythm game scene receive update
        //multiText.text = "Multiplier: x" + currentMultiplier;
        //scoreText.text = "Score: " + currentScore;
    }
    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        //TODO - Make something in the rhythm game scene receive update
        //multiText.text = "Multiplier: x" + currentMultiplier;
    }
    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
    }
    public void GoodHit()
    {
        currentScore += scorePerNote * currentMultiplier;
    }
    public void PerfectHit()
    {
        currentScore += scorePerNote * currentMultiplier;
    }
    #endregion
}
