using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maze : MonoBehaviour {

    [SerializeField]
    int width;
    [SerializeField]
    int height;

    [SerializeField]
    int scaleX;
    [SerializeField]
    int scaleY;

    [SerializeField]
    float stepHeight;

    public GameObject ground;
    public GameObject wall;
    public GameObject player;

    public NavMeshSurface surface;

    float[,] map;
	
    public void InitMap()
    {
        map = new float[height, width];

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                map[y, x] = Mathf.PerlinNoise(x + 0.1f, y + 0.1f);
            }
        }
    }

	public void SetMeshes()
    {
        GameObject cloneGround = Instantiate(ground,transform);
        cloneGround.transform.localScale = new Vector3(height * scaleY, 0, width * scaleX);

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if(map[y,x] >= stepHeight)
                {
                    float coordX = (x - width / 2) * wall.transform.localScale.z + wall.transform.localScale.z / 2;
                    float coordY = (y - height / 2) * wall.transform.localScale.x + wall.transform.localScale.x / 2;
                    Instantiate(wall, new Vector3(coordY, wall.transform.position.y, coordX), wall.transform.localRotation, transform);
                }
            }
        }
        surface.BuildNavMesh();
        Instantiate(player);
    }
}
