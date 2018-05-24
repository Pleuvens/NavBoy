using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maze : MonoBehaviour {

    public uint width;
    public uint height;
    public uint xStart;
    public uint yStart;
    private uint xEnd;
    private uint yEnd;
    private int scaleX = 4;
    private int scaleY = 4;
    private Cell[,] maze;
    private Vector2 posStart;
    public GameObject player;
    public GameObject wall;
    public GameObject ground;
    public GameObject end;
    public NavMeshSurface surface;


    // Use this for initialization
    public void InitMap()
    {
        xEnd = (uint)Random.Range(0, width);
        yEnd = (uint)Random.Range(0, height);
        maze = new Cell[width, height];
        posStart = new Vector2(xStart, yStart);
        for (uint x = 0; x < width; x++)
            for (uint y = 0; y < height; y++)
            {
                if (x == xStart && y == yStart)
                    maze[x, y] = new Cell(Type.START);
                else if (x == xEnd && y == yEnd)
                    maze[x, y] = new Cell(Type.END);
                else
                    maze[x, y] = new Cell();
            }
        generate();
        PrintMaze();
    }

    private bool verticalWall(uint x1, uint x2, uint y)
    {
        if (x2 < width && x1 < width && x2 >= 0 && x1 >= 0 && maze[x1, y].isLinked((uint)x2, y))
            return false;

        return true;
    }

    private bool horizontalWall(uint x, uint y1, uint y2)
    {
        if (y2 < height && y1 < height && y2 >= 0 && y1 >= 0 && maze[x, y1].isLinked(x, (uint)y2))
            return false;

        return true;
    }

    private void PrintMaze()
    {
        int count = 32;

        GameObject G = Instantiate(ground, new Vector3(width * scaleX / 2 - 1, 0, height * scaleY / 2 - 1), ground.transform.rotation, transform);
        G.transform.localScale = new Vector3(G.transform.localScale.x * scaleX, 0, G.transform.localScale.z * scaleY);
        Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, -1), wall.transform.rotation, transform);

        for (uint x = 0; x < width * 4; x++)
        {
            Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, x), wall.transform.rotation, transform);
        }

        for (uint y = 0; y < height; y++)
        {
            Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, y), wall.transform.rotation, transform);
            Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, y * scaleY - 1), wall.transform.rotation, transform);
            Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, y * scaleY + 2), wall.transform.rotation, transform);
            Instantiate(wall, new Vector3(-1, wall.transform.localScale.y / 2, y * scaleY + 3), wall.transform.rotation, transform);


            for (uint x = 0; x < width; x++)
            {
                if (verticalWall(x, x + 1, y))
                {
                    Instantiate(wall, new Vector3(x * scaleX + 3, wall.transform.localScale.y / 2, y * scaleY + 1), wall.transform.rotation, transform);
                    Instantiate(wall, new Vector3(x * scaleX + 3, wall.transform.localScale.y / 2, y * scaleY), wall.transform.rotation, transform);
                    Instantiate(wall, new Vector3(x * scaleX + 3, wall.transform.localScale.y / 2, y * scaleY + 2), wall.transform.rotation, transform);
                }
                if (horizontalWall(x, y, y + 1))
                {
                    Instantiate(wall, new Vector3(x * scaleX + 1, wall.transform.localScale.y / 2, y * scaleY + 3), wall.transform.rotation, transform);
                    Instantiate(wall, new Vector3(x * scaleX + 2, wall.transform.localScale.y / 2, y * scaleY + 3), wall.transform.rotation, transform);
                    Instantiate(wall, new Vector3(x * scaleX, wall.transform.localScale.y / 2, y * scaleY + 3), wall.transform.rotation, transform);
                }
                Instantiate(wall, new Vector3(x * scaleX + 3, wall.transform.localScale.y / 2, y * scaleY + 3), wall.transform.rotation, transform);
            }
        }
        
        surface.BuildNavMesh();
        GameObject E = Instantiate(end, new Vector3((xEnd * scaleX + 2), 0, (yEnd * scaleY + 1)), end.transform.rotation, transform);
        GameObject P = Instantiate(player, new Vector3((xStart * scaleX + 2), 0, (yStart * scaleY + 1)), player.transform.rotation);
        P.GetComponent<BoyBehaviour>().Init();
        P.GetComponent<BoyBehaviour>().MoveToDestination(E.transform.position);
    }

    public void generate()
    {
        uint x = (uint)Random.Range(0, (int)width);
        uint y = (uint)Random.Range(0, (int)height);

        generateRec(x, y);
    }
    private void generateRec(uint x, uint y)
    {
        if (maze[x, y].getVisited())
            return;

        maze[x, y].setVisited(true);
        if (maze[x, y].getType() == Type.START || maze[x, y].getType() == Type.END)
            return;
        List<Vector2> n = getNotVisitedNeighbor(x, y);
        while (n.Count != 0)
        {
            int i = Random.Range(0, n.Count);
            Vector2 t = n[i];
            maze[x, y].addLink(t);
            maze[(uint)t.x, (uint)t.y].addLink(new Vector2(x, y));
            generateRec((uint)t.x, (uint)t.y);
            n = getNotVisitedNeighbor(x, y);
        }
    }
    private List<Vector2> getNotVisitedNeighbor(uint x, uint y)
    {
        List<Vector2> n = new List<Vector2>();
        if (x > 0 && !maze[x - 1, y].getVisited())
            n.Add(new Vector2(x - 1, y));
        if (x < width - 1 && !maze[x + 1, y].getVisited())
            n.Add(new Vector2(x + 1, y));
        if (y > 0 && !maze[x, y - 1].getVisited())
            n.Add(new Vector2(x, y - 1));
        if (y < height - 1 && !maze[x, y + 1].getVisited())
            n.Add(new Vector2(x, y + 1));
        return n;
    }
}
