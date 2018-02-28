using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public static void Show(string msg, float duration){
		if (_instance == null)
			_instance = FindObjectOfType<csMessageBox> ();
		msgText.text = msg;
		_instance.StartCoroutine (show(duration));

	}

	private static IEnumerator show(float duration){
		messageBox.SetActive (true);

		yield return new WaitForSeconds (duration);

		messageBox.SetActive (false);
	}
}
