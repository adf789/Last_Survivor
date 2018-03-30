using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임상의 모든 아이템을 해당 클래스에서 정의
public class csItemList{
	// Non-generic이 아닌 Generic을 이용한 방법으로 Dictionary를 사용
	private static Dictionary<int, csItem> itemDatabase = new Dictionary<int, csItem> ();
	private static Dictionary<string, int> itemKeyDatabase = new Dictionary<string, int> ();
	private static csItemList _instance = null;
	public enum Type
	{
		EMPTY,
		ETC,
		FOOD,
		TOOL,
		BUILD
	}

	private csItemList(){
		// 모든 아이템들은 itemDatabase 안에 추가된다.
		int id = -1;
		AddItem (id, new csItem ("Empty", id++, (int)Type.EMPTY, csAlreadyGame.GetImg ("Empty")));	// id : -1
		AddItem (id, new csItem("Wood", id++, (int)Type.ETC, csAlreadyGame.GetImg("Wood")));	// id : 0
		AddItem (id, new csItem("Iron", id++, (int)Type.ETC, csAlreadyGame.GetImg("Iron")));		// id : 1
		AddItem (id, new csItem("Crystal", id++, (int)Type.ETC, csAlreadyGame.GetImg("Crystal")));	// id : 2
		AddItem (id, new csItem("Meat", id++, (int)Type.FOOD, csAlreadyGame.GetImg("Meat")));	// id : 3
		AddItem (id, new csItem("Axe", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Axe")));	// id : 4
		AddItem (id, new csItem("Pickaxe", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Pickaxe")));	// id : 5
		AddItem (id, new csItem("Shovel", id++, (int)Type.TOOL, csAlreadyGame.GetImg("Shovel")));	// id : 6
		AddItem (id, new csItem("Tent", id++, (int)Type.BUILD, csAlreadyGame.GetImg("Tent")));	// id : 7
		AddItem (id, new csItem("Campfire", id++, (int)Type.BUILD, csAlreadyGame.GetImg("Campfire")));	// id : 8
	}

	private void AddItem(int id, csItem item){
		itemDatabase.Add (id, item);
		itemKeyDatabase.Add (item.Name, id);
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

	public csItem GetItem(string name){
		return (itemKeyDatabase.ContainsKey (name)) ? itemDatabase [itemKeyDatabase [name]] : itemDatabase [-1];
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

	// 아이템별로 사용되었을 시 행동 정의
	public void ItemUse(csItem item){
		switch (item.Name) {
		case "Meat":
			csCharacterStatus.Instance.ChangeFatigue (10);
			csCharacterStatus.Instance.ChangeHp (5);
			csSelectedItem.Instance.UseItem ();
			break;
		case "Tent":
		case "Campfire":
			csBuilding.Instance.SearchBuildingObj (item);
			csBuilding.Instance.IsBuilding = true;
			break;
		}
	}

}
