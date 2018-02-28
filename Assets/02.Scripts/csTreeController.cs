using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 나무를 제어하기 위한 클래스이다.
public class csTreeController : MonoBehaviour {
	private csObjectStatus stats;
	private Animator anim;
	private BoxCollider col;
	private Rigidbody rigid;

	void Start () {
		stats = gameObject.GetComponent<csObjectStatus> ();
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<BoxCollider> ();
		rigid = gameObject.GetComponent<Rigidbody> ();
	}
	
	public void Lumber(){
		StartCoroutine ("actionLumber");
	}

	private void CutTree(){
		col.enabled = false;
		anim.SetBool ("Lumber", true);
		stats.death (2.5f);
		csInventory inv = csInventory.Instance;
		csItem item = csItemList.Instance.GetItem (0);
		inv.SetToInventory (item, 3);
	}

	// 도끼가 부딪힐 타이밍에 Rigidbody를 활성화한다.
	IEnumerator actionLumber(){
		yield return new WaitForSeconds (0.5f);
		rigid.isKinematic = false;

	}

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Axe") {
			stats.DecreaseHp (1f);
			if (stats.GetHp <= 0f) {
				CutTree ();
			}
			rigid.isKinematic = true;
		}
	}
}
