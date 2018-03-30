using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임상 아이템 클래스
// 해당 클래스에 조합재료, 조합가능한 아이템을 정의하기위해 클래스로 정의
public class csItem {
	private string name;
	private int id;
	private int type;
	private Sprite picture;
	private List<csItems> combList = null;
	private List<csItems> materialList = null;

	public csItem(string name, int id, int type, Sprite picture){
		this.name = name;
		this.id = id;
		this.type = type;
		this.picture = picture;
	}

	public string Name{
		get{
			return name;
		}
	}

	public int Id{
		get{
			return id;
		}
	}
		
	public int Type{
		get{
			return type;
		}
	}

	public Sprite Picture{
		get{
			return picture;
		}
	}

	// 조합 가능한 List를 반환
	public List<csItems> CombinationList{
		get{
			if (combList == null)
				combList = new List<csItems> ();
			return combList;
		}
	}

	// 조합 재료 List를 반환
	public List<csItems> MaterialList{
		get{
			if (materialList == null)
				materialList = new List<csItems> ();
			return materialList;
		}
	}

	// 조합 가능한 아이템을 List에 추가
	public void AddCombination(csItem item, int count){
		csItems items = new csItems (item, count);
		if (combList == null)
			combList = new List<csItems> ();
		combList.Add (items);
	}

	// 해당 아이템을 조합하기 위한 재료를 List에 추가
	public void AddMaterial(csItem item, int count){
		csItems items = new csItems (item, count);
		if (materialList == null)
			materialList = new List<csItems> ();
		materialList.Add (items);
	}
}
