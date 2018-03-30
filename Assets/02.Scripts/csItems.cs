using System;
using UnityEngine;

// 아이템과 개수를 묶기위한 구조체
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
