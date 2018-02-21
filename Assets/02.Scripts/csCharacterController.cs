using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터를 제어하기 위한 클래스이다.
public class csCharacterController : MonoBehaviour {
	private CharacterController cc;
	private csCharacterStatus charStats;
	[SerializeField]private Transform cameraLocation;
	[SerializeField]private Transform playerModel;
	private Animator anim;
	private Vector3 charDir;
	private float gravity;
	private float mouseSensitive;
	private float moveSpeed;
	private float jumpSpeed;
	private float jumpLocation;
	private float curMoveSpeed;

	void Awake() {
		cc = gameObject.GetComponent<CharacterController> ();
		charStats = csCharacterStatus.Instance;
		anim = gameObject.GetComponent<Animator> ();
		Init ();
	}

	void Init(){
		// 초기 조건 설정
		charDir = Vector3.zero;
		gravity = 3f;
		mouseSensitive = 80f;
		moveSpeed = 1f;
		jumpSpeed = 0.8f;
		jumpLocation = 0f;
		curMoveSpeed = 0f;
	}

	void Update () {
		if (charStats.isStop)
			return;
		CharMove ();
		CharRot ();
		if(Input.GetKeyDown(KeyCode.F) && isPlayLumbering())
			StartCoroutine ("CharLumber");

	}

	private bool isPlayLumbering(){
		// Lumbering 하는 애니메이터 조건 Active, 현재 Lumbering이 실행중이면 실행하지 않음.
		RaycastHit hit;
		Debug.DrawRay (transform.position + transform.up / 2f, transform.forward, Color.red);
		if(Physics.Raycast(transform.position + transform.up / 2f, transform.forward, out hit, 1f)){
			if (hit.collider.tag == "Tree" && !anim.GetCurrentAnimatorStateInfo (0).IsName ("Lumbering")) {
				hit.collider.GetComponent<csTreeController> ().Lumber ();
				return true;
			}
		}
		return false;
	}

	private IEnumerator CharLumber(){
		// Lumbering 애니메이션 재생
		anim.SetBool ("Lumber", true);
		charStats.isStop = true;

		yield return new WaitForSeconds (0.5f);

		anim.SetBool ("Lumber", false);
		charStats.isStop = false;
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
			if (Input.GetButtonDown ("Jump")) {
				jumpLocation = -jumpSpeed;
			}

			// 'R'키가 눌러진 상태에서 이동하는 경우 moveSpeed를 변경하여 캐릭터의 속도를 빠르게 한다.
			if (Input.GetButton("Run")) {
				if (moveSpeed < 3f)
					moveSpeed += 0.1f;
				if (curMoveSpeed < 20f)
					curMoveSpeed += 1f;
				anim.SetFloat ("Run", curMoveSpeed);
			} else {
				if (moveSpeed > 1f)
					moveSpeed -= 0.1f;
				if (curMoveSpeed > 0f)
					curMoveSpeed -= 1f;
				anim.SetFloat ("Run", curMoveSpeed);
			}
		}

		// 현재 방향키가 눌러진 방향과 중력을 더한 방향으로 캐릭터를 이동시킨다.
		jumpLocation += gravity * Time.deltaTime;
		charDir += Vector3.down * jumpLocation;
		WalkDirAnim (hor, ver);
		cc.Move (charDir * Time.deltaTime);
	}

	private void WalkDirAnim(float hor, float ver){
		// 현재 입력된 방향키에 따라 캐릭터의 방향을 전환한다.
		if (ver == 0 && hor == 0) {
			anim.SetFloat ("Walk", 0);
			return;
		}
		anim.SetFloat ("Walk", 1);
	}

	// 사용자의 마우스 움직임으로 캐릭터의 방향을 회전한다.
	private void CharRot(){
		float mouseX = Input.GetAxis ("Mouse X");

		// 마우스의 X축 회전에 따른 캐릭터 회전.
		transform.Rotate (Vector3.up * mouseX * Time.deltaTime * mouseSensitive, Space.World);

	}

	public float GetSensetive{
		get{
			return mouseSensitive;
		}
	}
}
