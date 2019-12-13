using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board gameBoard;
    Spawner spawner;

    Shape activeShape;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();

        if (spawner)
        {
            if (activeShape == null)
            {
                activeShape = spawner.SpawnShape();
            }
        }

        if (!gameBoard)
        {
            Debug.LogWarning("Warning! There is no game board!");
        }

        if (!spawner)
        {
            Debug.LogWarning("Warning! There is no spawner!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameBoard || !spawner)
        {
            return;
        }



    }
}
