using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAttackState : csFSMstate<csMonster> {
	private static csAttackState _instance = null;
	private float attackTimer = 0f;
	private Color originColor;

	private csAttackState(){

	}

	public static csAttackState Instance{
		get{
			if (_instance == null)
				_instance = new csAttackState ();
			return _instance;
		}
	}



	public override void EnterState (csMonster _Monster)
	{
		_Monster.transform.GetChild (0).GetComponent<CapsuleCollider> ().isTrigger = true;
		_Monster.anim.SetBool ("Attack", true);
		originColor = _Monster.MonsterColor;
		_Monster.MonsterColor = Color.red;
		Debug.Log (_Monster.name + " 공격 시작");
	}

	public override void ExitState (csMonster _Monster)
	{
		_Monster.transform.GetChild (0).GetComponent<CapsuleCollider> ().isTrigger = false;
		_Monster.anim.SetBool ("Attack", false);
		_Monster.MonsterColor = originColor;
		Debug.Log (_Monster.name + " 공격 종료");
	}

	public override void UpdateState (csMonster _Monster)
	{
		Vector3 targetDir = _Monster.MyTarget.position - _Monster.transform.position;

		// 몬스터가 위,아래로 회전하지 못하게 함.
		targetDir.y = 0f;
		_Monster.transform.rotation = Quaternion.LookRotation (targetDir * _Monster.rotSpeed * Time.deltaTime);
	}
}
