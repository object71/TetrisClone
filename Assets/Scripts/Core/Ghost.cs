using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Shape ghostShape = null;

    public Color color = new Color(1f, 1f, 1f, 0.2f);

    public void Reset()
    {
        Destroy(ghostShape.gameObject);
    }

    public void DrawGhost(Shape originalShape, Board gameBoard)
    {
        if (!ghostShape)
        {
            ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            ghostShape.gameObject.name = "GhostShape";

            SpriteRenderer[] allRenderers = ghostShape.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in allRenderers)
            {
                renderer.color = color;
            }
        }
        else
        {
            ghostShape.transform.position = originalShape.transform.position;
            ghostShape.transform.rotation = originalShape.transform.rotation;
        }

        bool hitBottom = false;

        while (!hitBottom)
        {
            ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(ghostShape))
            {
                ghostShape.MoveUp();
                hitBottom = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
