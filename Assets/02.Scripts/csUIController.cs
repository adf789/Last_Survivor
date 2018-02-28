using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 UI를 관리하기 위한 클래스이다.
public class csUIController : MonoBehaviour {
	private GameObject inventory;
	private bool isShowInventory;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find ("Canvas").transform.Find ("Scroll View").gameObject;
		inventory.SetActive (false);
		isShowInventory = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("I")) {
			isShowInventory = !isShowInventory;
			inventory.SetActive (isShowInventory);
			csCharacterStatus.Instance.isStop = isShowInventory;
			csCameraController.isStop = isShowInventory;
		}
		if (isShowInventory) {
			if (Input.GetButtonDown ("Fire1")) {
//				RaycastHit hit;
//				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//
//				if(Physics.Raycast(ray, out hit)){
//
//				}
			}
		}
	}

}
