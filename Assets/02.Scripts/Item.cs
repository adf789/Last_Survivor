using System;
using UnityEngine;

[Serializable]
public struct Item {
	public enum Id{
		Empty = -1,
		Wood,
		Iron,
		Crystal,
		Meat,
		Axe,
		Pickaxe,
		Shovel
	}

	public Id id;
	public int count;
}
