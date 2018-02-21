using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct csItem {
	private string name;
	private int id;
	private int type;
	private Sprite picture;

	public csItem(string n, int i, int t, Sprite p){
		this.name = n;
		this.id = i;
		this.type = t;
		this.picture = p;
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
}
