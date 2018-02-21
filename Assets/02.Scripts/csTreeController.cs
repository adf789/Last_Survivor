using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTreeController : MonoBehaviour {
	private csObjectStatus stats;
	private Animator anim;
	private BoxCollider col;

	private float alpha = 1f;

	// Use this for initialization
	void Start () {
		stats = gameObject.GetComponent<csObjectStatus> ();
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		alpha -= 0.01f;
		if (alpha <= 0f)
			alpha = 0f;
		Debug.Log (alpha);
	}

	public void Lumber(){
		stats.DecreaseHp (20f);
		if (stats.GetHp <= 0f) {
			col.enabled = false;
			anim.SetBool ("Lumber", true);
			stats.death (2.5f);
		}
	}
}
