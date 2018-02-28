using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCrate : MonoBehaviour {
	[SerializeField] private Item item0;
	[SerializeField] private Item item1;
	[SerializeField] private Item item2;
	[SerializeField] private Item item3;
	[SerializeField] private Item item4;
	[SerializeField] private Item item5;
	private List<Item> items = new List<Item> (6);
	private Transform crateInv;
	private bool isOpened;


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

	}
	
	void OnTriggerEnter(Collider col){
		crateInv.gameObject.SetActive (true);
		if (!isOpened) {
			CrateSetItem ();
			isOpened = true;
		}
	}

	void OnTriggerExit(Collider col){
		crateInv.gameObject.SetActive (false);
	}

	private void CrateSetItem(){
		csItemList itemList = csItemList.Instance;

		for (int i = 0; i < items.Capacity; i++) {
			if (items [i].count == 0)
				continue;
			crateInv.GetChild(i).GetComponent<csCrateSlot>().SetItem(itemList.GetItem ((int)items[i].id), items[i].count);
		}
	}
}
