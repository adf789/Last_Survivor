using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 상태를 담는 클래스이다.
public class csCharacterStatus : MonoBehaviour {
	private static csCharacterStatus _instance = null;
	private static csInventory inventory;
	private static csCharacterController charController;
	private int curTool = -1;
	public bool isStop = false;

	public static csCharacterStatus Instance{
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(csCharacterStatus)) as csCharacterStatus;
				inventory = csInventory.Instance;
				charController = GameObject.FindObjectOfType (typeof(csCharacterController)) as csCharacterController;
			}
			return _instance;
		}
	}

	public csInventory GetInventory(){
		return inventory;
	}

	public void SetEquipment(string weapon){
		switch (weapon) {
		case "Axe":
			charController.chgWeapon (curTool, 0);
			curTool = 0;
			break;
		case "Pickaxe":
			charController.chgWeapon (curTool, 1);
			curTool = 1;
			break;
		default:
			charController.chgWeapon (curTool, -1);
			curTool = -1;
			break;
		}
	}

	public string CurrentEquip(){
		switch (curTool) {
		case 0:
			return "Axe";
		case 1:
			return "Pickaxe";
		default:
			return "Empty";
		}
	}
}
