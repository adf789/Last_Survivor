using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPatrolState : csFSMstate<csMonster> {
	private static csPatrolState _instance = null;
	private float moveTimer = 0f;
	private float curMoveSpeed = 0f;
	private float curTimer = 0f;
	private int moveFlag = 0;

	private csPatrolState(){

	}

	public static csPatrolState Instance{
		get{
			if (_instance == null)
				_instance = new csPatrolState ();
			return _instance;
		}
	}

	public override void EnterState (csMonster _Monster)
	{
		moveTimer = 0f;
		curTimer = 0f;
		Debug.Log (_Monster.name + " 이동 시작");
	}

	public override void ExitState (csMonster _Monster)
	{
		_Monster.anim.SetBool ("Idle", false);
		Debug.Log (_Monster.name + " 이동 종료");
	}

	public override void UpdateState (csMonster _Monster)
	{
		if (!_Monster.cc.isGrounded) {
			curMoveSpeed -= Time.deltaTime;
			if (curMoveSpeed <= 0f) {
				curMoveSpeed = 0f;
			}
			Vector3 gravityDir = Vector3.down * _Monster.gravity;
			Vector3 monsterDir = _Monster.transform.forward * curMoveSpeed;
			_Monster.cc.Move ((gravityDir + monsterDir) * Time.deltaTime);
			return;
		}
		RandMove ();
		switch (moveFlag) {
		// Idle
		case 0:
			_Monster.anim.SetBool ("Idle", true);
			break;
		// Move to Forward
		case 1:
			_Monster.anim.SetBool ("Idle", false);
			curMoveSpeed = _Monster.moveSpeed;
			_Monster.cc.Move (_Monster.transform.forward * curMoveSpeed * Time.deltaTime);
			break;
		// Rotate to Left
		case 2:
			_Monster.anim.SetBool ("Idle", true);
			_Monster.transform.Rotate (Vector3.up * -_Monster.rotSpeed * Time.deltaTime, Space.World);
			break;
		// Rotate to Right
		case 3:
			_Monster.anim.SetBool ("Idle", true);
			_Monster.transform.Rotate (Vector3.up * _Monster.rotSpeed * Time.deltaTime, Space.World);
			break;
		}
	}

	private void RandMove(){
		if (curTimer < moveTimer) {
			curTimer += Time.deltaTime;
			return;
		}
		curTimer = 0f;
		// moveFlag는 현재 움직임을 알려주는 변수이다.
		// 0 : Idle
		// 1 : Move to Forward
		// 2 : Rotate to Left
		// 3 : Rotate to Right
		moveFlag = Random.Range (0, 4);
		if (moveFlag == 0) {
			moveTimer = 3f;
		}if (moveFlag == 1) {
			moveTimer = 2f;
		}else {
			moveTimer = 1f;
		}
	}
}
