using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class csConstruction : MonoBehaviour {
	[SerializeField]
	private Material[] materials;

	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	void Update (){
	
	}

	public void SetMaterials(){
		gameObject.GetComponent<Renderer> ().materials = materials;
	}

	public abstract void Action ();
}
