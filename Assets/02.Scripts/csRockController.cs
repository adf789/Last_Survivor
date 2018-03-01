using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRockController : MonoBehaviour {
	private csObjectStatus stats;
	private Animator anim;
	private SphereCollider col;
	private Rigidbody rigid;
	private AudioSource audioSource;

	void Start () {
		stats = gameObject.GetComponent<csObjectStatus> ();
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<SphereCollider> ();
		rigid = gameObject.GetComponent<Rigidbody> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	public void Dig(){
		StartCoroutine ("actionDig");
	}

	private void BreakRock(){
		col.enabled = false;
//		anim.SetBool ("Lumber", true);
		stats.death (2.5f);
	}

	// 도끼가 부딪힐 타이밍에 Rigidbody를 활성화한다.
	IEnumerator actionDig(){
		yield return new WaitForSeconds (0.5f);
		rigid.isKinematic = false;

	}

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Pickaxe") {
			stats.DecreaseHp (1f);
			audioSource.Play ();
			csInventory inv = csInventory.Instance;
			csItem item = csItemList.Instance.GetItem (Random.Range(1, 3));
			inv.SetToInventory (item, 1);
			if (stats.GetHp <= 0f) {
				BreakRock ();
			}
			rigid.isKinematic = true;
		}
	}
}
