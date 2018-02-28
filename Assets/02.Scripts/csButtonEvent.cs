using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csButtonEvent : MonoBehaviour{

	public void BtnStart(){
		SceneManager.LoadScene (1);
	}
}
