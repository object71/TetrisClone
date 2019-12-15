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

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(child.position);

            if (!IsWithinBoard(position.x, position.y))
            {
                return false;
            }

            if (IsOccupied(position.x, position.y, shape))
            {
                return false;
            }
        }

        return true;
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(child.position);
            grid[position.x, position.y] = child;
        }
    }

    private bool IsWithinBoard(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsOccupied(int x, int y, Shape shape)
    {
        return (grid[x, y] != null && grid[x, y].parent != shape.transform);
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

    bool IsComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Transform parent = null;
            parent = grid[x, y].parent;

            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;

            if (parent.childCount == 0)
            {
                Destroy(parent);
            }
        }

    }

    private void ShiftOneRowDown(int y)
    {
        if (y == 0)
        {
            return;
        }

        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < height; i++)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows()
    {
        int y = 0;
        while (y < height)
        {
            if (IsComplete(y))
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
            }
            else
            {
                y++;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (height - header - 1))
            {
                return true;
            }
        }

        return false;
    }
}
