using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public void itemDown(){
		if (csItemList.Instance.IsEmpty(item))
			return;

		inventory.SetToInventory (item, itemCount);
		SetItem (csItemList.Instance.EmptyItem, 0);
	}


	public void  SetItem(csItem item, int count){
		this.item = item;
		this.itemCount = count;
		gameObject.GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}
}
