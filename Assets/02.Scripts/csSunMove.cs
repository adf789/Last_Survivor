using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSunMove : MonoBehaviour {
	private Transform transform;
	[SerializeField]
	private Material[] skys;
	[SerializeField]
	private float turnSpeed;
	[SerializeField]
	private Transform moonPos;
	[SerializeField]
	private float Moon_Distance = 1000f;
	[SerializeField]
	private float Moon_Scale = 15f;

	// Use this for initialization
	void Start () {
		transform = gameObject.GetComponent<Transform> ();
		transform.position = new Vector3 (250f, 0f, 250f);
		moonPos.rotation = transform.rotation;
		moonPos.Rotate (Vector3.right, 180f);

		Transform moon = transform.Find ("Moon").GetComponent<Transform> ();
		moon.localScale = new Vector3 (Moon_Scale, Moon_Scale, Moon_Scale);
		moon.localPosition = new Vector3 (moonPos.localPosition.x, moonPos.localPosition.y, Moon_Distance);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.right, turnSpeed * Time.deltaTime);
	}
}
