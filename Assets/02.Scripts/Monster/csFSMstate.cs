using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태마다 다형성을 위해 추상화클래스로 정의한다.
abstract public class csFSMstate<T> {

	abstract public void EnterState(T _Monster);
	abstract public void UpdateState(T _Monster);
	abstract public void ExitState(T _Monster);
}
