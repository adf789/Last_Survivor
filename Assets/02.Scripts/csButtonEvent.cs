using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csButtonEvent : MonoBehaviour{
	[SerializeField] private AudioSource audioSource;

	// 타이틀 화면의 시작 버튼 효과음을 실행한 후 다음 씬으로 전환한다.
	public void BtnStart(){
		audioSource.Play ();
		StartCoroutine ("start");
	}

	private IEnumerator start(){
		yield return new WaitForSeconds (audioSource.clip.length);
		SceneManager.LoadScene (1);
	}
}
