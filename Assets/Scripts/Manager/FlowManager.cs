using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        } else
        {
            player = GameObject.Find("Player(Clone)");
        }
	}
}
