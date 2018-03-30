using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csChaseState : csFSMstate<csMonster> {
	private static csChaseState _instance;
	private float curMoveSpeed = 0f;
	private float curJumpLocation = 0f;
	private Color originColor;
	private float jumpTimer = 0f;

	// 싱글톤을 위해 생성자는 private로 지정한다.
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

	// 추적 상태에 돌입할 때
	public override void EnterState (csMonster _Monster)
	{
		originColor = _Monster.MonsterColor;

		// 추적 상태일 때 몬스터의 색을 바꾼다.
		_Monster.MonsterColor = Color.magenta;
	}

	// 추적 상태에서 나올 때
	public override void ExitState (csMonster _Monster)
	{
		// 추적 상태에서 나올 때 몬스터의 색을 원래대로 바꾼다.
		_Monster.MonsterColor = originColor;
	}

	// 추적 상태를 업데이트
	public override void UpdateState (csMonster _Monster)
	{
		// 몬스터가 땅을 밟고있지 않은 경우 중력을 가한다. 
		if (!_Monster.cc.isGrounded) {
			// 시간에 따라 현재 속도를 감속시킨다.
			curMoveSpeed -= Time.deltaTime;
			if (curMoveSpeed <= 0f) {
				curMoveSpeed = 0f;
			}

			// 현재 점프한 위치에서 중력만큼 감소한다.
			curJumpLocation += _Monster.gravity;
			if (curJumpLocation > _Monster.gravity) {
				curJumpLocation = _Monster.gravity;
			}
			// 중력의 방향과 땅을 밟았을 때 속도만큼 이동한다.
			Vector3 gravityDir = Vector3.down * curJumpLocation;
			Vector3 monsterDir = _Monster.transform.forward * curMoveSpeed;
			_Monster.cc.Move ((gravityDir + monsterDir) * Time.deltaTime);
		}
		// 몬스터가 지금 땅을 밟고있고 타겟이 있다면..
		else {
			if (_Monster.MyTarget != null) {
				// 몬스터가 추적하면서 플레이어의 예상 경로를 목표로 전진한다.
				float expectAngle = 0f;
				// 몬스터의 위치와 타겟의 거리가 몬스터의 공격 범위 + 1 보다 큰 경우에만 타겟의 예상 경로로 회전한다.
				// 작은 경우는 타겟의 방향으로 회전한다.
				if (Vector3.Distance (_Monster.MyTarget.position, _Monster.transform.position) >= _Monster.attackRange + 1f) {
					expectAngle = Vector3.Dot (_Monster.MyTarget.forward, _Monster.transform.forward);
					expectAngle = Mathf.Abs (expectAngle) * 1.3f;
				}
				Vector3 targetDir = _Monster.MyTarget.position + _Monster.MyTarget.forward * expectAngle - _Monster.transform.position;

				// 몬스터가 위,아래로 회전하지 못하게 함.
				targetDir.y = 0f;

				// 추적 중에는 몬스터의 속도를 증가시킨다.
				curMoveSpeed = _Monster.moveSpeed + 0.5f;
				curJumpLocation = 0f;

				// 3초마다 몬스터의 앞에 장애물이 있는지 체크한 후 장애물이 있다면 점프한다.
				if (jumpTimer > 3f) {
					jumpTimer = 0f;
					if (!CheckPath (_Monster.transform)) {
						curJumpLocation = -10f;
					}
				} else {
					jumpTimer += Time.deltaTime;
				}

				// 타겟의 예상 방향으로 회전하며, 전진한다.
				_Monster.transform.rotation = Quaternion.RotateTowards(_Monster.transform.rotation, Quaternion.LookRotation (targetDir), 20f);
				_Monster.cc.Move ((_Monster.transform.forward * curMoveSpeed - _Monster.transform.up * curJumpLocation) * Time.deltaTime);
			}
		}
	}

	// 앞에 장애물이 있는지 체크한다.
	private bool CheckPath(Transform pos){
		if (Physics.Raycast (pos.position, pos.forward, 1f)) {
			return false;
//		} else if (Physics.Raycast (pos.position, pos.forward + pos.right, 1.4f)) {
//			return false;
//		} else if (Physics.Raycast (pos.position, pos.forward - pos.right, 1.4f)) {
//			return false;
		}
		return true;
	}

}
