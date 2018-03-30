using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캠프파이어 오브젝트 클래스
public class csCampfire : csConstruction {

	public override void Interaction (GameObject obj)
	{
		
	}

	// 캠프파이어가 건설될 때 충돌체를 활성화하고 하위 오브젝트의 이펙트를 활성화한다.
	public override void Establish ()
	{
		transform.GetChild(0).GetComponent <MeshCollider> ().enabled = true;
		SetMaterials ();
		transform.GetChild (0).Find ("FX_Fire").gameObject.SetActive (true);
	}
}
