using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csCheckBuilding : MonoBehaviour {
	private csBuilding buildingInstance;

	// Use this for initialization
	void Start () {
		buildingInstance = csBuilding.Instance;
	}

	// 해당 트리거내에 지형, 리스폰 오브젝트를 제외한 오브젝트가 있을 경우
	// csBuilding의 객체의 SetBuilding 메소드를 호출한다.
	// SetBuilding 메소드는 인자로 들어온 충돌 유무를 받아 상황에 따라 진행한다.
	void OnTriggerStay(Collider col){
		if (col.tag != "Ground" && col.tag != "Respawn") {
			if (col.tag == "Monster") {
				csMessageBox.Show ("몬스터의 주변엔 지을 수 없습니다.");
			}
			buildingInstance.SendMessage ("SetBuilding", true);
		}
	}

	// 해당 트리거에서 지형, 리스폰 오브젝트를 제외한 오브젝트가 나갔을 경우
	// csBuilding의 객체의 SetBuilding 메소드를 호출한다.
	void OnTriggerExit(Collider col){
		if (col.tag != "Ground" && col.tag != "Respawn") {
			buildingInstance.SendMessage ("SetBuilding", false);
		}
	}
}
