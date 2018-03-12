using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class csWorktable : MonoBehaviour{
	private bool isChangedList, isInit = false;
	private Transform worktableObj;
	private List<csItem> possibleCombList;
	[SerializeField]
	private GameObject combPanel;

	void Start(){
		
	}

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
			if (csItemList.Instance.IsEmpty (item) || item.Type != (int)csItemList.Type.ETC)
				continue;
			List<csItems> combList = item.CombinationList;
			for (int j = 0; j < combList.Count; j++) {
				csItem combItem = csItemList.Instance.GetItem ((int)combList [j].id);
				if (!possibleCombList.Contains (combItem)) {
					possibleCombList.Add (combItem);
					isChangedList = true;
				}
			}
		}
		UpdateUI ();
	}

	// 현재 조합 가능성이 있는 리스트에 따라 UI를 Update한다.
	private void UpdateUI(){
		if (!isChangedList)
			return;
		
		int curChildCount = worktableObj.childCount;
		int possibleCount = possibleCombList.Count;
		int maxLength;
		if (curChildCount > possibleCount)
			maxLength = curChildCount;
		else
			maxLength = possibleCount;
		
		for (int i = 0; i < maxLength; i++) {
			// 현재 추가할 오브젝트가 없을 때
			if (maxLength == curChildCount) {
				GameObject panel = worktableObj.GetChild (i).gameObject;
				if (i < possibleCount) {
					panel.SetActive (true);
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				} else {
					panel.SetActive (false);
				}
			} 
			// 추가할 오브젝트가 있을 때
			else {
				if (i < curChildCount) {
					GameObject panel = worktableObj.GetChild (i).gameObject;
					panel.SetActive (true);
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				} else {
					Instantiate (combPanel, worktableObj);
					GameObject panel = worktableObj.GetChild (i).gameObject;
					panel.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -i * 50f);
					panel.SetActive (true);
					UpdateCombPanel (panel.transform, possibleCombList [i]);
				}
			}
		}
	}

	private bool UpdateCombPanel(Transform panel, csItem combItem){
		StringBuilder sb = new StringBuilder ();
		short checkCount = 0;
		bool checkPossibility = false;
		List<csItems> materials = combItem.MaterialList;

		for (int i = 0; i < materials.Count; i++) {
			csItems needItem = materials [i]; 
			int needCount = needItem.count;
			csItem item = csItemList.Instance.GetItem((int)needItem.id);
			int count = csInventory.Instance.CountToInventory (item);

			// 현재 가지고 있는 아이템의 개수가 조합에 필요한 개수보다 많다면 카운트한다.
			if (count >= needCount)
				checkCount++;
			if (count > 0)
				checkPossibility = true;

			sb.Append (item.Name + " : " + count + "/" + needCount + "\n");
		}

		csWorktablePanel workPanel = panel.GetComponent<csWorktablePanel> ();
		panel.GetChild (0).GetComponent<Image> ().sprite = combItem.Picture;
		panel.GetChild (1).GetComponent<Text> ().text = sb.ToString ();

		workPanel.SetWorkPanel (combItem, 1, this);

		// 카운트된 개수가 조합에 필요한 아이템의 종류 수와 같다면 workPanel 객체의 HasItems를 true로 바꾼다.
		if (checkCount == materials.Count) {
			workPanel.HasItems = true;
		} else {
			workPanel.HasItems = false;
		}

		// 조합 재료가 하나라도 없을 때 false를 반환한다.
		if (checkPossibility) {
			return true;
		} else {
			return false;
		}

	}

	public void RemoveAtPossibilityList(){
		// 조합 아이템 리스트 중에서 현재 조합이 가능한 것만 제외한 후 나머지는 제거한다.
		for (int i = 0; i < possibleCombList.Count;) {
			csItem combItem = possibleCombList[i++];
			if (!csCombineInfo.Instance.isPossibility (combItem) && possibleCombList.Contains (combItem)) {
				this.possibleCombList.Remove (combItem);
				i--;
			}
		}
	}
}
