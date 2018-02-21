using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStartInventory : MonoBehaviour {

	void Start () {
		gameObject.GetComponent<RectTransform> ().localPosition = new Vector3 (0f, -71.545f);
	}
}
