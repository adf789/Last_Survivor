using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건설에 필요한 메소드를 담은 추상화 클래스
public abstract class csConstruction : MonoBehaviour, csObjectInteraction {
	[SerializeField]
	private Material[] materials;

	void Update(){
		ConstructionAct ();
	}

	public void SetMaterials(){
		transform.GetChild(0).GetComponent<Renderer> ().materials = materials;
	}

	public abstract void Establish ();
	public abstract void Interaction (GameObject obj);
	public virtual void ConstructionAct(){

	}
	public virtual void Respawn(Vector3 position){

	}
}
