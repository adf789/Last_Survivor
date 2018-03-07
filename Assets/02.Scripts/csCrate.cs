using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCrate : MonoBehaviour {
	[SerializeField] private csItems item0;
	[SerializeField] private csItems item1;
	[SerializeField] private csItems item2;
	[SerializeField] private csItems item3;
	[SerializeField] private csItems item4;
	[SerializeField] private csItems item5;
	private List<csItems> items = new List<csItems> (6);
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
		csCameraController.isStop = true;
		if (!isOpened) {
			CrateSetItem ();
			isOpened = true;
		}
	}

	void OnTriggerExit(Collider col){
		crateInv.gameObject.SetActive (false);
		csCameraController.isStop = false;
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
