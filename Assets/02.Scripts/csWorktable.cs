using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// 조합창을 제어하는 클래스
public class csWorktable : MonoBehaviour{
	private bool isChangedList, isInit = false;
	private Transform worktableObj;
	private List<csItem> possibleCombList;
	[SerializeField]
	private GameObject combPanel;

	private void Init(){
		worktableObj = transform.GetChild (0).GetChild (0).GetChild (0);
		possibleCombList = new List<csItem> ();
		isChangedList = false;
	}

	// 현재 인벤토리에서 조합 가능한 재료를 탐색한다.
	public void UpdateList(){
		if (!isInit) {
			Init ();
			isInit = true;
		}
		for (int i = 0; i < csInventory.Instance.Count; i++) {
			csItem item = csInventory.Instance.GetToInventory (i);
			// 현재 아이템이 빈 아이템이거나 아이템의 타입이 기타 아이템인 경우 continue
			if (csItemList.Instance.IsEmpty (item) || item.Type != (int)csItemList.Type.ETC)
				continue;
			// 해당 아이템이 조합재료가 인벤토리에 하나라도 존재할 시 조합 가능한 List에 추가한다.
			List<csItems> combList = item.CombinationList;
			for (int j = 0; j < combList.Count; j++) {
				csItem combItem = csItemList.Instance.GetItem ((int)combList [j].id);
				if (!possibleCombList.Contains (combItem)) {
					// 조합 가능한 List에 추가
					possibleCombList.Add (combItem);
					// 조합창의 List가 한번이라도 변화가 있을 경우 조합창 UI를 업데이트 하기 위한 bool 변수
					isChangedList = true;
				}
			}
		}
		// 탐색이 끝난 후 UI를 재구성한다.
		UpdateUI ();
	}

	// 현재 조합 가능성이 있는 리스트에 따라 UI를 Update한다.
	private void UpdateUI(){
		if (!isChangedList)
			return;

		// 오브젝트 폴링을 위해 현재 추가된 자식 오브젝트의 개수와 조합가능한 아이템 개수를 비교한다.
		// 그리고 maxLength에 값을 넣고 for문을 실행한다.
		int curChildCount = worktableObj.childCount;
		int possibleCount = possibleCombList.Count;
		int maxLength;
		if (curChildCount > possibleCount)
			maxLength = curChildCount;
		else
			maxLength = possibleCount;
		
		for (int i = 0; i < maxLength; i++) {
			// 추가된 오브젝트의 개수보다 조합가능한 개수가 적을 때, 현재 오브젝트로 관리
			if (maxLength == curChildCount) {
				GameObject panel = worktableObj.GetChild (i).gameObject;
				// 현재 인덱스가 조합가능한 개수보다 적은경우
				if (i < possibleCount) {
					// Panel 오브젝트 활성화
					panel.SetActive (true);
					// 해당 Panel에 조합아이템을 Update
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				}
				// 그 외의 나머지 오브젝트 비활성화
				else {
					panel.SetActive (false);
				}
			} 
			// 추가된 오브젝트의 개수보다 조합가능한 개수가 많을 때, 오브젝트를 새로 생성
			else {
				// 현재 인덱스가 자식 오브젝트보다 적은경우
				if (i < curChildCount) {
					GameObject panel = worktableObj.GetChild (i).gameObject;
					// Panel 오브젝트 활성화
					panel.SetActive (true);
					// 해당 Panel에 조합아이템을 Update
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				}
				// 그 외에 Panel 오브젝트를 생성
				else {
					Instantiate (combPanel, worktableObj);
					GameObject panel = worktableObj.GetChild (i).gameObject;
					// 생성된 오브젝트 위치 설정 및 활성화
					panel.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -i * 50f);
					panel.SetActive (true);
					// 해당 Panel에 조합아이템을 Update
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				}
			}
		}
	}

	// 조합아이템 및 재료가 표시될 Panel Update
	private void UpdateCombPanel(Transform panel, csItem combItem){
		// 조합 재료를 출력하기 위한 StringBuilder
		StringBuilder sb = new StringBuilder ();
		short checkCount = 0;
//		bool checkPossibility = false;
		List<csItems> materials = combItem.MaterialList;

		// 조합에 필요한 재료들을 StringBuilder에 Append한다.
		for (int i = 0; i < materials.Count; i++) {
			csItems needItem = materials [i]; 
			int needCount = needItem.count;
			csItem item = csItemList.Instance.GetItem((int)needItem.id);
			int count = csInventory.Instance.CountToInventory (item);

			// 현재 가지고 있는 아이템의 개수가 조합에 필요한 개수보다 많다면 카운트한다.
			if (count >= needCount)
				checkCount++;
//			if (count > 0)
//				checkPossibility = true;

			sb.Append (item.Name + " : " + count + "/" + needCount + "\n");
		}

		// 조합창에 출력될 Panel의 조합아이템 이미지와 조합재료 문자열을 설정한다.
		csWorktablePanel workPanel = panel.GetComponent<csWorktablePanel> ();
		panel.GetChild (0).GetComponent<Image> ().sprite = combItem.Picture;
		panel.GetChild (1).GetComponent<Text> ().text = sb.ToString ();

		// 현재 Update될 Panel에 조합아이템과 개수와 해당 클래스의 객체를 인자로 보낸다.
		workPanel.SetWorkPanel (combItem, 1, this);

		// 카운트된 개수가 조합에 필요한 아이템의 종류 수와 같다면 workPanel 객체의 HasItems를 true로 바꾼다.
		// 이 HasItems은 해당 Panel에서 조합 가능한 유무를 판단한다.
		if (checkCount == materials.Count) {
			workPanel.HasItems = true;
		} else {
			workPanel.HasItems = false;
		}

		// 조합 재료가 하나도 없을 때 false를 반환한다.
//		if (checkPossibility) {
//			return true;
//		} else {
//			return false;
//		}

	}

	// 조합 아이템 리스트 중에서 현재 조합이 가능한 것만 제외한 후 나머지는 제거한다.
	public void RemoveAtPossibilityList(){
		if (!isInit) {
			Init ();
			isInit = true;
		}
		for (int i = 0; i < possibleCombList.Count;) {
			csItem combItem = possibleCombList[i++];
			if (!csCombineInfo.Instance.isPossibility (combItem) && possibleCombList.Contains (combItem)) {
				this.possibleCombList.Remove (combItem);
				i--;
			}
		}
	}
}
