using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItemList{
	// Non-generic이 아닌 Generic을 이용한 방법으로 Dictionary를 사용
	private static Dictionary<int, csItem> itemDatabase = new Dictionary<int, csItem> ();
	private static csItemList _instance = null;

	public const int EMPTY = -1, ETC = 0, FOOD = 1;

	private csItemList(){
		// 모든 아이템들은 itemDatabase 안에 추가된다.
		int id = -1;
		itemDatabase.Add (id, new csItem ("", id++, EMPTY, csAlreadyGame.GetImg("Empty")));	// id : -1
		itemDatabase.Add (id, new csItem("나무", id++, ETC, csAlreadyGame.GetImg("Wood")));	// id : 0
		itemDatabase.Add (id, new csItem("철", id++, ETC, csAlreadyGame.GetImg("Iron")));		// id : 1
		itemDatabase.Add (id, new csItem("수정", id++, ETC, csAlreadyGame.GetImg("Crystal")));	// id : 2
		itemDatabase.Add (id, new csItem("고기", id++, FOOD, csAlreadyGame.GetImg("Meat")));	// id : 3
	}

	public static csItemList Instance{
		get{
			if (_instance == null)
				_instance = new csItemList ();
			return _instance;
		}
	}

	public csItem GetItem(int id){
		return (itemDatabase.ContainsKey (id)) ? itemDatabase [id] : itemDatabase[-1];
	}

}
