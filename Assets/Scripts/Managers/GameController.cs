using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Board gameBoard;
    private Spawner spawner;
    private Shape activeShape;

    // The interval at which the shapes drop
    private float dropInterval = 0.75f;
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

    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        if (!gameBoard)
        {
            Debug.LogWarning("Warning! There is no game board!");
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameBoard || !spawner || !activeShape || gameIsOver)
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
        timeToDrop = Time.time + dropInterval;
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

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void LandShape()
    {
        gameBoard.StoreShapeInGrid(activeShape);
        activeShape = spawner.SpawnShape();

        timeToNextDownKey = Time.time;
        timeToNextLeftRightKey = Time.time;
        timeToNextRotateKey = Time.time;

        gameBoard.ClearAllRows();
    }

    private void ActOnPlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > timeToNextLeftRightKey || Input.GetButtonDown("MoveRight"))
        {
            timeToNextLeftRightKey = Time.time + leftRightKeyRepeatRate;

            activeShape.MoveRight();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveLeft();
            }
        }
        else if (Input.GetButton("MoveLeft") && Time.time > timeToNextLeftRightKey || Input.GetButtonDown("MoveLeft"))
        {
            timeToNextLeftRightKey = Time.time + leftRightKeyRepeatRate;

            activeShape.MoveLeft();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveRight();
            }
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > timeToNextRotateKey)
        {
            timeToNextRotateKey = Time.time + rotateKeyRepeatRate;

            activeShape.RotateRight();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && Time.time > timeToNextDownKey)
        {
            timeToNextDownKey = Time.time + downKeyRepeatRate;
            MoveActiveShapeDown();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
    }
}
