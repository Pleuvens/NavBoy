using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour {

    public Maze maze;

	// Use this for initialization
	void Start () {
        maze.InitMap();
        maze.SetMeshes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
