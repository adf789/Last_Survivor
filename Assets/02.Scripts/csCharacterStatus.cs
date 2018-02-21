using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCharacterStatus : MonoBehaviour {
	private static csCharacterStatus _instance = null;
	private bool isLumber = false;

	public static csCharacterStatus Instance{
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(csCharacterStatus)) as csCharacterStatus;
			}
			return _instance;
		}
	}

	public bool IsLumber{
		get{
			return isLumber;
		}
		set{
			isLumber = value;
		}
	}
}
