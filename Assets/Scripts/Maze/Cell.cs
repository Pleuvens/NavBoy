using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    private List<Vector2> linked;
    private Type type;
    private bool visited;

    public Cell(Type type = Type.NONE)
    {
        linked = new List<Vector2>();
        this.type = type;
        visited = false;
    }

    public Type getType()
    {
        return type;
    }

    public List<Vector2> getLinkedList()
    {
        return linked;
    }

    public void setVisited(bool b)
    {
        visited = b;
    }

    public bool getVisited()
    {
        return visited;
    }

    public void addLink(Vector2 posToLink)
    {
        linked.Add(posToLink);

    }

    public bool isLinked(uint x, uint y)
    {
        foreach (Vector2 a in linked)
            if (a.x == x && a.y == y)
                return true;
        return false;
    }
    public string display()
    {
        if (type == Type.NONE)
            return "  ";
        if (type == Type.START)
            return "SS";
        if (type == Type.PATH)
            return "XX";
        return "EE";
    }
}

public enum Type
{
    START,
    END,
    PATH,
    NONE
};