using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSelectedItem : MonoBehaviour {
	private csItem curItem;
	private int count;
	private static csSelectedItem _instance;

	public void BtnSelect(){
		gameObject.SetActive (false);
		if (curItem.Type == (int)csItemList.Type.BUILD) {
			csBuilding.Instance.SearchBuildingObj (curItem);
			csBuilding.Instance.IsBuilding = true;
		}
	}

	public void BtnDelete(){
		csInventory.Instance.SetToInventory (curItem, -count);
		csMessageBox.Show (curItem.Name + "을(를) 모두 버렸습니다.");
		gameObject.SetActive (false);
	}

	public csItem CurItem{
		set{
			curItem = value;
			count = csInventory.Instance.CountToInventory (curItem);
		}
	}

	public static csSelectedItem Instance{
		get{
			if (_instance == null)
				_instance = FindObjectOfType<csSelectedItem> () as csSelectedItem;
			return _instance;
		}
	}

	public void UseItem(){
		csInventory.Instance.SetToInventory (curItem, -1);
	}
}
