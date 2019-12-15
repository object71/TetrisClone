using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Board gameBoard;
    private Spawner spawner;
    private SoundManager soundManager;
    private FxManager fxManager;
    private Ghost ghost;
    private ScoreManager scoreManager;
    private Shape activeShape;

    // The interval at which the shapes drop
    private float dropIntervalInitial = 0.75f;
    private float dropIntervalCurrent;
    private float timeToDrop = 0f;

    // the update speed for moving left or right
    private float leftRightKeyRepeatRate = 0.15f;
    private float timeToNextLeftRightKey = 0f;

    // the update speed for rotating
    private float rotateKeyRepeatRate = 0.1f;
    private float timeToNextRotateKey = 0f;

    // the update speed for moving doown
    private float downKeyRepeatRate = 0.05f;
    private float timeToNextDownKey = 0f;

    private bool gameIsOver = false;

    private bool isClockwiseRotation = true;

    public RotationToggledEvent rotationToggled = new RotationToggledEvent();

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject[] overlays;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        fxManager = GameObject.FindObjectOfType<FxManager>();
        ghost = GameObject.FindObjectOfType<Ghost>();

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }

        foreach (GameObject overlay in overlays)
        {
            overlay.SetActive(true);
        }

        if (!gameBoard)
        {
            Debug.LogWarning("Warning! There is no game board!");
        }
        else
        {
            gameBoard.rowsClearedEvent.AddListener(OnRowsCleared);
        }

        if (!soundManager)
        {
            Debug.LogWarning("Warning! There is no sound manager!");
        }

        if (!fxManager)
        {
            Debug.LogWarning("Warning! There is no fx manager!");
        }

        if (!ghost)
        {
            Debug.LogWarning("Warning! There is no ghost!");
        }

        if (!scoreManager)
        {
            Debug.LogWarning("Warning! There is no score manager!");
        }
        else
        {
            scoreManager.levelUpEvent.AddListener(OnLevelUp);
        }

        if (!spawner)
        {
            Debug.LogWarning("Warning! There is no spawner!");
        }
        else
        {
            if (activeShape == null)
            {
                activeShape = spawner.SpawnShape();
            }
        }

        dropIntervalCurrent = dropIntervalInitial;

        rotationToggled.Invoke(isClockwiseRotation);
    }

    private void OnLevelUp(int level)
    {
        if (soundManager)
        {
            soundManager.PlaySound("LevelUpVocal");
        }

        dropIntervalCurrent = Mathf.Clamp(dropIntervalInitial - (((float)level - 1) * 0.1f), 0.05f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameBoard || !spawner || !activeShape || gameIsOver || !soundManager || !scoreManager || !fxManager)
        {
            return;
        }

        ActOnPlayerInput();

        if (Time.time > timeToDrop)
        {
            MoveActiveShapeDown();
        }

    }

    private void MoveActiveShapeDown()
    {
        timeToDrop = Time.time + dropIntervalCurrent;
        if (activeShape)
        {
            activeShape.MoveDown();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveUp();

                if (gameBoard.IsOverLimit(activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }
    }

    private void GameOver()
    {
        gameIsOver = true;

        soundManager.PlaySound("GameOver");
        soundManager.PlaySound("GameOverVocal");

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void LandShape()
    {
        foreach (Transform child in activeShape.transform)
        {
            Vector3Int position = Vector3Int.RoundToInt(child.position);
            fxManager.PlayGlowFx(position.x, position.y);
        }

        gameBoard.StoreShapeInGrid(activeShape);

        if (ghost)
        {
            ghost.Reset();
        }

        activeShape = spawner.SpawnShape();
        fxManager.PlaySpawnFx();

        timeToNextDownKey = Time.time;
        timeToNextLeftRightKey = Time.time;
        timeToNextRotateKey = Time.time;

        gameBoard.ClearAllRows();

        soundManager.PlaySound("Drop");
    }

    private void ActOnPlayerInput()
    {
        if ((Input.GetButton("MoveRight") && Time.time > timeToNextLeftRightKey) || Input.GetButtonDown("MoveRight"))
        {
            timeToNextLeftRightKey = Time.time + leftRightKeyRepeatRate;

            activeShape.MoveRight();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveLeft();
                soundManager.PlaySound("Error");
            }
            else
            {
                soundManager.PlaySound("Move");
            }
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > timeToNextLeftRightKey) || Input.GetButtonDown("MoveLeft"))
        {
            timeToNextLeftRightKey = Time.time + leftRightKeyRepeatRate;

            activeShape.MoveLeft();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveRight();
                soundManager.PlaySound("Error");
            }
            else
            {
                soundManager.PlaySound("Move");
            }
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > timeToNextRotateKey)
        {
            timeToNextRotateKey = Time.time + rotateKeyRepeatRate;

            activeShape.Rotate(isClockwiseRotation);

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.Rotate(!isClockwiseRotation);
                soundManager.PlaySound("Error");
            }
            else
            {

                soundManager.PlaySound("Move");
            }
        }
        else if (Input.GetButton("MoveDown") && Time.time > timeToNextDownKey)
        {
            timeToNextDownKey = Time.time + downKeyRepeatRate;
            MoveActiveShapeDown();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void OnRowsCleared(int n, int startY)
    {
        soundManager.PlaySound("ClearRow");
        soundManager.PlaySound($"Cleared{n}Vocal");

        for (int y = startY; y < startY + n; y++)
        {
            for (int x = 0; x < gameBoard.width; x++)
            {
                fxManager.PlayGlowFx(x, y);
            }
        }

        scoreManager.OnLinesCleared(n);
    }

    void LateUpdate()
    {
        if (ghost)
        {
            ghost.DrawGhost(activeShape, gameBoard);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        if (pausePanel)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void ToggleRotation()
    {
        isClockwiseRotation = !isClockwiseRotation;
        rotationToggled.Invoke(isClockwiseRotation);
    }

    public void TogglePause()
    {
        if (gameIsOver)
        {
            return;
        }

        isPaused = !isPaused;

        if (pausePanel)
        {
            pausePanel.SetActive(isPaused);
            soundManager.musicSource.volume = isPaused ? soundManager.musicVolume * 0.25f : soundManager.musicVolume;
        }

        Time.timeScale = isPaused ? 0 : 1;
    }
}
