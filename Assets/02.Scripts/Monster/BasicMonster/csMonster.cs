using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMonster : MonoBehaviour {
	public float hp = 100f;
	private float curHp;
	public int monsterDamage = 0;
	public float attackRange = 2f;
	public float attackSpeed = 1.5f;
	public float chaseCancelTime = 3f;
	public float chaseTime = 0f;
	public float moveSpeed = 2.5f;
	public float rotSpeed = 5f;
	private Transform myTarget;
	public Transform transform;
	public Animator anim;
	public CharacterController cc;
	public float deadTimer = 0f;
	public readonly float gravity = 3f;

	private csStateMachine<csMonster> _State;
	private Coroutine missingCo;

	// Use this for initialization
	void Start () {
		curHp = hp;
		transform = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();
		_State = new csStateMachine<csMonster> ();
		_State.Init (this, csPatrolState.Instance);
		StartCoroutine ("CheckCurMovement");
	}
	
	// Update is called once per frame
	void Update () {
		_State.Update ();
	}

	public void ChangeState(csFSMstate<csMonster> newState){
		_State.ChangeState(newState);
	}

	public bool CheckAttackRange(){
		float _Distance = Vector3.Distance (myTarget.position, transform.position);
		if (_Distance <= attackRange)
			return true;
		else
			return false;
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			myTarget = col.transform;
			if(missingCo != null)
				StopCoroutine (missingCo);
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			missingCo = StartCoroutine ("MissingTarget");
		}
	}

	IEnumerator CheckCurMovement(){
		if (curHp <= 0f) {
			_State.ChangeState (csDieState.Instance);
		} else {
			if (myTarget != null) {
				if (CheckAttackRange ()) {
					_State.ChangeState (csAttackState.Instance);
					yield return new WaitForSeconds (2f);
				} else {
					_State.ChangeState (csChaseState.Instance);
					yield return new WaitForSeconds (0.5f);
				}
			} else {
				_State.ChangeState (csPatrolState.Instance);
				yield return new WaitForSeconds (1f);
			}
		}

		StartCoroutine ("CheckCurMovement");
	}

	IEnumerator MissingTarget(){
		yield return new WaitForSeconds (5f);
		myTarget = null;
	}

	public Transform MyTarget{
		get{
			return myTarget;
		}
	}

	public Color MonsterColor{
		set{
			transform.GetChild(0).GetComponent<Renderer> ().material.color = value;
		}get{
			return transform.GetChild(0).GetComponent<Renderer> ().material.color;
		}
	}
}
