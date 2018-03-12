using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTent : csConstruction {
	private bool isCol;

	void Update(){
		if (!isCol)
			return;
		if (Input.GetKeyDown (KeyCode.F)) {
			csCharacterStatus.Instance.isStop = true;
			csCameraController.isStop = true;
			Time.timeScale = 30f;
			StartCoroutine ("TentSleep");
		}
	}

	IEnumerator TentSleep(){
		yield return new WaitForSeconds (100f);

		csCharacterStatus.Instance.isStop = false;
		csCameraController.isStop = false;
		Time.timeScale = 1f;
	}

	void OnTriggerEnter(Collider col){
		isCol = true;
	}

	void OnTriggerExit(Collider col){
		isCol = false;
	}

	public override void Action ()
	{
		throw new System.NotImplementedException ();
		Debug.Log ("ActionTest");
	}


}
