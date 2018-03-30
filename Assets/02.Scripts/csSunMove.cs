using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 태양(Directional Light)을 제어하는 클래스
public class csSunMove : MonoBehaviour {
	private static csSunMove _instance;
	private Transform transform;
	[SerializeField]
	private Material[] skys;
	[SerializeField]
	private Transform moonPos;
	[SerializeField]
	private float Moon_Distance = 1000f;
	[SerializeField]
	private float Moon_Scale = 15f;
	[SerializeField]
	private float turnSpeed = 0.5f;
	private float curTime = 90f;
	private Skybox skybox;
	private float i = 200;

	public static csSunMove Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<csSunMove> ();
			}
			return _instance;
		}
	}

	// 태양의 위치와 같이 회전할 달의 위치를 초기화
	void Start () {
		skybox = GameObject.Find ("Character").transform.Find ("Main Camera").GetComponent<Skybox>();
		transform = gameObject.GetComponent<Transform> ();
		transform.position = new Vector3 (250f, 0f, 250f);
		moonPos.rotation = transform.rotation;
		moonPos.Rotate (Vector3.right, 180f);

		Transform moon = transform.Find ("Moon").GetComponent<Transform> ();
		moon.localScale = new Vector3 (Moon_Scale, Moon_Scale, Moon_Scale);
		moon.localPosition = new Vector3 (moonPos.localPosition.x, moonPos.localPosition.y, Moon_Distance);
	}
	
	// 태양을 turnSpeed 만큼 회전함
	// 그에 따라 게임상의 시간(curTime)이 흐름
	void Update () {
		if (curTime >= 360f) {
			curTime = 0f;
		} else {
			curTime += Time.deltaTime * turnSpeed;
		}
		// 해가 짐과 뜨는 시각에 하늘의 색상을 변경
		if ((CurTime > 18f && CurTime < 24f) || (CurTime > 6f && CurTime < 9f)) {
			ChangeSkyLight ();
		}
		Quaternion rot = Quaternion.Euler (0, 0, Time.deltaTime * 5);
		Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1, 1, 1));
		skybox.material.SetMatrix ("_Rotation", mat);
		transform.Rotate (Vector3.right, turnSpeed * Time.deltaTime);
	}

	// 하늘의 색상을 변경
	private void ChangeSkyLight(){
		bool day = false;
		// 해가 질 경우
		if (CurTime > 18f && CurTime < 21f) {
			day = false;
		}
		// 해가 뜰 경우
		else if (CurTime > 6f && CurTime < 9f) {
			day = true;
		}
		if (day) {
			i += turnSpeed * 0.5f;
			if (i > 190)
				i = 190;
		} else {
			i -= turnSpeed * 0.5f;
			if (i < 20f)
				i = 20f;
		}
		// 해가 회전하는 속력에 따라 하늘의 색이 변함
		skybox.material.SetColor ("_Tint", new Color(i/255f, i/255f, i/255f, 1f));
	}

	public float CurTime{
		get{
			return curTime / 15f;
		}
	}

	public float TurnSpeed{
		get{
			return turnSpeed;
		}
	}
}
	