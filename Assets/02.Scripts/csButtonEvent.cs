using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csButtonEvent : MonoBehaviour{
	[SerializeField] private AudioSource audioSource;

	public void BtnStart(){
		audioSource.Play ();
		StartCoroutine ("start");
	}

	private IEnumerator start(){
		yield return new WaitForSeconds (audioSource.clip.length);
		SceneManager.LoadScene (1);
	}
}
