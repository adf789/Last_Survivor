using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터를 리스폰을 제어하기 위한 클래스
public class csRespawn : MonoBehaviour {
	private int curMonsterCount = 0;
	private float respawnTimer = 0f;
	private Transform monstersT;
	private SphereCollider col;
	private static csRespawn _instance;

	public static csRespawn Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<csRespawn> ();
			}
			return _instance;
		}
	}

	void Start(){
		col = GetComponent<SphereCollider> ();
		// 현재 활성화된 몬스터들을 카운트한다.
		monstersT = transform.Find("Monsters");
		for (int i = 0; i < monstersT.childCount; i++) {
			if (monstersT.GetChild (i).gameObject.activeSelf) {
				curMonsterCount++;
			}
		}
	}

	// 리스폰될 몬스터의 개수가 최대인지 확인한 후 리스폰을 하거나 행동없이 리턴
	void Update(){
		if (curMonsterCount == monstersT.childCount) {
			return;
		}
		// 50초마다 리스폰을 한다.
		if (respawnTimer > 50f) {
			respawnTimer = 0f;
			RespawnMonster ();
		} else {
			respawnTimer += Time.deltaTime * Random.Range(1, 4);
		}
	}

	// 몬스터를 리스폰한다.
	private void RespawnMonster(){
		Transform monstersT = transform.Find("Monsters");
		for (int i = 0; i < monstersT.childCount; i++) {
			GameObject monsterObj = monstersT.GetChild (i).gameObject;
			// 비활성화된 몬스터를 찾는다.
			if (!monsterObj.activeSelf) {
				// 비활성화된 몬스터가 있는 경우 Respawn의 트리거내에서 랜덤한 위치에 리스폰한다.
				monsterObj.SetActive (true);
				// x,z좌표는 무작위, y좌표는 x, z좌표에 준하여 지정한다.
				float x = Random.Range (-col.radius, col.radius);
				float z = Random.Range (-col.radius, col.radius);
				float y = CheckHeight (x, z);
				monsterObj.GetComponent<csObjectInteraction> ().Respawn (new Vector3 (x, y + 1f, z));
				curMonsterCount++;
				return;
			}
		}
	}

	// x, z 좌표에 따라 지형의 최고 위치를 찾는다.
	private float CheckHeight(float x, float z){
		RaycastHit hit;
		if (Physics.Raycast (new Vector3 (col.center.x + x, col.center.y + col.radius, col.center.z + z), Vector3.down, out hit)) {
			return hit.point.y;
		}
		return col.center.y;
	}

	public int CurMonsterCount{
		get{
			return curMonsterCount;
		}
		set{
			curMonsterCount = value;
		}
	}
}
