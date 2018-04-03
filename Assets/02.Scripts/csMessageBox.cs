using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임상에서 표시될 임시 메세지를 정의하는 클래스
public class csMessageBox : MonoBehaviour{
	private static GameObject messageBox = null;
	private static Text msgText = null;
	private static csMessageBox _instance = null;

	void Start(){
		messageBox = GameObject.Find ("Canvas").transform.Find ("MessageBox").gameObject;
		msgText = messageBox.transform.GetChild (0).GetComponent<Text> ();
		messageBox.SetActive (false);
	}

	public static void Show(string msg){
		Show (msg, 2f);
	}

	// 매개변수로 받은 string을 게임상에 출력한다.
	public static void Show(string msg, float duration){
		if (_instance == null)
			_instance = FindObjectOfType<csMessageBox> ();
		msgText.text = msg;
		_instance.StartCoroutine (show (duration));

	}

	// 일정 시간동안 메세지를 출력한다.
	// messageBox에 출력될 메세지가 표시된다.
	private static IEnumerator show(float duration){
		messageBox.SetActive (true);

		yield return new WaitForSeconds (duration);

		messageBox.SetActive (false);
	}
}
