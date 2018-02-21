using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 나무를 제어하기 위한 클래스이다.
public class csTreeController : MonoBehaviour {
	private csObjectStatus stats;
	private Animator anim;
	private BoxCollider col;

	void Start () {
		stats = gameObject.GetComponent<csObjectStatus> ();
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<BoxCollider> ();
	}
	
	public void Lumber(){
		stats.DecreaseHp (20f);
		if (stats.GetHp <= 0f) {
			col.enabled = false;
			anim.SetBool ("Lumber", true);
			stats.death (2.5f);
			csInventory inv = csInventory.Instance;
			csItem item = csItemList.Instance.GetItem (0);
			inv.SetToInventory (item, 3);
		}
	}
}
