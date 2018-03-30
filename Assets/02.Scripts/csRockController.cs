using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 바위 오브젝트를 제어하는 클래스
public class csRockController : MonoBehaviour, csObjectInteraction {
	public float hp = 1000f;
	public float curHp;
	private Animator anim;
	private SphereCollider col;
	private Rigidbody rigid;
	private AudioSource audioSource;

	void Start () {
		curHp = hp;
		anim = gameObject.GetComponent<Animator> ();
		col = gameObject.GetComponent<SphereCollider> ();
		rigid = gameObject.GetComponent<Rigidbody> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	public void Dig(){
		StartCoroutine ("actionDig");
	}

	// 바위 오브젝트를 비활성화
	private void BreakRock(){
		col.enabled = false;
		gameObject.SetActive (false);
	}

	// 도끼가 부딪힐 타이밍에 Rigidbody를 활성화한다.
	IEnumerator actionDig(){
		yield return new WaitForSeconds (0.5f);
		rigid.isKinematic = false;

	}

	// 해당 충돌체에 감지된 오브젝트의 태그가 Pickaxe인 경우 행동
	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Pickaxe") {
			curHp -= 1f;
			audioSource.Play ();
			// 물리적 충돌이 감지될 때마다 플레이어의 인벤토리로 지정된 아이템이 추가됨
			csInventory inv = csInventory.Instance;
			csItem item = csItemList.Instance.GetItem (1);
			inv.SetToInventory (item, 1);
			// 해당 오브젝트의 내구도가 0이하가 됐을 경우
			if (curHp <= 0f) {
				BreakRock ();
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
