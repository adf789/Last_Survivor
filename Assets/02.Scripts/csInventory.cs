using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 인벤토리를 관리하는 클래스이다.
public class csInventory{
	private const int SIZE = 12;
	private static csInventory _instance = null;
	private List<csItem> curInventory = new List<csItem>(SIZE);
	private List<int> countList = new List<int> (SIZE);
	private static Transform objInventory = null;
	private int curIndex = 0;




	public static csInventory Instance{
		get{
			if (_instance == null) {
				_instance = new csInventory ();
			}
			return _instance;
		}
	}

	private csInventory(){
		// 처음 인벤토리를 찾을시 objInventory의 GameObject를 찾는다.
		objInventory = GameObject.Find ("Canvas").transform.GetChild (0).GetChild(0).Find ("Inventory");
		objInventory.GetComponent<RectTransform> ().localPosition = new Vector3 (0f, -71.545f);

		// 처음 인벤토리를 초기화한다.
		for (int i = 0; i < SIZE; i++) {
			// Empty 아이템으로 인벤토리를 채운다.
			curInventory.Add (csItemList.Instance.GetItem (-1));
			countList.Add (0);
		}
	}

	// 아이템과 개수로 현재 인벤토리 상태를 재배열한다.
	public void SetToInventory(csItem item, int count){
		if (SetToList (item, count)) {
			csMessageBox.Show (item.Name + " " + count + "개를 얻었습니다.");
		} else if (count < 0) {
			csMessageBox.Show ("해당 아이템이 인벤토리에 충분치 않습니다.");
		} else if(curIndex > 11){
			csMessageBox.Show ("인벤토리 공간이 없습니다.");
		}
			
	}

	public int GetToInventory(csItem item){
		if (IsContain (item)) {
			int index = curInventory.IndexOf (item);
			return countList[index];
		}
		return 0;
	}

	public bool HasInventory(csItem item){
		return IsContain (item);
	}

	// 현재 사용하는 csItem과 int를 담고있는 list의 순서를 같게 하기 위해
	// 함수를 이용하여 삽입, 삭제, 검사를 함.

	// SetToList의 반환 형태는 새로 추가를 했거나, 값을 변경에 성공했으면 true를 반환하고, 실패하였으면 false를 반환한다.
	// 또한 이 함수에서 인벤토리의 GUI 상태를 변경한다.
	private bool SetToList(csItem item, int count){
		// 해당 아이템이 있는 경우
		if (IsContain (item)) {
			// 해당 아이템의 인덱스와 개수를 구한다.
			int index = curInventory.IndexOf (item);
			int curCount = countList [index];
			// 현재 가지고 있는 개수와 파라미터인 count 값과의 합이 음수이면 아무 행동없이 false를 반환한다.
			if (curCount + count < 0)
				return false;
			curInventory [index] = csItemList.Instance.GetItem (-1);
			countList [index] = 0;
			if (curCount + count == 0) {
				// 인벤토리 UI에서 해당 아이템을 삭제한다.
				DeleteToInventoryUI (item, index);
				SetIndex ();
			}
			if (curCount + count > 0) {
				curInventory[index] = item;
				countList[index] = curCount + count;

				// 해당 아이템의 개수를 인벤토리 UI에서 수정한다.
				AddToInventoryUI (index, curCount + count);
			}
			return true;
		}
		// 해당 아이템이 없는 경우
		else {
			if (count <= 0 || curIndex > 11)
				return false;
			// 아이템 추가 전 추가해야 될 index를 찾음.
			SetIndex ();

			curInventory[curIndex] = item;
			countList[curIndex] = count;

			// 해당 아이템을 인벤토리 UI에 새로 추가
			AddToInventoryUI (item, count);
			return true;
		}
	}

	// 현재 아이템이 들어갈 index를 찾음
	private void SetIndex(){
		for (int i = 0; i < curInventory.Capacity; i++) {
			if (curInventory [i].Equals(csItemList.Instance.GetItem (-1))) {
				curIndex = i;
				break;
			}
		}
	}

	private bool IsContain(csItem item){
		return curInventory.Contains (item);
	}

	// List의 아이템 항목 중에서 index1과 index2의 순서를 바꾼다.
	private bool ReplaceToList(int index1, int index2){
		int range = curInventory.Capacity;
		if (range - 1 < index1 && range - 1 < index2)
			return false;

		csItem tempItem = curInventory [index1];
		int temp = countList [index1];

		curInventory [index1] = curInventory [index2];
		countList [index1] = countList [index2];

		curInventory [index2] = tempItem;
		countList [index2] = temp;

		return true;
	}

	// 현재 얻은 아이템을 인벤토리 UI에 추가하거나 변경한다.
	private void AddToInventoryUI(csItem item, int count){
		GameObject objSlot = objInventory.GetChild (curIndex).gameObject;
		objSlot.GetComponent<csInventorySlot> ().SetItem (item, count);
	}

	// 현재 index의 아이템의 개수를 count로 변경한다.
	private void AddToInventoryUI(int index, int count){
		GameObject objSlot = objInventory.GetChild (index).gameObject;
		objSlot.GetComponent<csInventorySlot> ().SetItem (count);
	}

	// 인벤토리 UI에서 현재 아이템을 삭제한다.
	private void DeleteToInventoryUI(csItem item, int index){
		GameObject objSlot = objInventory.GetChild (index).gameObject;
		objSlot.GetComponent<csInventorySlot> ().SetEmpty ();
	}

	public void ItemSwap(csInventorySlot toSlot, Vector3 fromPos){
		csInventorySlot fromSlot;
		if (!NearSlot (fromPos, out fromSlot)) {
			toSlot.Restore ();
			return;
		}

		// 바꿀 아이템의 index 값을 구한다.
		int index1 = curInventory.IndexOf (toSlot.Item);
		int index2 = curInventory.IndexOf (fromSlot.Item);

		// 만약 바꿀 목표의 아이템이 빈 아이템일 경우 curIndex에 임시 저장된 인덱스 값으로 변경한 후
		// 다음 들어갈 위치의 curIndex를 재탐색한다.
		if (index2 == -1) {
			index2 = curIndex;
			SetIndex ();
		}

		// 아이템의 위치와 개수를 swap 한다.
		csItem toItem = toSlot.Item;
		int toCount = toSlot.Count;
		toSlot.SetItem(fromSlot.Item, fromSlot.Count);
		fromSlot.SetItem (toItem, toCount);

		// list의 아이템 정보를 swap 한다.
		ReplaceToList (index1, index2);
	}

	// 해당 좌표에 있는 오브젝트의 csInventorySlot 인스턴스를 반환한다.
	private bool NearSlot(Vector3 pos, out csInventorySlot slot){
		// pos의 좌표가 인벤토리 GUI내의 Slot이 있는 위치인지 파악하며, 찾지 못했을 시 false를 반환한다.
		for (int i = 0; i < objInventory.childCount; i++) {
			float x = objInventory.GetChild (i).position.x;
			// pos의 x좌표가 현재 탐색한 Slot의 x좌표 안의 값인지 확인한다.
			if (x < pos.x && x + 40 > pos.x) {
				float y = objInventory.GetChild (i).position.y;
				// pos의 y좌표가 현재 탐색한 Slot의 y좌표 안의 값인지 확인한다.
				if (y > pos.y && y - 40 < pos.y) {
					// x, y좌표가 모두 범위 안에 있는 경우 out 파라미터인 slot에 현재 탐색된 slot을 저장한다.
					slot = objInventory.GetChild (i).GetComponent<csInventorySlot> ();

					// slot이 빈 아이템일 경우 현재 위치를 curIndex에 저장한다.
					curIndex = i;
					return true;
				}
			}
		}
		slot = objInventory.GetChild (0).GetComponent<csInventorySlot> ();
		return false;
	}
}
