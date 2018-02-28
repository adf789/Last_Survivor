using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 UI를 관리하기 위한 클래스이다.
public class csUIController : MonoBehaviour {
	private GameObject inventory;
	private GameObject crateInv;
	private RectTransform scrollGuide;
	private bool isShowInventory;
	private float[] scrollIndex = {1.5f, 12.5f, 23.5f, 34.5f, 45.5f, 56.5f, 67.5f, 78.5f, 89.5f};
	private int curScrollIndex = 0;

	void Start () {
		Transform canvas = GameObject.Find ("Canvas").transform;
		inventory = canvas.Find ("Scroll View").gameObject;
		crateInv = canvas.Find ("CrateInventory").gameObject;
		scrollGuide = canvas.Find ("QuickBar").Find ("Guide").GetComponent<RectTransform>();
		Init ();
		isShowInventory = false;
	}
	
	void Update () {
		if (Input.GetButtonDown ("I")) {
			OpenInventory ();
		}
		float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
		SetScrollIndex (mouseScroll);
	}


	private void Init(){
		Transform inventoryTransform = inventory.transform.GetChild (0).GetChild (0);
		for (int i = 0; i < inventoryTransform.childCount; i++) {
			inventoryTransform.GetChild (i).GetComponent<csInventorySlot> ().Init ();
		}
		for (int i = 0; i < crateInv.transform.childCount; i++) {
			crateInv.transform.GetChild (i).GetComponent<csCrateSlot> ().Init ();
		}
		inventory.SetActive (false);
		crateInv.SetActive (false);
	}

	// 인벤토리 GUI를 열고 닫는다.
	private void OpenInventory(){
		isShowInventory = !isShowInventory;
		inventory.SetActive (isShowInventory);
		csCharacterStatus.Instance.isStop = isShowInventory;
		csCameraController.isStop = isShowInventory;
	}

	// 퀵바의 가이드 오브젝트 위치를 조절한다.
	private void MoveScrollGuide(){
		Vector2 tempVector2 = scrollGuide.anchoredPosition;
		tempVector2.x = scrollIndex [curScrollIndex];
		scrollGuide.anchoredPosition = tempVector2;
	}

	// 퀵바의 Guide 오브젝트 위치의 아이템의 종류에 따라 손의 도구를 바꾼다.
	public void UseTools(){
		csItem item = csInventory.Instance.GetToInventory (curScrollIndex);
		csCharacterStatus.Instance.SetEquipment (item.Name);
		
	}

	// 마우스의 휠 움직임에 따라 Guide 오브젝트 움직임 방향을 정한다.
	private void SetScrollIndex(float mouse){
		if (mouse == 0f)
			return;

		if (mouse > 0f) {
			// 마우스의 휠을 올릴 때
			curScrollIndex--;
			if (curScrollIndex < 0)
				curScrollIndex = 8;
		} else if (mouse < 0f) {
			// 마우스의 휠을 내릴 때
			curScrollIndex++;
			if (curScrollIndex > 8)
				curScrollIndex = 0;
		}
		MoveScrollGuide ();
		UseTools ();
	}

}
