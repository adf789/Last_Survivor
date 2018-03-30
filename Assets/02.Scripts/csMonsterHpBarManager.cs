using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 몬스터들의 체력바를 오브젝트 폴링하기위한 클래스
public class csMonsterHpBarManager : MonoBehaviour {
	private static csMonsterHpBarManager _instance = null;
	private List<GameObject> hpBarList = null;
	[SerializeField] private GameObject hpBarPrefab;

	public static csMonsterHpBarManager Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<csMonsterHpBarManager> () as csMonsterHpBarManager;
			}
			return _instance;
		}
	}

	// 미리 생성된 체력바 오브젝트들을 List에 추가한다.
	private void Init(){
		hpBarList = new List<GameObject> (transform.childCount);
		for (int i = 0; i < transform.childCount; i++) {
			hpBarList.Add(transform.GetChild (i).gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		if (hpBarList == null) {
			Init ();
		}
	}

	// 현재 사용가능한 체력바 오브젝트들을 반환한다.
	// 만약 사용할 수 있는 체력바가 없을 경우 생성한다.
	public Transform GetHpBar(){
		if (hpBarList == null) {
			Init ();
		}
		// List 중 비활성화된 오브젝트를 찾아 반환
		for (int i = 0; i < hpBarList.Count; i++) {
			if (!hpBarList [i].activeSelf) {
				hpBarList [i].SetActive (true);
				return hpBarList [i].transform;
			}
		}
		// 사용가능한 오브젝트가 없을 경우 생성
		GameObject tempObj = Instantiate (hpBarPrefab, transform) as GameObject;
		tempObj.SetActive (true);
		hpBarList.Add (tempObj);
		return tempObj.transform;
	}

	// 매개변수로 받은 체력바 오브젝트를 비활성화 한다.
	public void RemoveHpBar(Transform hpBar){
		hpBar.GetComponent<Slider> ().value = 1f;
		hpBar.gameObject.SetActive (false);
	}
}
