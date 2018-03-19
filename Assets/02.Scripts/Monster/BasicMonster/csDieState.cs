using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csDieState : csFSMstate<csMonster> {
	private static csDieState _instance = null;

	private csDieState(){

	}

	public static csDieState Instance{
		get{
			if (_instance == null)
				_instance = new csDieState ();
			return _instance;
		}
	}

	public override void EnterState (csMonster _Monster)
	{
		Debug.Log (_Monster.name + " Death");
	}

	public override void ExitState (csMonster _Monster)
	{
		
	}

	public override void UpdateState (csMonster _Monster)
	{
		
	}
}
