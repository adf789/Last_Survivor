using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 인벤토리를 관리하는 클래스이다.
public class csInventory{
	private static csInventory _instance = null;
	private Dictionary<csItem, int> curInventory = new Dictionary<csItem, int>();
	private Transform objInventory = null;
	private int curIndex = 0;

	public static csInventory Instance{
		get{
			if (_instance == null) {
				_instance = new csInventory ();
			}
			return _instance;
		}
	}

	// 아이템과 개수로 현재 인벤토리 상태를 재배열한다.
	public void SetToInventory(csItem item, int count){
		// 처음 실행시 objInventory의 GameObject를 찾는다.
		if(objInventory == null)
			objInventory = GameObject.Find ("Canvas").transform.GetChild (0).GetChild(0).Find ("Inventory");

		// 인벤토리에 이미 아이템이 있는 경우 count만 증가시킨다.
		if (curInventory.ContainsKey (item)) {
			int count1 = curInventory [item] + count;
			curInventory [item] = count1;
			objInventory.GetChild (curIndex).GetChild (0).GetComponent<Text> ().text = count1.ToString();

			if (curInventory [item] <= 0) {
				curInventory.Remove (item);
				curIndex--;
			}
		}
		// 인벤토리에 해당 아이템이 없을 경우
		else {
			if (count <= 0)
				return;
			objInventory.GetChild (curIndex).gameObject.SetActive (true);
			objInventory.GetChild (curIndex).GetComponent<Image> ().sprite = item.Picture;
			objInventory.GetChild (curIndex).GetChild (0).GetComponent<Text> ().text = count.ToString();
			curIndex++;
			curInventory.Add (item, count);
		}
	}

	public int GetToInventory(csItem item){
		if (curInventory.ContainsKey (item))
			return curInventory [item];
		return 0;
	}

	public bool HasInventory(csItem item){
		return curInventory.ContainsKey (item);
	}


}
