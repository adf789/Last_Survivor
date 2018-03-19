using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class csFSMstate<T> {

	abstract public void EnterState(T _Monster);
	abstract public void UpdateState(T _Monster);
	abstract public void ExitState(T _Monster);
}
