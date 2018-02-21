using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csInventorySlot : MonoBehaviour {
	private bool isClicked = false;

	void Update(){
		if (isClicked)
			transform.position = Input.mousePosition;
	}

	void OnClick(){
		isClicked = !isClicked;
	}
}
