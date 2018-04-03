using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAttackState : csFSMstate<csMonster> {
	private static csAttackState _instance = null;
	private Color originColor;

	// 싱글톤을 위해 생성자는 private로 지정한다.
	private csAttackState(){

	}

	public static csAttackState Instance{
		get{
			if (_instance == null)
				_instance = new csAttackState ();
			return _instance;
		}
	}

	// 공격 상태에 돌입할 때
	public override void EnterState (csMonster _Monster)
	{
		// 몬스터의 공격 애니메이션을 실행하기위한 조건 활성화
		_Monster.anim.SetBool ("Attack", true);
		originColor = _Monster.MonsterColor;
		_Monster.MonsterColor = Color.red;
	}

	// 공격 상태에서 나올 때
	public override void ExitState (csMonster _Monster)
	{
		// 몬스터의 공격 애니메이션을 실행하기위한 조건 활성화
		_Monster.anim.SetBool ("Attack", false);
		_Monster.MonsterColor = originColor;
	}

	// 공격 상태를 업데이트
	public override void UpdateState (csMonster _Monster)
	{
		
		// 타겟의 방향 지정
		Vector3 targetDir = _Monster.MyTarget.position - _Monster.transform.position;

		// 몬스터가 위,아래로 회전하지 못하게 함.
		targetDir.y = 0f;

		// 몬스터의 방향을 타겟을 바라보는 방향으로 회전
		_Monster.transform.rotation = Quaternion.LookRotation (targetDir * _Monster.rotSpeed * 0.7f * Time.deltaTime);
	}
}
