using System;
using UnityEngine;

[Serializable]
public struct csItems {
	public enum Id{
		Empty = -1,
		Wood,
		Iron,
		Crystal,
		Meat,
		Axe,
		Pickaxe,
		Shovel,
		Tent
	}

	public Id id;
	public int count;

	public csItems(csItem item, int count){
		this.id = (Id)item.Id;
		this.count = count;
	}
}
