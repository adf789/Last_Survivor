using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 바위 오브젝트를 제어하는 클래스
public class csRockController : MonoBehaviour, csObjectInteraction {
	public float hp = 1000f;
	public float curHp;
	private SphereCollider col;
	private AudioSource audioSource;

	void Start () {
		curHp = hp;
		col = gameObject.GetComponent<SphereCollider> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	// 바위 오브젝트를 비활성화
	private void BreakRock(){
		col.enabled = false;
		gameObject.SetActive (false);
	}

	// 해당 오브젝트에 상호작용의 발생된 경우 호출된다.
	public void Interaction(GameObject gameObject){
		// 현재 캐릭터가 들고있는 도구가 Pickaxe인 경우
		if (csCharacterStatus.Instance.CurrentEquip ().Equals ("Pickaxe")) {
			// 해당 오브젝트의 내구도를 1깎는다.
			curHp -= 1f;
			// 상호작용에 해당하는 소리 재생
			audioSource.Play ();
			// 플레이어의 인벤토리로 지정된 아이템이 추가됨
			csInventory inv = csInventory.Instance;
			csItem item = csItemList.Instance.GetItem (1);
			inv.SetToInventory (item, 1);
			// 해당 오브젝트의 내구도가 0이하가 됐을 경우
			if (curHp <= 0f) {
				BreakRock ();
			}
		} else {
			csMessageBox.Show ("알맞은 도구가 아닙니다.");
		}
	}

	public void Respawn(Vector3 position){

	}
}
