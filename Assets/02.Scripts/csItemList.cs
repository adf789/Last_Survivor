using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItemList{
	// Non-generic이 아닌 Generic을 이용한 방법으로 Dictionary를 사용
	private static Dictionary<int, csItem> itemDatabase = new Dictionary<int, csItem> ();
	private static csItemList _instance = null;

	public const int EMPTY = -1, ETC = 0;

	private csItemList(){
		itemDatabase.Add (-1, new csItem ("", -1, EMPTY, Resources.Load<Sprite>("Empty")));
		itemDatabase.Add (0, new csItem("나무", 0, ETC, Resources.Load<Sprite>("Wood")));
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
