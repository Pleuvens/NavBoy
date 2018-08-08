using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravity : MonoBehaviour {

    public FauxGravityAttractor attractor;
    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        Physics.gravity = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
        attractor.Attract(gameObject);
	}
}
