﻿using System.Collections;
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
    
    Vector3 startPosition;
    Vector3 endPosition;

    public GameObject ground;
    public GameObject wall;
    public GameObject player;

    public NavMeshSurface surface;

    int[,] map;
    Map[,] wallMap;
    bool[,] visited;

    bool CoordValid(int x, int y)
    {
        return x > -1 && y > -1 && x < width && y < height;
    }

    int[][] SetDirections()
    {
        int[][] directions = new int[4][];
        //Nord
        directions[0] = new int[2];
        directions[0][0] = 0;
        directions[0][1] = -1;
        //Sud
        directions[1] = new int[2];
        directions[1][0] = 0;
        directions[1][1] = 1;
        //Est
        directions[2] = new int[2];
        directions[2][0] = 1;
        directions[2][1] = 0;
        //Ouest
        directions[3] = new int[2];
        directions[3][0] = -1;
        directions[3][1] = 0;

        return directions;
    }

    int CarvePassageFrom(int x, int y)
    {
        if (y != 0 && x != 0 && Vector3.Distance(startPosition, endPosition) < Vector3.Distance(startPosition, new Vector3(y * scaleY, startPosition.y, x * scaleX)))
            endPosition = new Vector3(y * scaleY, 0, x * scaleX);
        visited[y, x] = true;
        int v = 1;

        int[][] directions = SetDirections();

        while (directions.Length > 0)
        {
            int dir = Random.Range(0, directions.Length);
            int cx = directions[dir][0];
            int cy = directions[dir][1];

            if (CoordValid(x + cx, y + cy) && !visited[y + cy, x + cx])
            {
                wallMap[y, x].DestroyWall(cx, cy);
                wallMap[y + cy, x + cx].DestroyWall(-cx, -cy);
                v += CarvePassageFrom(x + cx, y + cy);
            }
            var tmp = new List<int[]>(directions);
            tmp.RemoveAt(dir);
            directions = tmp.ToArray();
        }
        return v;
    }

    public void InitMap()
    {
        map = new int[height, width];
        wallMap = new Map[height, width];
        visited = new bool[height, width];
        startPosition = new Vector3(10, 0, 10);
        endPosition = startPosition;

        int i = 0;

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                map[y, x] = i;
                wallMap[y, x] = new Map();
                visited[y, x] = false;
                i += 1;
            }
        }

        int startx = 0;
        int starty = 0;

        CarvePassageFrom(startx, starty);
    }

	public void SetMeshes()
    {
        GameObject cloneGround = Instantiate(ground, new Vector3(-wall.transform.localScale.x, 0 , -wall.transform.localScale.z), Quaternion.identity, transform);
        cloneGround.transform.localScale = new Vector3(height * scaleY * 2 - wall.transform.localScale.x, 0, width * scaleX * 2 - wall.transform.localScale.z);

        for (int y = 0; y < height * 2; y++)
        {
            if (y % 2 != 0)
                continue;
            for (int x = 0; x < width * 2; ++x)
            {
                if (x % 2 != 0)
                    continue;
                float cx = (x - width) * wall.transform.localScale.z;
                float cy = (y - height) * wall.transform.localScale.x;
                if (wallMap[y / 2, x / 2].WallRight)
                {
                    GameObject w = Instantiate(wall, new Vector3(cy, wall.transform.position.y, cx), wall.transform.localRotation, transform);
                }
                if (y - 1 > 0 && wallMap[y / 2, x / 2].WallUp)
                {
                    cx = (x - width) * wall.transform.localScale.z;
                    cy = (y - 1 - height) * wall.transform.localScale.x;
                    GameObject w = Instantiate(wall, new Vector3(cy, wall.transform.position.y, cx), wall.transform.localRotation, transform);
                }
            }
        }
        for (int x = 0; x < width * 2; x++)
        {
            if (x % 2 != 0)
                continue;
            for (int y = 0; y < height * 2; ++y)
            {
                if (y % 2 != 0)
                    continue;
                float cx = (x - width) * wall.transform.localScale.z;
                float cy = (y - height) * wall.transform.localScale.x;
                if (wallMap[y / 2, x / 2].WallDown)
                {
                    GameObject w = Instantiate(wall, new Vector3(cy, wall.transform.position.y, cx), wall.transform.localRotation, transform);
                }
                if (x - 1 > 0 && wallMap[y / 2, x / 2].WallLeft)
                {
                    cx = (x - 1 - width) * wall.transform.localScale.z;
                    cy = (y - height) * wall.transform.localScale.x;
                    GameObject w = Instantiate(wall, new Vector3(cy, wall.transform.position.y, cx), wall.transform.localRotation, transform);
                }
            }
        }

        surface.BuildNavMesh();
        GameObject p = Instantiate(player);
        p.transform.position = startPosition;
        p.GetComponent<BoyBehaviour>().Init();
        Debug.Log(endPosition);
        p.GetComponent<BoyBehaviour>().MoveToDestination(endPosition);
    }

    public class Map
    {
        public bool WallUp;
        public bool WallDown;
        public bool WallLeft;
        public bool WallRight;

        public Map()
        {
            WallUp = true;
            WallDown = true;
            WallLeft = true;
            WallRight = true;
        }

        public void DestroyWall(int Xoffset, int Yoffset)
        {
            if (Xoffset < 0)
                WallLeft = false;
            if (Xoffset > 0)
                WallRight = false;
            if (Yoffset < 0)
                WallUp = false;
            if (Yoffset > 0)
                WallDown = false;
        }
    }
}
