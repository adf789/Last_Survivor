using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템이 사용에 관한 이벤트 클래스
public class csSelectedItem : MonoBehaviour {
	private csItem curItem;
	private int count;
	private static csSelectedItem _instance;
	private csWorktable workTable;

	// 아이템을 사용하는 이벤트
	public void BtnSelect(){
		gameObject.SetActive (false);
		csItemList.Instance.ItemUse (curItem);
	}

	// 아이템을 버리는 이벤트
	public void BtnDelete(){
		csInventory.Instance.SetToInventory (curItem, -count);
		csMessageBox.Show (curItem.Name + "을(를) 모두 버렸습니다.");
		gameObject.SetActive (false);
		if (workTable == null) {
			workTable = csAlreadyGame.WorktableObj.GetComponent<csWorktable> ();
		}
		workTable.RemoveAtPossibilityList ();
		workTable.UpdateList ();
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

	// 선택된 아이템을 소모함
	// 외부에서 사용될 경우
	public void UseItem(){
		csInventory.Instance.SetToInventory (curItem, -1);
		if (workTable == null) {
			workTable = csAlreadyGame.WorktableObj.GetComponent<csWorktable> ();
		}
		workTable.RemoveAtPossibilityList ();
		workTable.UpdateList ();
	}
}
