using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCheckBuilding : MonoBehaviour {
	private csBuilding buildingInstance;

	// Use this for initialization
	void Start () {
		buildingInstance = csBuilding.Instance;
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Ground"))
			return;
		buildingInstance.SendMessage ("SetBuilding", true);
	}

	void OnTriggerExit(Collider col){
		buildingInstance.SendMessage ("SetBuilding", false);
	}
}
