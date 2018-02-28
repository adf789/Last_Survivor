using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 UI를 관리하기 위한 클래스이다.
public class csUIController : MonoBehaviour {
	private GameObject inventory;
	private RectTransform scrollGuide;
	private bool isShowInventory;
	private float[] scrollIndex = {1.5f, 12.5f, 23.5f, 34.5f, 45.5f, 56.5f, 67.5f, 78.5f, 89.5f};
	private int curScrollIndex = 0;

	// Use this for initialization
	void Start () {
		Transform canvas = GameObject.Find ("Canvas").transform;
		inventory = canvas.Find ("Scroll View").gameObject;
		scrollGuide = canvas.Find ("QuickBar").Find ("Guide").GetComponent<RectTransform>();
		inventory.SetActive (false);
		isShowInventory = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("I")) {
			OpenInventory ();
		}
		float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
		SetScrollIndex (mouseScroll);
	}

	private void OpenInventory(){
		isShowInventory = !isShowInventory;
		inventory.SetActive (isShowInventory);
		csCharacterStatus.Instance.isStop = isShowInventory;
		csCameraController.isStop = isShowInventory;
	}

	private void MoveScrollGuide(){
		Vector2 tempVector2 = scrollGuide.anchoredPosition;
		tempVector2.x = scrollIndex [curScrollIndex];
		scrollGuide.anchoredPosition = tempVector2;
	}

	private void SetScrollIndex(float mouse){
		if (mouse == 0f)
			return;

		if (mouse > 0f) {
			curScrollIndex--;
			if (curScrollIndex < 0)
				curScrollIndex = 8;
		} else if (mouse < 0f) {
			curScrollIndex++;
			if (curScrollIndex > 8)
				curScrollIndex = 0;
		}
		MoveScrollGuide ();
	}

}
