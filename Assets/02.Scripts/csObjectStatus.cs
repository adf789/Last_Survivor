using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectStatus : MonoBehaviour {
	[SerializeField]private float hp;
	private float curHp;
	private bool isDead;

	void Start () {
		isDead = false;
		curHp = hp;
	}

	public float GetHp{
		get{
			return curHp;
		}
	}

	public void DecreaseHp(float number){
		curHp -= number;
		Debug.Log (gameObject.name + " HP : " + curHp);
	}

	public void death(){
		death (0f);
	}

	public void death(float time){
		if (isDead)
			return;
		isDead = true;
		StartCoroutine (TimeForDeath (time));
	}

	private IEnumerator TimeForDeath(float time){
		yield return new WaitForSeconds (time);
		gameObject.SetActive (false);
	}
}
