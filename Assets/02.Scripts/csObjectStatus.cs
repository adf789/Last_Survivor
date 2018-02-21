using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectStatus : MonoBehaviour {
	[SerializeField]private float hp;
	private bool isDead;

	void Start () {
		isDead = false;
	}

	public float GetHp{
		get{
			return hp;
		}
	}

	public void DecreaseHp(float number){
		hp -= number;
		Debug.Log (gameObject.name + " HP : " + hp);
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
