using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSunMove : MonoBehaviour {
	private Transform ground;
	[SerializeField] private float turnSpeed;

	// Use this for initialization
	void Start () {
		ground = GameObject.Find ("Ground").transform;
		transform.position = new Vector3 (245f, 106f, -25f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (ground.position, Vector3.right, turnSpeed * Time.deltaTime);
	}
}
