using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 상태를 담는 클래스이다.
public class csCharacterStatus : MonoBehaviour {
	private static csCharacterStatus _instance = null;
	private static csInventory inventory;
	public bool isStop = false;

	public static csCharacterStatus Instance{
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(csCharacterStatus)) as csCharacterStatus;
				inventory = csInventory.Instance;
			}
			return _instance;
		}
	}

	public csInventory GetInventory(){
		return inventory;
	}

}
