using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터를 제어하기 위한 클래스이다.
public class csCharacterController : MonoBehaviour {
	private CharacterController cc;
	private csCharacterStatus charStats;
	private Transform transform;
	[SerializeField]private Transform cameraLocation;
	[SerializeField]private Transform playerModel;
	[SerializeField]private GameObject[] thirdWeapons;
	[SerializeField]private Transform attackEffectT;
	[SerializeField]private Transform takeEffectT;
	private ParticleSystem attackPS;
	private ParticleSystem takePS;
	private Animator thirdAnim;
	private Vector3 charDir;
	private float gravity, moveSpeed, jumpSpeed, jumpLocation, curMoveSpeed, fatTimer;
	private Collider target;

	// 오브젝트 및 변수 초기화
	void Awake() {
		transform = gameObject.GetComponent<Transform> ();
		cc = gameObject.GetComponent<CharacterController> ();
		charStats = csCharacterStatus.Instance;
		attackPS = attackEffectT.GetComponent<ParticleSystem> ();
		takePS = takeEffectT.GetComponent<ParticleSystem> ();
		thirdAnim = gameObject.GetComponent<Animator> ();
		Init ();
	}

	void Init(){
		// 초기 조건 설정
		charDir = Vector3.zero;
		gravity = 3f;
		moveSpeed = 1f;
		jumpSpeed = 0.8f;
		jumpLocation = 0f;
		curMoveSpeed = 0f;
		fatTimer = 0f;
	}

	// 캐릭터의 움직임 유무를 확인하고 배고픔감소, 이동, 상호작용을 한다.
	void Update () {
		if (charStats.isStop)
			return;
		DecreaseFat ();
		// 앞의 오브젝트와 상호작용하기 위해선 건설상태가 아니고 점프상태가 아닐 때 F키를 눌렀을 때 동작한다.
		if (cc.isGrounded && !csBuilding.Instance.IsBuilding && Input.GetKeyDown (KeyCode.F)) {
			CharStartInteration();
		}
		CharMove ();

	}

	void FixedUpdate(){
		if (charStats.isStop)
			return;
		// 캐릭터의 실제 이동은 FixedUpdate에서 이뤄진다.
		jumpLocation += gravity * Time.deltaTime;
		charDir += Vector3.down * jumpLocation;
		cc.Move (charDir * Time.deltaTime);
	}

	// 시간이 지남에 따라 배고픔이 감소된다.
	private void DecreaseFat(){
		if (fatTimer > 10f) {
			fatTimer = 0f;
			charStats.ChangeFatigue (-3);
		} else {
			fatTimer += Time.deltaTime;
		}
	}

	// 앞의 대상을 판단하여 반환한다.
	private Collider CheckTarget(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (csAlreadyGame.FocusObj.position);

		// 현재 인칭에 따라 체크 범위를 달리 한다.
		float distance = 2.5f;

		// Ray를 쏴서 상호작용 가능한 물체를 구분하며, 상호작용한다.
		if(Physics.Raycast(ray, out hit, distance)){
			// 상호작용의 이펙트를 발생시킬 위치를 지정한다.
			// 몬스터의 경우와 그 외의 경우로 나뉨
			if (hit.collider.CompareTag ("Monster")) {
				attackEffectT.position = hit.point;
				attackEffectT.rotation = Quaternion.LookRotation (transform.position);
			} else {
				takeEffectT.position = hit.point;
				takeEffectT.rotation = Quaternion.LookRotation (transform.position);
			}
			return hit.collider;
		}
		return null;
	}

	private void CharStartInteration(){
		// 처음 앞의 대상이 무슨 오브젝트인지 판단.
		target = CheckTarget ();

		csObjectInteraction component = null;
		// 앞에 오브젝트가 있으면, 오브젝트의 csObjectInteraction 인터페이스를 GetComponent함.
		if (target != null) {
			component = target.GetComponent (typeof(csObjectInteraction)) as csObjectInteraction;
		}
		// 만약 앞의 오브젝트가 상자라면 Interaction 메소드만 호출함.
		if (target != null && target.CompareTag("Crate")) {
			if (component != null) {
				component.Interaction (gameObject);
			}

		} 
		// 그 외의 오브젝트라면 현재 지정된 Animator의 Lumbering 애니메이션을 활성화한 후 코루틴 CancelLumber를 호출함.
 		// 만약 아무 오브젝트도 없을 시 애니메이션만 실행
		else {
			thirdAnim.CrossFade("Lumbering", 0.3f);
			charStats.isStop = true;
			StartCoroutine ("CancelLumber");
		}
	}

	// 일정시간 후 Lumbering 애니메이션 조건을 비활성화 후 0.5초 후에 캐릭터의 멈춤상태도 비활성화한다.
	private IEnumerator CancelLumber(){
		yield return new WaitForSeconds (0.5f);
		thirdAnim.SetBool("Lumber", false);
		yield return new WaitForSeconds (0.5f);
		charStats.isStop = false;
	}

	// Lumbering 애니메이션 중 이벤트로 들어갈 메소드로 상호작용 타겟이 지형이 아니라면 상황에 맞는 이펙트와 메소드를 호출한다.
	private void ActInteration(){
		// 판단된 오브젝트가 지형이 아닌 다른 오브젝트인 경우
		if (target != null && !(target.CompareTag("Ground"))) {
			csObjectInteraction component = target.GetComponent (typeof(csObjectInteraction)) as csObjectInteraction;
			// 해당 오브젝트의 Interaction 메소드를 호출
			component.Interaction (gameObject);
			// 몬스터의 경우 attackPS 이펙트 재생
			if (target.CompareTag ("Monster")) {
				attackPS.Stop ();
				attackPS.Play ();
			}
			// 그 외의 경우 takePS 이펙트 재생
			else if(target.CompareTag("Resource")){
				takePS.Stop ();
				takePS.Play ();
			}
		}
	}

	// 사용자의 방향키 입력으로 캐릭터의 움직임을 조작한다.
	private void CharMove(){
		float hor = Input.GetAxis ("Horizontal");
		float ver = Input.GetAxis ("Vertical");
		// 캐릭터의 행동을 위해 중력과 입력받은 방향을 합친 방향으로 이동한다.
		// 또한 캐릭터가 땅을 밟았을 때 중력의 영향은 받지 않는다.
		if (cc.isGrounded) {
			// 방향키 입력으로 캐릭터를 움직이며, 설정된 Jump키의 조작으로 캐릭터가 점프유무를 판단한다.
			jumpLocation = 0f;
			charDir = (transform.forward * ver + transform.right * hor) * moveSpeed;
			thirdAnim.SetBool ("Jump", false);
			if (Input.GetButtonDown ("Jump")) {
				jumpLocation = -jumpSpeed;
				thirdAnim.SetBool ("Jump", true);
			}

			// 'R'키가 눌러진 상태에서 이동하는 경우 달리기를 한다.
			// moveSpeed를 변경하여 캐릭터의 속도를 빠르게 한다.
			// 좌우키가 눌려진 상태의 달리기는 속도제한이 더 높다.
			float maxSpeed = 4f;
			if (hor != 0f || ver < 0f) {
				maxSpeed = 2f;
			}
			if(!Input.GetButton("Run") || (ver == 0f && hor == 0f)) {
				if (moveSpeed > 1f && moveSpeed < maxSpeed) {
					moveSpeed -= 0.1f;
				} else {
					moveSpeed = 1f;
				}
				if (curMoveSpeed > 0f) {
					curMoveSpeed -= 1f;
				}
				thirdAnim.SetFloat ("Run", curMoveSpeed);
			}
			else {
				// moveSpeed : 캐릭터와 관련된 속도
				if (moveSpeed < maxSpeed) {
					moveSpeed += 0.1f;
				} else {
					moveSpeed = maxSpeed;
				}
				// curMoveSpeed : 애니메이션 전환에 관련된 속도
				if (curMoveSpeed < 20f)
					curMoveSpeed += 1f;
				thirdAnim.SetFloat ("Run", curMoveSpeed);
			}
		}

		// 현재 방향키가 눌러진 방향으로 캐릭터의 애니메이션을 전환한다.
		WalkDirAnim (hor, ver);
	}

	private void WalkDirAnim(float hor, float ver){
		// 현재 입력된 방향키에 따라 캐릭터의 방향을 전환한다.
		thirdAnim.SetFloat ("Walk", ver);
		thirdAnim.SetFloat ("SideWalk", hor);
		if (ver == 0 && hor == 0) {
			thirdAnim.SetBool ("Move", false);
			return;
		}
		if (!thirdAnim.GetBool ("Move")) {
			thirdAnim.CrossFade ("Moving", 0.3f);
			thirdAnim.SetBool ("Move", true);
		}
	}

	// 현재 캐릭터의 손에 들 아이템을 now 인덱스의 아이템으로 변경
	// now가 -1인 경우 빈 손
	public void chgWeapon(int prev, int now){
		if (prev != -1)
			thirdWeapons [prev].SetActive (false);
		if (now != -1)
			thirdWeapons [now].SetActive (true);
	}
}
