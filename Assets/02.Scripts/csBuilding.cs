using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건물아이템을 위해 건설할 수 있도록 컨트롤하는 클래스이다.
public class csBuilding : MonoBehaviour{
	private static csBuilding _instance;
	private Transform buildingObjBucket;
	private Transform buildingObj;
	private Transform focusObj;
	private Renderer buildingRenderer;
	private BoxCollider col;
	private bool isBuilding = false, isObjShow = false, isPossible = true;
	private Camera mainCam;

	void Update(){
		// 현재 건물을 짓는 상태인지 확인한 후 건설 상태가 아니라면 아무 것도 하지 않고 반환한다.
		if (!isBuilding) {
			return;
		}
		// 현재 상태가 건설상태인 경우
		// 메인 카메라에서 스크린상의 조준점의 방향으로 발사된 Ray를 호출한다.
		Ray ray = mainCam.ScreenPointToRay (focusObj.position);
		RaycastHit hit;
		// Ray의 Layer에서 지형만 확인하도록 한다.
		int mask = 1 << LayerMask.NameToLayer ("Ground");
		// Ray의 최대 길이가 12인 거리내에 지형이 있다면
		if (Physics.Raycast (ray, out hit, 12f, mask)) {
			// 건설할 물체의 틀을 반환된 위치로 이동한다.
			buildingObjBucket.position = hit.point;

			// Q버튼을 누를 경우 건설틀을 90도 회전한다.
			if (Input.GetKeyDown (KeyCode.Q)) {
				buildingObj.Rotate (Vector3.up, 90f);
			}

			// 해당 위치에 건설이 가능한 경우에 F버튼을 누를 경우
			if (Input.GetKeyDown (KeyCode.F) && isPossible) {
				// 건설 상태를 비활성화한다.
				isBuilding = false;
				// 건설틀의 자식 오브젝트를 Unparent 한다.
				buildingObjBucket.DetachChildren ();
				// 건설틀의 트리거를 비활성화한다.
				buildingObjBucket.GetComponent<BoxCollider> ().enabled = false;
				// 실제 건설할 오브젝트의 csConstruction 클래스의 Establish 메소드를 호출한다.
				buildingObj.GetComponent<csConstruction> ().Establish ();
				// 건설이 성공적으로 됐다면 선택되었던 아이템을 소모한다.
				csSelectedItem.Instance.UseItem ();
			}
			// G버튼을 누를 경우 건설상태를 비활성화한다.
			else if (Input.GetKeyDown (KeyCode.G)) {
				isBuilding = false;
				// 건설틀의 자식오브젝트를 삭제한다.
				Destroy (buildingObjBucket.GetChild (0).gameObject);
			}
		}
	}

	// 현재 건설 가능상태를 설정한다.
	public void SetBuilding(bool isClash){
		// 넘어온 충돌상태가 현재 건설가능상태와 같다면 행동없이 리턴한다.
		if (!isBuilding || isPossible != isClash) {
			return;
		}
		isPossible = !isClash;
		// 건설 가능한 상태가 아닌 경우 재질의 색상을 적색으로 변경한다.
		// 만약 가능한 상태라면 백색으로 변경한다.
		if (!isPossible) {
			for (int i = 0; i < buildingRenderer.materials.Length; i++) {
				buildingRenderer.materials [i].color = Color.red;
			}
		} else {
			for (int i = 0; i < buildingRenderer.materials.Length; i++) {
				buildingRenderer.materials [i].color = Color.white;
			}
		}
	}

	// 해당 클래스의 초기 상태
	private void Init(){
		mainCam = Camera.main;
		focusObj = csAlreadyGame.FocusObj;
		buildingObjBucket = GameObject.Find ("BuildingObject").transform;
		col = buildingObjBucket.GetComponent<BoxCollider> ();
		col.enabled = false;
	}

	public static csBuilding Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<csBuilding>() as csBuilding;
			}
			return _instance;
		}
	}

	public bool IsBuilding{
		get{
			return isBuilding;
		}
		set{
			isBuilding = value;
		}
	}

	// 건설할 오브젝트를 설정한다.
	public Transform BuildingObj{
		set{
			if (mainCam == null)
				Init ();
			buildingObj = value;
			buildingObj.gameObject.SetActive (true);
			// 건설할 오브젝트를 기본 건설틀의 하위 자식으로 넣는다.
			buildingObj.SetParent(buildingObjBucket);
			// 지역위치는 (0, 0, 0)으로 초기화한다.
			buildingObj.localPosition = Vector3.zero;
			// 건설 판단 영역의 크기를 건설할 오브젝트의 Renderer 크기로 초기화한다.
			buildingRenderer = buildingObj.GetChild(0).GetComponent<Renderer> ();
			col.size = buildingRenderer.bounds.size;
		}
	}

	// 해당 아이템의 오브젝트를 BuildList 하위 오브젝트에서 찾아서 생성한다.
	public void SearchBuildingObj(csItem item){
		Transform obj = GameObject.Find ("BuildList").transform.Find (item.Name);

		obj = Instantiate<Transform> (obj);

		_instance.BuildingObj = obj;
	}
}
