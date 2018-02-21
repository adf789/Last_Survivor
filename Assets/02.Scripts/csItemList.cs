using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csItemList{
	private static Dictionary<string, csItem> items = new Dictionary<string, csItem> ();
	private static csItem emptyItem = new csItem ("", -1, EMPTY);
	private static csItemList _instance = null;
	public const int EMPTY = -1, ETC = 0;

	private csItemList(){
		items.Add ("나무", new csItem("나무", 0, 0));
	}

	public static csItemList Instance{
		get{
			if (_instance == null)
				_instance = new csItemList ();
			return _instance;
		}
	}

	public csItem GetItem(string key){
		return (items.ContainsKey (key)) ? items [key] : emptyItem;
	}
}
