using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csInventorySlot : MonoBehaviour {
	private Transform dragPos;
	private Image dragImg;
	private csItem item;
	private csItem emptyItem;
	private csInventorySlot slot;
	private int itemCount;
	private bool hasItem;

	void Start(){
		dragPos = GameObject.Find ("Canvas").transform.Find ("DragImg");
		dragImg = dragPos.GetComponent<Image> ();
		emptyItem = csItemList.Instance.GetItem(-1);
		GetComponent<Image> ().sprite = emptyItem.Picture;
		slot = GetComponent<csInventorySlot> ();
	}
		
	public void ItemDrag(){
		if (!hasItem)
			return;
		// 마우스로 드래그할 때 임시이미지의 좌표를 마우스와 같게 한다.
		dragPos.position = Input.mousePosition;
	}

	public void ItemDown(){
		if (GetComponent<Image> ().sprite == emptyItem.Picture) {
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

		GetComponent<Image> ().sprite = emptyItem.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = "";
	}

	public void EndItemDrag(){
		if (!hasItem)
			return;
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
		SetItem (emptyItem, 0);
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
