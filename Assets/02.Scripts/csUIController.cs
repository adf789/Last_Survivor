using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 UI를 관리하기 위한 클래스이다.
public class csUIController : MonoBehaviour {
	private GameObject inventory;
	private GameObject crateInv;
	private GameObject worktable;
	private RectTransform scrollGuide;
	private bool isShowInventory, isShowWorktable;
	private float[] scrollIndex = {1.5f, 12.5f, 23.5f, 34.5f, 45.5f, 56.5f, 67.5f, 78.5f, 89.5f};
	private int curScrollIndex = 0;

	void Start () {
		inventory = csAlreadyGame.InventoryObj;
		crateInv = csAlreadyGame.CrateObj;
		worktable = csAlreadyGame.WorktableObj;
		scrollGuide = csAlreadyGame.QuickBarObj.transform.Find ("Guide").GetComponent<RectTransform>();
		Init ();
		isShowInventory = false;
	}
	
	void Update () {
		if (Input.GetButtonDown ("I")) {
			OpenInventory ();
			csAlreadyGame.SelectItemView.gameObject.SetActive (false);

		}
		if (Input.GetButtonDown ("J")) {
			worktable.GetComponent<csWorktable> ().UpdateList ();
			OpenWorktable ();
		}
		if (!isShowInventory && !isShowWorktable) {
			float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
			SetScrollIndex (mouseScroll);
			NumKeyDown ();
		}
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
		worktable.SetActive (false);

		Transform quickBarObj = csAlreadyGame.QuickBarObj.transform;
		for(int i = 0; i < quickBarObj.childCount; i++){
			if (quickBarObj.GetChild (i).name.Equals ("Guide"))
				continue;
			quickBarObj.GetChild (i).GetComponent<csInventorySlot> ().Init ();
		}
	}

	private void NumKeyDown(){
		int temp = curScrollIndex;
		if (Input.GetKeyDown (KeyCode.Alpha1))
			temp = 0;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			temp = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha3))
			temp = 2;
		else if (Input.GetKeyDown (KeyCode.Alpha4))
			temp = 3;
		else if (Input.GetKeyDown (KeyCode.Alpha5))
			temp = 4;
		else if (Input.GetKeyDown (KeyCode.Alpha6))
			temp = 5;
		else if (Input.GetKeyDown (KeyCode.Alpha7))
			temp = 6;
		else if (Input.GetKeyDown (KeyCode.Alpha8))
			temp = 7;
		else if (Input.GetKeyDown (KeyCode.Alpha9))
			temp = 8;

		if (temp != curScrollIndex) {
			curScrollIndex = temp;
			MoveScrollGuide ();
			UseTools ();
		}
	}

	// 조합창 GUI를 열고 닫는다.
	private void OpenWorktable(){
		isShowWorktable = !isShowWorktable;
		worktable.SetActive (isShowWorktable);
		csCharacterStatus.Instance.isStop = isShowWorktable;
		csCameraController.isStop = isShowWorktable;
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
