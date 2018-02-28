using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csCrateSlot : MonoBehaviour {
	private csItem item;
	private csItem emptyItem;
	private int itemCount;
	private csInventory inventory;

	void Start(){
		
	}

	public void Init(){
		emptyItem = csItemList.Instance.EmptyItem;
		item = emptyItem;
		itemCount = 0;
		inventory = csInventory.Instance;
		gameObject.GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}

	public void itemDown(){
		if (item.Equals (emptyItem))
			return;

		inventory.SetToInventory (item, itemCount);
		SetItem (emptyItem, 0);
	}


	public void  SetItem(csItem item, int count){
		this.item = item;
		this.itemCount = count;
		gameObject.GetComponent<Image> ().sprite = item.Picture;
		transform.GetChild (0).GetComponent<Text> ().text = itemCount.ToString ();
	}
}
