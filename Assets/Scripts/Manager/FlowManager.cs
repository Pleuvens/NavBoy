using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour {

    public Maze maze;
    public GameObject mazeHolder;
    public GameObject player;

	// Use this for initialization
	void Start () {
        maze.InitMap();
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null)
        {
            if (player.GetComponent<BoyBehaviour>().distance < 1)
            {
                for (int i = 0; i < mazeHolder.transform.childCount; i++)
                {
                    Destroy(mazeHolder.transform.GetChild(i).gameObject);
                    Destroy(player);
                }
                maze.InitMap();
            }
        } else
        {
            player = GameObject.Find("Player(Clone)");
        }
	}
}
