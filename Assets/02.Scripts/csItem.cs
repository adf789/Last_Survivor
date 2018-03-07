using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public List<csItems> CombinationList{
		get{
			if (combList == null)
				combList = new List<csItems> ();
			return combList;
		}
	}

	public List<csItems> MaterialList{
		get{
			if (materialList == null)
				materialList = new List<csItems> ();
			return materialList;
		}
	}

	public void AddCombination(csItem item, int count){
		csItems items = new csItems (item, count);
		if (combList == null)
			combList = new List<csItems> ();
		combList.Add (items);
	}

	public void AddMaterial(csItem item, int count){
		csItems items = new csItems (item, count);
		if (materialList == null)
			materialList = new List<csItems> ();
		materialList.Add (items);
	}
}
