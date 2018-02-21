using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct csItem {
	private string name;
	private int id;
	private int type;

	public csItem(string n, int i, int t){
		this.name = n;
		this.id = i;
		this.type = t;
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
}
