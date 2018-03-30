using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csStateMachine <T>{
	private T monster;
	private csFSMstate<T> curState;
	private csFSMstate<T> prevState;

	void Awake(){
		curState = null;
		prevState = null;
	}

	// 현재 상태 변경
	public void ChangeState(csFSMstate<T> newState){
		// 현재 상태가 새로운 상태와 같다면 그대로 종료한다.
		if (curState == newState)
			return;

		// 이전 상태에 현재 상태를 저장한다.
		prevState = curState;

		// 현재 상태가 있다면 상태종료를 호출한다.
		if (curState != null) {
			curState.ExitState (monster);
		}

		// 현재 상태에 새로운 상태를 저장한다.
		curState = newState;

		// 새로운 상태가 있다면 상태시작을 호출한다.
		if (curState != null) {
			curState.EnterState (monster);
		}
	}

	// 첫 상태를 초기화한다.
	public void Init(T monster, csFSMstate<T> initState){
		this.monster = monster;
		ChangeState (initState);
	}

	public void Update(){
		// 현재 상태가 있다면 상태 업데이트를 호출한다.
		if (curState != null) {
			curState.UpdateState (monster);
		}
	}

	// 이전 상태로 돌아간다.
	public void PrevRestore(){
		if (prevState != null) {
			ChangeState (prevState);
		}
	}
}
