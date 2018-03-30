using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 상자 인벤토리의 슬롯에 해당하는 이벤트 클래스
public class csCrateSlot : MonoBehaviour {
	private csItem item;
	private int itemCount;
	private csInventory inventory;

	public void Init(){
		item = csItemList.Instance.EmptyItem;
		itemCount = 0;
		inventory = csInventory.Instance;
		gameObject.GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}

	// 해당 슬롯을 눌렀을 경우 해당 아이템 습득
	public void itemDown(){
		// 해당 슬롯의 아이템이 빈 아이템인 경우 행동없이 리턴
		if (csItemList.Instance.IsEmpty(item))
			return;

		inventory.SetToInventory (item, itemCount);
		// 아이템 습득 후 빈 아이템으로 교체
		SetItem (csItemList.Instance.EmptyItem, 0);
	}

	// 해당 슬롯에 아이템과 개수 초기화.
	public void  SetItem(csItem item, int count){
		this.item = item;
		this.itemCount = count;
		gameObject.GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}
}
