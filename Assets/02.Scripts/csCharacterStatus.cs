using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 상태를 담는 클래스이다.
public class csCharacterStatus : MonoBehaviour {
	private static csCharacterStatus _instance = null;
	private static csInventory inventory;
	private static csCharacterController charController;
	private int curTool = -1;
	private int damage = 5;

	public readonly float maxHp = 100;
	public float curHp = 100;
	public readonly float maxFatigue = 100;
	public float curFatigue = 100;
	public bool isStop = false, isSleep = false;

	// 싱글톤으로 객체를 반환할 때 초기 설정을 한다.
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

	// 해당 캐릭터의 인벤토리 객체를 반환한다.
	public csInventory GetInventory(){
		return inventory;
	}

	// string의 값에 따라 현재 착용할 도구를 설정한다.
	public void SetEquipment(string weapon){
		switch (weapon) {
		case "Axe":
			charController.chgWeapon (curTool, 0);
			curTool = 0;
			damage = 20;
			break;
		case "Pickaxe":
			charController.chgWeapon (curTool, 1);
			curTool = 1;
			damage = 20;
			break;
		default:
			charController.chgWeapon (curTool, -1);
			curTool = -1;
			damage = 5;
			break;
		}
	}

	// 현재 착용한 도구를 string으로 반환한다.
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

	public int Damage{
		get{
			return damage;
		}
	}

	// 매개변수로 넘어온 값 만큼 체력 수치를 변화한다.
	public void ChangeHp(int amount){
		curHp += amount;
		if (curHp > maxHp) {
			curHp = maxHp;
		} else if (curHp < 0) {
			curHp = 0;
		}
	}

	// 매개변수로 넘어온 값 만큼 배고픔 수치를 변화한다.
	public void ChangeFatigue(int amount){
		curFatigue += amount;
		if (curFatigue > maxFatigue) {
			curFatigue = maxFatigue;
		} else if (curFatigue < 0) {
			curFatigue = 0;
		}
	}
}
