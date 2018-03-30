using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 텐트 오브젝트 클래스
public class csTent : csConstruction {

	// 텐트에서 쉴 수 있는 행동을 하는 IEnumerator
	IEnumerator TentSleep(){
		// 현재 게임상의 시간 중 현재 시간에서 오전 6시까지 남은 시간을 계산
		float basicTime;
		if (csSunMove.Instance.CurTime >= 18f && csSunMove.Instance.CurTime <= 24f) {
			basicTime = 30f;
		} else {
			basicTime = 6f;
		}
		basicTime = basicTime - csSunMove.Instance.CurTime;
		basicTime = basicTime * (15 * (1 / csSunMove.Instance.TurnSpeed));

		// 현재 게임시간에서 오전 6시까지의 시간을 계산하여 대기한다.
		yield return new WaitForSeconds(basicTime);

		// 대기한 후 게임시간과 플레이어의 상태를 이전 상태로 되돌림
		csCharacterStatus.Instance.isStop = false;
		csCameraController.isStop = false;
		csCharacterStatus.Instance.isSleep = false;
		Time.timeScale = 1f;
		
	}

	// 텐트 오브젝트의 상호작용
	public override void Interaction (GameObject obj)
	{
		// 게임상의 시간으로 6 ~ 18시 사이에는 행동없이 리턴
		if (csSunMove.Instance.CurTime > 6 && csSunMove.Instance.CurTime < 18f) {
			csMessageBox.Show ("밤에만 잘 수 있습니다.");
			return;
		}
		// 플레이어를 멈춤상태로 전환한 후 게임 속도를 10배로 올림
		csCharacterStatus.Instance.isStop = true;
		csCameraController.isStop = true;
		csCharacterStatus.Instance.isSleep = true;
		Time.timeScale = 10f;
		StartCoroutine ("TentSleep");
	}

	// 캠프파이어가 건설될 때 충돌체를 활성화
	public override void Establish ()
	{
		transform.GetComponent <MeshCollider> ().enabled = true;
		SetMaterials ();
	}
}
