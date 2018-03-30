using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 나무를 제어하기 위한 클래스이다.
public class csTreeController : MonoBehaviour, csObjectInteraction {
	public float hp = 5f;
	public float curHp;
	private Animator anim;
	private BoxCollider col;
	private Rigidbody rigid;
	private AudioSource audioSource;

	void Start () {
		curHp = hp;
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<BoxCollider> ();
		rigid = gameObject.GetComponent<Rigidbody> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	public void Lumber(){
		StartCoroutine ("actionLumber");
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

	// 일정 시간 후 Rigidbody의 물리 충돌을 비활성화한다.
	IEnumerator actionLumber(){
		yield return new WaitForSeconds (1f);
		rigid.isKinematic = true;

	}

	// 해당 충돌체에 감지된 오브젝트의 태그가 Axe인 경우 행동
	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Axe") {
			curHp -= 1f;
			audioSource.Play ();
			// 해당 오브젝트의 내구도가 0이하가 됐을 경우
			if (curHp <= 0f) {
				CutTree ();
			}
			// rigidbody의 물리적 충돌 비활성화
			rigid.isKinematic = true;
		}
	}

	// 해당 오브젝트에 상호작용의 발생된 경우 rigidbody 물리적 충돌 활성화
	// 게임의 성능을 위해 필요한 경우에만 물리적 충돌 허용
	public void Interaction(GameObject gameObject){
		rigid.isKinematic = false;
	}

	public void Respawn(Vector3 position){

	}
}
