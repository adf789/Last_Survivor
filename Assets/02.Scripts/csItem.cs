using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct csItem {
	private string name;
	private int id;
	private int type;
	private Sprite picture;

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
}
