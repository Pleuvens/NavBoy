using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBehaviourSphere : MonoBehaviour {

    public float speed;
    const float gravity = 10f;
    CharacterController controller;
    Animator animator;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("run", true);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * speed;
        controller.Move(direction * Time.deltaTime);
	}
}
