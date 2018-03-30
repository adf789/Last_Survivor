using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 오브젝트마다 상호작용될 메소드를 정의하기 위한 추상화 클래스
public interface csObjectInteraction{

	void Interaction (GameObject gameObject);
	void Respawn(Vector3 position);
}
