using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조합창에 출력될 Panel을 제어하는 클래스
public class csWorktablePanel : MonoBehaviour {
	private csItem combItem;
	private int combCount = 1;
	private csWorktable workTable;
	private bool hasItems = false;

	void Start () {
		if(combItem == null)
			combItem = csItemList.Instance.EmptyItem;
	}

	// 조합 버튼 이벤트
	public void BtnCombine(){
		// 조합 재료를 모두 가지고 있지 않은 경우
		if (!hasItems) {
			csMessageBox.Show ("재료가 부족합니다.");
			return;
		}
		// 조합 재료가 빈 아이템인 경우 행동없이 리턴
		if (csItemList.Instance.IsEmpty (combItem))
			return;

		// 조합아이템의 재료List를 호출
		List<csItems> materials = combItem.MaterialList;
		// 현재 인벤토리에 조합재료가 모두 있는지 확인
		for (int i = 0; i < materials.Count; i++) {
			csItem item = csItemList.Instance.GetItem ((int)materials [i].id);
			// 재료가 부족한 경우 바로 리턴
			if (!csInventory.Instance.SetToInventory (item, -materials [i].count)) {
				csMessageBox.Show ("재료가 부족합니다.");
				return;
			}
		}
		// 재료를 모두 가지고 있는 경우 조합아이템을 추가
		csInventory.Instance.SetToInventory (combItem, combCount);
		// 조합아이템의 재료들을 인벤토리에서 제거
		workTable.RemoveAtPossibilityList ();
		// 조합창 Update
		workTable.UpdateList();
	}

	public bool HasItems{
		set{
			hasItems = value;
		}
	}

	// 현재 Panel의 조합아이템과 조합될 시 추가될 개수와 Panel이 관리될 csWorktable을 설정한다.
	public void SetWorkPanel(csItem item, int count, csWorktable workTable) {
		this.combItem = item;
		this.combCount = count;
		this.workTable = workTable;
	}
}
