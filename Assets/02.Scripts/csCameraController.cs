using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라를 제어하기 위한 클래스이다.
public class csCameraController : MonoBehaviour {
	private Transform thirdPos;
	private Transform charTransform;
	private Transform transform;
	private float mouseSensitive;
	private Vector3 wallPos;

	// 카메라 컨트롤러의 멈춤 유무를 결정하는 변수
	public static bool isStop;


	// 카메라의 초기 상태 지정
	void Start () {
		isStop = false;
		thirdPos = GameObject.Find ("ThirdCameraPos").transform;
		charTransform = FindObjectOfType<csCharacterController> ().transform;
		mouseSensitive = 80f;
		wallPos = new Vector3 ();
		transform = gameObject.GetComponent<Transform> ();
		transform.localPosition = thirdPos.localPosition;
		transform.LookAt (charTransform.position + charTransform.up);
		thirdPos.LookAt (charTransform.position + charTransform.up);
	}

	// 카메라의 회전, 벽 체크를 매 프레임마다 시행
	void Update () {
		if (isStop)
			return;
		CameraRotation ();
		CheckWall ();
	}

	// 카메라 회전을 하는 메소드
	private void CameraRotation(){
		float mouseY = Input.GetAxis ("Mouse Y");
		float mouseX = Input.GetAxis ("Mouse X");

		// 마우스의 X축 회전에 따른 캐릭터 회전.
		charTransform.Rotate (Vector3.up * mouseX * Time.deltaTime * mouseSensitive, Space.World);

		float cameraAngle = Vector3.Angle (charTransform.up, thirdPos.forward);

		// 3인칭의 움직임 범위를 제한한다.
		if ((cameraAngle > 145f && mouseY < 0f) || (cameraAngle < 20f && mouseY > 0f)) {
			return;
		}

		// 마우스의 Y축 회전에 따른 카메라 회전.
		// 빈 오브젝트를 회전시킨 후 카메라의 위치를 변경함.
		thirdPos.RotateAround(charTransform.position, charTransform.right, mouseSensitive * -mouseY * Time.deltaTime);
		thirdPos.LookAt (charTransform.position + charTransform.up);
		transform.localPosition = thirdPos.localPosition;
		transform.localRotation = thirdPos.localRotation;
	}

	// 플레이어에서 카메라로 Ray를 쏴서 방해되는 오브젝트가 있는지 판단한 후 카메라 위치조정
	private void CheckWall(){
		RaycastHit hit;
		Vector3 pos = transform.position;
		if (Physics.Linecast (charTransform.position, transform.position, out hit)) {
			if (hit.collider.tag.Equals ("Player"))
				return;
			// 탐지된 오브젝트가 지형이라면
			if (hit.collider.tag.Equals ("Ground")) {
				// 지형의 y축 좌표로 고정한다.
				wallPos.Set (pos.x, hit.point.y, pos.z);
			}
			// 그 외의 오브젝트라면
			else {
				// x, z축 좌표로 고정
				wallPos.Set (hit.point.x, pos.y, hit.point.z);
			}
			// 해당 좌표로 카메라를 이동한 후 플레이어 방향으로 회전함.
			transform.position = wallPos;
			transform.LookAt (charTransform.position + charTransform.up);
		}
	}
}
