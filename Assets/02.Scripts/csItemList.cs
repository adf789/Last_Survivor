using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItemList{
	// Non-generic이 아닌 Generic을 이용한 방법으로 Dictionary를 사용
	private static Dictionary<int, csItem> itemDatabase = new Dictionary<int, csItem> ();
	private static csItemList _instance = null;
	public enum Type
	{
		EMPTY,
		ETC,
		FOOD,
		TOOL
	}

	private csItemList(){
		// 모든 아이템들은 itemDatabase 안에 추가된다.
		int id = -1;
		itemDatabase.Add (id, new csItem ("Empty", id++, (int)Type.EMPTY, csAlreadyGame.GetImg("Empty")));	// id : -1
		itemDatabase.Add (id, new csItem("Wood", id++, (int)Type.ETC, csAlreadyGame.GetImg("Wood")));	// id : 0
		itemDatabase.Add (id, new csItem("Iron", id++, (int)Type.ETC, csAlreadyGame.GetImg("Iron")));		// id : 1
		itemDatabase.Add (id, new csItem("Crystal", id++, (int)Type.ETC, csAlreadyGame.GetImg("Crystal")));	// id : 2
		itemDatabase.Add (id, new csItem("Meat", id++, (int)Type.FOOD, csAlreadyGame.GetImg("Meat")));	// id : 3
		itemDatabase.Add (id, new csItem("Axe", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Axe")));	// id : 4
		itemDatabase.Add (id, new csItem("Pickaxe", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Pickaxe")));	// id : 5
		itemDatabase.Add (id, new csItem("Shovel", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Shovel")));	// id : 6
	}

	public static csItemList Instance{
		get{
			if (_instance == null) {
				_instance = new csItemList ();
			}
			return _instance;
		}
	}

	public csItem GetItem(int id){
		return (itemDatabase.ContainsKey (id)) ? itemDatabase [id] : itemDatabase[-1];
	}

	public bool IsEmpty(csItem item){
		if (item.Equals (itemDatabase [-1]))
			return true;
		else
			return false;
	}

	public csItem EmptyItem{
		get{
			return itemDatabase [-1];
		}
	}

}
