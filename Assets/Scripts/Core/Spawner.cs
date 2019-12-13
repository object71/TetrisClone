using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Shape[] allShapes;

    Shape GetRandomShape()
    {
        int i = Random.Range(0, allShapes.Length);

        if (allShapes[i])
        {
            return allShapes[i];
        }
        else
        {
            Debug.LogWarning("Warning! Invalid shape in spawner.");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;

        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("Warning! Invalid shape in spawner.");
        }

        return shape;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3Int.RoundToInt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
