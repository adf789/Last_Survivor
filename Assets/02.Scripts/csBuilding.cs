using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (!isBuilding) {
			return;
		}

		if (!isObjShow) {
			buildingObj.gameObject.SetActive (true);
			isObjShow = true;
		}
		Ray ray = mainCam.ScreenPointToRay (focusObj.position);
		RaycastHit hit;
		int mask = 1 << LayerMask.NameToLayer ("Ground");
		if (Physics.Raycast (ray, out hit, 12f, mask)) {
			buildingObjBucket.position = hit.point;
			if (Input.GetKeyDown (KeyCode.Q)) {
				buildingObj.Rotate (Vector3.up, 90f);
			}

			if (Input.GetKeyDown (KeyCode.F) && isPossible) {
				isObjShow = false;
				isBuilding = false;
				buildingObjBucket.DetachChildren ();
				buildingObjBucket.GetComponent<BoxCollider> ().enabled = false;
				buildingObj.GetComponent <MeshCollider> ().enabled = true;
				buildingObj.GetComponent <MeshCollider> ().convex = true;
				buildingObj.GetComponent<csConstruction> ().SetMaterials ();
				csSelectedItem.Instance.UseItem ();
			} else if (Input.GetKeyDown (KeyCode.G)) {
				isObjShow = false;
				isBuilding = false;
				Destroy (buildingObjBucket.GetChild (0).gameObject);
			}
		}
	}

	public void SetBuilding(bool isClash){
		isPossible = !isClash;
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

	private void Init(){
		mainCam = Camera.main;
		focusObj = csAlreadyGame.FocusObj;
		buildingObjBucket = GameObject.Find ("BuildingObject").transform;
		buildingObjBucket.GetComponent<BoxCollider> ().enabled = true;
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

	public Transform BuildingObj{
		set{
			if (mainCam == null)
				Init ();
			buildingObj = value;
			buildingObj.gameObject.SetActive (true);
			buildingObj.SetParent(buildingObjBucket);
			buildingObj.localPosition = Vector3.zero;
			buildingObj.GetComponent <MeshCollider> ().enabled = false;
			col = buildingObjBucket.GetComponent<BoxCollider> ();
			col.enabled = true;
			col.center = Vector3.zero;
			buildingRenderer = buildingObj.GetComponent<Renderer> ();
			col.size = buildingRenderer.bounds.size;
		}
	}

	public void SearchBuildingObj(csItem item){
		Transform obj = GameObject.Find ("BuildList").transform.Find (item.Name);

		obj = Instantiate<Transform> (obj);

		_instance.BuildingObj = obj;
	}
}
