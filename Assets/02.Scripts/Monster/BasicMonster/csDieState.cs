using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csDieState : csFSMstate<csMonster> {
	private static csDieState _instance = null;
	private float dieTimer = 0f;

	// 싱글톤을 위해 생성자는 private로 지정한다.
	private csDieState(){

	}

	public static csDieState Instance{
		get{
			if (_instance == null)
				_instance = new csDieState ();
			return _instance;
		}
	}

	// 죽는 상태로 돌입할 때
	public override void EnterState (csMonster _Monster)
	{
		csRespawn.Instance.CurMonsterCount = csRespawn.Instance.CurMonsterCount - 1;
		csInventory inv = csInventory.Instance;
		csItem item = csItemList.Instance.GetItem (2);
		inv.SetToInventory (item, 1);
	}

	// 죽는 상태에서 나올 때 상태
	public override void ExitState (csMonster _Monster)
	{
		dieTimer = 0f;
	}

	// 죽음 상태를 업데이트
	public override void UpdateState (csMonster _Monster)
	{
		// 시간이 3초 넘으면 몬스터의 오브젝트를 비활성화한다.
		if (dieTimer > 3f) {
			_Monster.gameObject.SetActive (false);
		} else {
			dieTimer += Time.deltaTime;
		}
	}
}
