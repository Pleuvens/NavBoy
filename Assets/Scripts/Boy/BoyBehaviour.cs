﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoyBehaviour : MonoBehaviour {

    public Camera cam;

    Vector3 destination;

    public float distance = Mathf.Infinity;

    public NavMeshAgent agent;

    public Animator animator;

    public void Init()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        distance = Vector3.Distance(destination, transform.position);
    }

    void ClickMovement()
    {
        if (!agent.enabled)
            agent.enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    public void MoveToDestination(Vector3 destination)
    {
        if (!agent.enabled)
            agent.enabled = true;
        animator.SetBool("run", true);
        agent.SetDestination(destination);
        this.destination = destination;
    }
}
