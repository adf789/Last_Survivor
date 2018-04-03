using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상자 오브젝트에 들어갈 아이템을 제어하는 클래스
public class csCrate : MonoBehaviour, csObjectInteraction {
	// 에디터에서 아이템을 직접 지정할 수 있다.
	[SerializeField] private csItems item0;
	[SerializeField] private csItems item1;
	[SerializeField] private csItems item2;
	[SerializeField] private csItems item3;
	[SerializeField] private csItems item4;
	[SerializeField] private csItems item5;
	private List<csItems> items = new List<csItems> (6);
	private Transform crateInv;
	private bool isOpened, isInit;


	// Use this for initialization
	void Start () {
		crateInv = GameObject.Find ("Canvas").transform.Find ("CrateInventory");
		items.Add (item0);
		items.Add (item1);
		items.Add (item2);
		items.Add (item3);
		items.Add (item4);
		items.Add (item5);
		isOpened = false;
		isInit = false;
	}

	void Update(){
		// 상자가 열려있는 경우에 Esc버튼으로 상자 인벤토리를 닫는다.
		if (isOpened) {
			if(Input.GetKeyDown(KeyCode.Escape)){
				CloseCrate ();
			}
		}
	}

	// 트리거 영역을 벗어난 경우에도 상자 인벤토리를 닫는다.
	void OnTriggerExit(Collider col){
		CloseCrate ();
	}

	// 상자 인벤토리 오브젝트를 비활성화하고 카메라의 정지상태를 비활성화한다.
	private void CloseCrate(){
		Cursor.visible = false;
		crateInv.gameObject.SetActive (false);
		csCameraController.isStop = false;
		isOpened = false;
	}

	// 상자에 들어갈 아이템을 Set한다.
	private void CrateSetItem(){
		csItemList itemList = csItemList.Instance;

		for (int i = 0; i < items.Count; i++) {
			if (items [i].count == 0)
				continue;
			crateInv.GetChild(i).GetComponent<csCrateSlot>().SetItem(itemList.GetItem ((int)items[i].id), items[i].count);
		}
	}

	// 플레이어와의 상호작용을 정의한다.
	public void Interaction(GameObject obj){
		if (obj.tag == "Player") {
			Cursor.visible = true;
			// 상자인벤토리를 활성화
			crateInv.gameObject.SetActive (true);
			crateInv.transform.position = Camera.main.WorldToScreenPoint (transform.position + Vector3.up * 0.5f);
			// 카메라 멈춤상태로 전환
			csCameraController.isStop = true;
			isOpened = true;

			// 상자를 처음 열 경우 상자 초기화
			if (!isInit) {
				CrateSetItem ();
				isInit = true;
			}
		}
	}

	public void Respawn(Vector3 position){

	}
}
