using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csInventorySlot : MonoBehaviour {
	private static Transform dragPos;
	private static Image dragImg;
	private static Transform SelectItemPos;
	private csItem item;
	private csInventorySlot slot;
	private int itemCount;
	private bool hasItem;
		
	public void Init(){
		if (dragPos == null || dragImg == null || SelectItemPos == null) {
			dragPos = csAlreadyGame.DragItemView;
			dragImg = dragPos.GetComponent<Image> ();
			SelectItemPos = csAlreadyGame.SelectItemView;
		}
		item = csItemList.Instance.EmptyItem;
		GetComponent<Image> ().sprite = item.Picture;
		slot = GetComponent<csInventorySlot> ();
	}

	public void ItemDrag(){
		if (!hasItem)
			return;
		// 마우스로 드래그할 때 임시이미지의 좌표를 마우스와 같게 한다.
		dragPos.position = Input.mousePosition;
	}

	public void ItemDown(){
		if (Input.GetButtonDown ("Fire1")) {
			SelectItemPos.gameObject.SetActive (false);
			if (csItemList.Instance.IsEmpty (item)) {
				hasItem = false;
				return;
			} else {
				hasItem = true;
			}
			// 드래그할 아이템의 정보를 dragPos에 임시 저장한다.
			dragPos.gameObject.SetActive (true);
			dragPos.position = Input.mousePosition;
			dragImg.sprite = GetComponent<Image> ().sprite;
			dragPos.GetChild (0).GetComponent<Text> ().text = transform.GetChild (0).GetComponent<Text> ().text;

			// 현재 아이템 슬롯은 아무것도 없는 것처럼 보이게 한다.
			GetComponent<Image> ().sprite = csItemList.Instance.EmptyItem.Picture;
			transform.GetChild (0).GetComponent<Text> ().text = "";
		}else if(Input.GetButtonDown("Fire2")){
			SelectItemPos.gameObject.SetActive (true);
			SelectItemPos.position = transform.position + Vector3.down * 40;
		}
	}

	public void EndItemDrag(){
		if (!hasItem)
			return;
		// 현재 슬롯과 마우스 드래그한 최종 위치의 아이템 슬롯을 swap 한다.
		csInventory.Instance.ItemSwap (slot, dragPos.position);

		dragPos.gameObject.SetActive (false);
	}

	public void SetItem(csItem item, int count){
		this.item = item;
		this.itemCount = count;
		GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = count.ToString ();
	}

	public void SetItem(int count){
		this.itemCount = count;
		transform.GetChild (0).GetComponent<Text> ().text = count.ToString ();
	}

	public void SetEmpty(){
		SetItem (csItemList.Instance.EmptyItem, 0);
	}

	public void Restore(){
		GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = Count.ToString ();
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
