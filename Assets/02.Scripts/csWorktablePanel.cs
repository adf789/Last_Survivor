using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csWorktablePanel : MonoBehaviour {
	private csItem combItem;
	private int combCount = 1;
	private csWorktable workTable;
	private bool hasItems = false;

	void Start () {
		if(combItem == null)
			combItem = csItemList.Instance.EmptyItem;
	}
	
	public void BtnCombine(){
		if (!hasItems) {
			csMessageBox.Show ("재료가 부족합니다.");
			return;
		}
		if (csItemList.Instance.IsEmpty (combItem))
			return;
		
		List<csItems> materials = combItem.MaterialList;
		for (int i = 0; i < materials.Count; i++) {
			csItem item = csItemList.Instance.GetItem ((int)materials [i].id);
			if (!csInventory.Instance.SetToInventory (item, -materials [i].count)) {
				csMessageBox.Show ("재료가 부족합니다.");
				return;
			}
		}
		csInventory.Instance.SetToInventory (combItem, combCount);
		workTable.RemoveAtPossibilityList ();
		workTable.UpdateList();
	}

	public bool HasItems{
		set{
			hasItems = value;
		}
	}

	public void SetWorkPanel(csItem item, int count, csWorktable workTable) {
		this.combItem = item;
		this.combCount = count;
		this.workTable = workTable;
	}
}
