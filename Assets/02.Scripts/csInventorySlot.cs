using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 슬롯의 이벤트 클래스
public class csInventorySlot : MonoBehaviour {
	private static Transform dragPos;
	private static Image dragImg;
	private static Transform SelectItemPos;
	private csItem item;
	private csInventorySlot slot;
	private int itemCount;
	private bool hasItem, isDrag;

	// 슬롯의 초기 설정을 한다.
	public void Init(){
		// 아이템의 이동을 시각적으로 표현하기 위한 dragPos, dragImg를 초기화한다.
		// 임의로 위치할 dragPos의 위치와 이미지로 나타낼 dragImg가 없는 경우 초기화한다.
		// SelectItemPos은 해당 슬롯의 사용, 버림과 관련된 변수다.
		if (dragPos == null || dragImg == null || SelectItemPos == null) {
			dragPos = csAlreadyGame.DragItemView;
			dragImg = dragPos.GetComponent<Image> ();
			SelectItemPos = csAlreadyGame.SelectItemView;
		}
		// 초기 아이템은 빈 아이템으로 대체한다.
		item = csItemList.Instance.EmptyItem;
		GetComponent<Image> ().sprite = item.Picture;
		slot = GetComponent<csInventorySlot> ();
	}

	// 해당 슬롯을 드래그할 경우
	public void ItemDrag(){
		if (!hasItem)
			return;
		// 마우스로 드래그할 때 임시이미지의 좌표를 마우스와 같게 한다.
		dragPos.position = Input.mousePosition;
		isDrag = true;
	}

	// 해당 슬롯이 눌러졌을 경우
	public void ItemDown(){
		// 만약 건설 상태일 경우 행동없이 리턴한다.
		if (csBuilding.Instance.IsBuilding)
			return;
		// 해당 슬롯이 빈 아이템인 경우에도 행동없이 리턴한다.
		if (csItemList.Instance.IsEmpty (item)) {
			hasItem = false;
			return;
		} else {
			hasItem = true;
		}
		// 해당 슬롯에 지정된 Fire1 버튼이 눌러진 경우 아이템을 이동할 준비를 한다.
		if (Input.GetButtonDown ("Fire1")) {
			SelectItemPos.gameObject.SetActive (false);
			// 드래그할 아이템의 정보를 dragPos에 임시 저장한다.
			dragPos.gameObject.SetActive (true);
			dragPos.position = Input.mousePosition;
			dragImg.sprite = GetComponent<Image> ().sprite;
			dragPos.GetChild (0).GetComponent<Text> ().text = transform.GetChild (0).GetComponent<Text> ().text;

			// 현재 아이템 슬롯은 아무것도 없는 것처럼 보이게 한다.
			GetComponent<Image> ().sprite = csItemList.Instance.EmptyItem.Picture;
			transform.GetChild (0).GetComponent<Text> ().text = "";
		}
		// 해당 슬롯에 지정된 Fire1 버튼이 눌러진 경우 아이템을 사용할 준비를 한다.
		else if(Input.GetButtonDown("Fire2")){
			// 아이템 사용에 관련된 SelectItemPos 오브젝트를 활성화한다.
			SelectItemPos.gameObject.SetActive (true);
			SelectItemPos.position = transform.position + Vector3.down * 40;
			// 현재 선택된 아이템을 SelectedItem으로 지정한다.
			csSelectedItem.Instance.CurItem = item;
		}
	}

	// 해당 슬롯의 드래그가 끝났을 경우
	public void EndItemDrag(){
		// 빈 아이템일 때는 행동없이 리턴
		if (!hasItem)
			return;
		// 현재 슬롯과 마우스 드래그한 최종 위치의 아이템 슬롯을 swap 한다.
		isDrag = false;
		// 해당 슬롯의 아이템과 마우스가 위치한 슬롯을 swap한다.
		csInventory.Instance.ItemSwap (slot, dragPos.position);

		// 임의로 표시된 오브젝트는 비활성화
		dragPos.gameObject.SetActive (false);
	}

	// 해당 슬롯의 버튼이 떨어졌을 경우
	public void ItemUp(){
		// 해당 슬롯의 아이템이 빈 아이템이거나 Fire2 버튼으로 눌러진 경우 행동없이 리턴
		if (!hasItem || Input.GetButtonUp("Fire2"))
			return;
		// 드래그 없이 그 자리에서 마우스 버튼을 놓았을 때 다시 원상태로 복구한다.
		if (!isDrag) {
			isDrag = false;
			dragPos.gameObject.SetActive (false);
			Restore ();
		}
	}

	// 해당 슬롯에 아이템과 개수를 초기화
	public void SetItem(csItem item, int count){
		this.item = item;
		this.itemCount = count;
		GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = count.ToString ();
	}

	//해당 슬롯의 개수를 초기화
	public void SetItem(int count){
		this.itemCount = count;
		transform.GetChild (0).GetComponent<Text> ().text = count.ToString ();
	}

	public void SetEmpty(){
		SetItem (csItemList.Instance.EmptyItem, 0);
	}

	// 해당 슬롯을 이전 상태로 되돌린다.
	public void Restore(){
		GetComponent<Image> ().sprite = dragImg.sprite;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}

	public csItem Item{
		get{
			return item;
		}
	}

	public int Count{
		get{
			return itemCount;
		}
	}
}
