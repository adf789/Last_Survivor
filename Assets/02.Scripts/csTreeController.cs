using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 나무를 제어하기 위한 클래스이다.
public class csTreeController : MonoBehaviour, csObjectInteraction {
	public float hp = 5f;
	public float curHp;
	private Animator anim;
	private BoxCollider col;
	private AudioSource audioSource;

	void Start () {
		curHp = hp;
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<BoxCollider> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// 나무 오브젝트를 비활성화한 후 특정 아이템을 습득
	private void CutTree(){
		col.enabled = false;
		anim.SetBool ("Lumber", true);
		csInventory inv = csInventory.Instance;
		csItem item = csItemList.Instance.GetItem (0);
		inv.SetToInventory (item, 3);
		StartCoroutine ("Disable");
	}

	IEnumerator Disable(){
		yield return new WaitForSeconds (2f);
		gameObject.SetActive (false);
	}

	// 해당 오브젝트에 상호작용의 발생된 경우 호출된다.
	public void Interaction(GameObject gameObject){
		// 현재 캐릭터가 들고있는 도구가 Axe인 경우
		if (csCharacterStatus.Instance.CurrentEquip ().Equals ("Axe")) {
			// 해당 오브젝트의 내구도를 1깎는다.
			curHp -= 1f;
			// 상호작용에 해당하는 소리 재생
			audioSource.Play ();
			// 해당 오브젝트의 내구도가 0이하가 됐을 경우
			if (curHp <= 0f) {
				CutTree ();
			}
		} else {
			csMessageBox.Show ("알맞은 도구가 아닙니다.");
		}
	}

	public void Respawn(Vector3 position){

	}
}
