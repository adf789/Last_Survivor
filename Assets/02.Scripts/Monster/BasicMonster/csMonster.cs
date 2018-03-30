using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csMonster : MonoBehaviour, csObjectInteraction{
	public float hp = 100f;
	public float curHp;
	public int damage = 5;
	public float attackRange = 1.0f;
	public float attackSpeed = 1.5f;
	public float chaseCancelTime = 3f;
	public float chaseTime = 0f;
	public float moveSpeed = 1.5f;
	public float rotSpeed = 4f;
	public Transform transform;
	public Animator anim;
	public CharacterController cc;
	public float deadTimer = 0f;
	public readonly float gravity = 3f;

	private Transform myTarget = null;
	private Transform hpBarT = null;
	private Slider hpBarSlider = null;
	private csStateMachine<csMonster> _State = null;
	private Coroutine missingCo = null;
	private csPatrolState patrolState;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		curHp = hp;
		patrolState = new csPatrolState ();
		transform = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();
		audioSource = GetComponent<AudioSource> ();
		_State = new csStateMachine<csMonster> ();
		_State.Init (this, patrolState);
		StartCoroutine ("CheckCurMovement");
	}
	
	// 현재 상태를 업데이트하고 체력바를 관리한다.
	void Update () {
		if (Vector3.Distance (transform.position, csCharacterStatus.Instance.transform.position) > 70f) {
			return;
		}
		_State.Update ();
		UpdateHp ();
	}

	// 현재 몬스터의 체력을 확인하며, 체력바의 위치를 몬스터의 위치로 설정한다.
	private void UpdateHp(){
		// 체력바 오브젝트가 null이 아닐 경우 진행한다.
		if (hpBarT == null) {
			return;
		}
		// 현재 체력바가 게임화면에 활성화되있고 타겟의 정면 방향과 몬스터의 정면 방향의 각이 70도 보다 낮을 경우
		// 체력바의 위치를 몬스터의 위치로 설정한다.
		if (myTarget != null && Vector3.Angle (myTarget.forward, transform.forward) > 70f) {
			hpBarT.position = Camera.main.WorldToScreenPoint (transform.position);
			hpBarSlider.value = curHp / hp;
		}
		// 몬스터의 체력이 0보다 낮은지 확인한다.
		if (curHp <= 0f) {
			// 몬스터의 상태를 죽음상태로 전환한다.
			_State.ChangeState (csDieState.Instance);

			// 현재 몬스터의 체력바를 비활성화한다.
			csMonsterHpBarManager.Instance.RemoveHpBar (hpBarT);
			hpBarT = null;
			hpBarSlider = null;
		}
	}

	// 몬스터의 상태를 매개변수로 받은 상태로 전환한다.
	public void ChangeState(csFSMstate<csMonster> newState){
		_State.ChangeState(newState);
	}

	// 몬스터의 위치와 타겟의 위치를 비교한 후 공격거리내에 있는지 판단한 후 결과 값을 반환한다.
	public bool CheckAttackRange(){
		float _Distance = Vector3.Distance (myTarget.position, transform.position);
		if (_Distance <= attackRange)
			return true;
		else
			return false;
	}

	void OnTriggerEnter(Collider col){
		// 트리거내에 플레이어가 있으면 그 플레이어를 타겟으로 초기화한다.
		if (col.tag == "Player") {
			myTarget = col.transform;
			// 타겟을 놓치기전에 다시 찾은경우 해당 코루틴을 정지한다.
			if(missingCo != null)
				StopCoroutine (missingCo);
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			// 플레이어가 트리거 밖으로 나간 경우 해당 코루틴을 실행한다.
			missingCo = StartCoroutine ("MissingTarget");
		}
	}

	// 몬스터의 행동 패턴을 지정한다.
	IEnumerator CheckCurMovement(){
		// 몬스터의 체력이 0보다 높은 경우 실행
		if(curHp > 0f) {
			// 타겟이 있고 플레이어가 자고있는 경우가 아닌 경우
			if (myTarget != null && !csCharacterStatus.Instance.isSleep) {
				// 타겟이 공격 범위내에 있다면
				if (CheckAttackRange ()) {
					// 몬스터의 상태를 공격상태로 전환한다.
					_State.ChangeState (csAttackState.Instance);
					// 다음 상태까지 3초 대기한다.
					yield return new WaitForSeconds (3f);
				}
				// 타겟이 공격 범위밖에 있다면
				else {
					// 몬스터의 상태를 추적상태로 전환한다.
					_State.ChangeState (csChaseState.Instance);
					// 다음 상태까지 0.5초 대기한다.
					yield return new WaitForSeconds (0.5f);
				}
			}
			// 타겟이 없거나 플레이어가 자는 경우
			else {
				// 몬스터의 상태를 자유이동 상태로 전환한다.
				_State.ChangeState (patrolState);

				// 몬스터의 체력바를 비활성화 한다.
				if (hpBarT != null) {
					csMonsterHpBarManager.Instance.RemoveHpBar (hpBarT);
					hpBarSlider = null;
					hpBarT = null;
				}
				// 다음 상태까지 1초 대기한다.
				yield return new WaitForSeconds (1f);
			}
			// 현재 코루틴을 계속 실행한다.
			StartCoroutine ("CheckCurMovement");
		}
	}

	// 주어진 대기시간 후에 타겟은 잃어버린다.
	IEnumerator MissingTarget(){
		yield return new WaitForSeconds (5f);
		myTarget = null;
	}

	public Transform MyTarget{
		get{
			return myTarget;
		}
	}

	// 몬스터의 상태에 따라 색상을 변경하거나, 현 몬스터의 색상을 반환한다.
	public Color MonsterColor{
		set{
			transform.GetChild(0).GetComponent<Renderer> ().material.color = value;
		}get{
			return transform.GetChild(0).GetComponent<Renderer> ().material.color;
		}
	}

	// 몬스터의 공격 범위내에 플레이어가 있고 타겟이 지정되어 있다면, 플레이어의 체력을 감소시킨다.
	public void TargetAttact(){
		if (CheckAttackRange () && myTarget != null) {
			csCharacterStatus.Instance.ChangeHp (-damage);
		}
	}

	// 몬스터에게 상호작용을 시도할 경우
	public void Interaction(GameObject obj){
		// 대상이 플레이어라면..
		if (obj.tag == "Player") {
			// 몬스터의 체력을 플레이어의 데미지만큼 감소한다.
			curHp -= csCharacterStatus.Instance.Damage;
			// 몬스터가 맞는 소리를 재생한다.
			audioSource.Play ();
			// 만약 체력바가 지정되어 있지 않은경우
			if (hpBarT == null) {
				// 체력바를 매니저에서 지정 받는다.
				hpBarT = csMonsterHpBarManager.Instance.GetHpBar ();
				hpBarSlider = hpBarT.GetComponent<Slider> ();
			}
			if (curHp < 0) {
				curHp = 0;
			}
		}
	}

	// 몬스터가 리스폰될 경우 해당 위치로 이동하며, 체력을 원래 상태로 복구한다.
	public void Respawn(Vector3 position){
		curHp = hp;
		ChangeState (patrolState);
		transform.localPosition = position;
	}
}
