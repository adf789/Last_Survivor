using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csChaseState : csFSMstate<csMonster> {
	private static csChaseState _instance;
	private float curMoveSpeed = 0f;
	private Color originColor;

	private csChaseState(){

	}

	public static csChaseState Instance{
		get{
			if (_instance == null) {
				_instance = new csChaseState ();
			}
			return _instance;
		}
	}

	public override void EnterState (csMonster _Monster)
	{
		originColor = _Monster.MonsterColor;
		_Monster.MonsterColor = Color.magenta;
		Debug.Log (_Monster.name + " 가 " + _Monster.MyTarget.name + " 추적 시작");
	}

	public override void ExitState (csMonster _Monster)
	{
		_Monster.MonsterColor = originColor;
		Debug.Log (_Monster.name + " 추적 종료 ");
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
		}
		// 몬스터가 지금 땅을 밟고있고 타겟이 있다면..
		else {
			if (_Monster.MyTarget != null) {
				Vector3 targetDir = _Monster.MyTarget.position - _Monster.transform.position;

				// 몬스터가 위,아래로 회전하지 못하게 함.
				targetDir.y = 0f;
				curMoveSpeed = _Monster.moveSpeed + 0.5f;
				_Monster.transform.rotation = Quaternion.RotateTowards(_Monster.transform.rotation, Quaternion.LookRotation (targetDir), 20f);
				_Monster.cc.Move (_Monster.transform.forward * curMoveSpeed * Time.deltaTime);
			}
		}
	}
}
