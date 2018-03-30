using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자유이동상태는 각각 상태가 달라야 하므로 싱글톤을 사용하지 않는다.
public class csPatrolState : csFSMstate<csMonster> {
	private float moveTimer = 0f;
	private float curMoveSpeed = 0f;
	private float curTimer = 0f;
	private int moveFlag = 0;

	// 자유이동 상태로 돌입할 때
	public override void EnterState (csMonster _Monster)
	{
		moveTimer = 0f;
		curTimer = 0f;
	}

	// 자유이동 상태에서 나올 때
	public override void ExitState (csMonster _Monster)
	{
		_Monster.anim.SetBool ("Idle", false);
	}

	// 자유이동 상태를 업데이트
	public override void UpdateState (csMonster _Monster)
	{
		// 몬스터가 땅을 밟고있지 않은 경우 중력을 가한다. 
		if (!_Monster.cc.isGrounded) {
			// 시간에 따라 현재 속도를 감속시킨다.
			curMoveSpeed -= Time.deltaTime;
			if (curMoveSpeed <= 0f) {
				curMoveSpeed = 0f;
			}

			// 중력의 방향과 땅을 밟았을 때 속도만큼 이동한다.
			Vector3 gravityDir = Vector3.down * _Monster.gravity;
			Vector3 monsterDir = _Monster.transform.forward * curMoveSpeed;
			_Monster.cc.Move ((gravityDir + monsterDir) * Time.deltaTime);
			return;
		}
		// 몬스터의 다음 행동을 지정한다.
		RandMove ();
		// 지정된 flag에 따라 행동을 달리한다.
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

	// 몬스터의 자유 이동을 설정한다.
	private void RandMove(){
		// 지정된 시간만큼 대기한다.
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

		// 해당 flag에 따라 대기시간을 달리한다.
		if (moveFlag == 0) {
			moveTimer = 10f;
		}if (moveFlag == 1) {
			moveTimer = 5f;
		}else {
			moveTimer = 7f;
		}
	}
}
