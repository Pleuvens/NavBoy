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

    int[,] map;
    Map[,] wallMap;
    bool[,] visited;

    bool CoordValid(int x, int y)
    {
        return x > -1 && y > -1 && x < width && y < height;
    }

    int CarvePassageFrom(int x, int y)
    {
        visited[y, x] = true;
        int v = 1;
        int[][] directions = new int[4][];
        int[][] opposites = new int[4][];
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

        while (directions.Length > 0)
        {
            int dir = Random.Range(0, directions.Length);
            int cx = directions[dir][0];
            int cy = directions[dir][1];

            if (CoordValid(x + cx, y + cy) && !visited[y + cy, x + cx])
            {
                Debug.Log("Destroying wall.");
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
        Debug.Log("Initing map data structure... ");
        map = new int[height, width];
        wallMap = new Map[height, width];
        visited = new bool[height, width];

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

        Debug.Log("Setting the walls...");

        int startx = 0;
        int starty = 0;

        int allvisited = width * height;
        int casevisited = 0;
        
        while (casevisited < allvisited) {
            bool found = false;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (!visited[y,x])
                    {
                        startx = x;
                        starty = y;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
            casevisited += CarvePassageFrom(startx, starty);
        }
        
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
                float cx = (x - width) * wall.transform.localScale.z / 2;
                float cy = (y - height) * wall.transform.localScale.x;
                if (wallMap[y / 2, x / 2].WallRight)
                {
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
                float cy = (y - height) * wall.transform.localScale.x / 2;
                if (wallMap[y / 2, x / 2].WallDown)
                {
                    GameObject w = Instantiate(wall, new Vector3(cy, wall.transform.position.y, cx), wall.transform.localRotation, transform);
                }
            }
        }
        /*
        if (y % 2 != 0)
        {
            continue;
        }
        for (int x = 2; x < width * 2 - 4; x++)
        {
            if (x % 2 != 0 || !CoordValid(x, y))
            {
                continue;
            }
            float coordX = 0;
            float coordY = 0;
            if (wallMap[y / 2, x / 2].WallUp)
            {
                coordX = ((x - width / 2) * wall.transform.localScale.z);
                coordY = ((y - 1 - height / 2) * wall.transform.localScale.x);
                GameObject w = Instantiate(wall, new Vector3(coordY, wall.transform.position.y, coordX), wall.transform.localRotation, transform);
            }
            if (wallMap[y / 2, x / 2].WallDown)
            {
                coordX = ((x - width / 2) * wall.transform.localScale.z);
                coordY = ((y + 1 - height / 2) * wall.transform.localScale.x);
                GameObject w = Instantiate(wall, new Vector3(coordY, wall.transform.position.y, coordX), wall.transform.localRotation, transform);
            }
            if (wallMap[y / 2, x / 2].WallLeft)
            {
                coordX = ((x - 1 - width / 2) * wall.transform.localScale.z);
                coordY = ((y - height / 2) * wall.transform.localScale.x);
                GameObject w = Instantiate(wall, new Vector3(coordY, wall.transform.position.y, coordX), wall.transform.localRotation, transform);
            }
            if (wallMap[y / 2, x / 2].WallRight)
            {
                coordX = ((x + 1 - width / 2) * wall.transform.localScale.z);
                coordY = ((y - height / 2) * wall.transform.localScale.x);
                GameObject w = Instantiate(wall, new Vector3(coordY, wall.transform.position.y, coordX), wall.transform.localRotation, transform);
            }
        }*/
        surface.BuildNavMesh();
        Instantiate(player);
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
