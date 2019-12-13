using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform emptySprite;
    public int height = 30;
    public int width = 10;
    public int header = 8;

    Transform[,] grid;

    void Awake()
    {
        grid = new Transform[width, height];
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DrawEmptyCells()
    {
        if (emptySprite)
        {

            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone;
                    clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = $"Board Space ( x = {x.ToString()}, y = {y.ToString()} )";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("Warning! Assign the empty sprite object");
        }
    }
}
