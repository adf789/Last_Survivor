using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class csCombineInfo{
	private static csCombineInfo _instance = null;
	private List<csItem> combinationList;

	private csCombineInfo(){
		combinationList = new List<csItem>();
		InitCombination ();
	}

	// 외부 xml 파일에서 조합목록을 불러와 초기화한다.
	private void InitCombination(){
		// combineList.xml을 불러온다.
		XmlNodeList nodeList = csXMLManager.XmlNodeList ("combineList");
		// 조합 아이템 개수만큼 for문
		for (int i = 1; i < nodeList.Count; i++) {
			XmlNode node = nodeList.Item (i);
			csItem item = csItemList.Instance.GetItem(node.Attributes.GetNamedItem ("combItem").Value);
			// 조합 아이템 List에 추가
			combinationList.Add (item);

			// 해당 조합아이템의 조합 재료만큼 for문
			for (int j = 0; j < (node.Attributes.Count - 1) / 2; j++) {
				csItem material = csItemList.Instance.GetItem(node.Attributes.GetNamedItem ("item" + j).Value);
				int count;
				int.TryParse(node.Attributes.GetNamedItem ("count" + j).Value, out count);
				// 조합아이템에는 재료리스트로, 각 재료에는 조합가능 리스트로 추가된다.
				SetMaterials (material, item, count);
			}
		}
	}

	private void SetMaterials(csItem lowItem, csItem highItem, int count){
		// 조합품의 조합재료 리스트에 해당 재료와 개수를 추가한다.
		highItem.AddMaterial (lowItem, count);
		// 조합재료의 조합품 리스트에 해당 물품과 가지고 있어야 할 개수를 추가한다.
		lowItem.AddCombination (highItem, count);
	}

	// 싱글톤
	public static csCombineInfo Instance{
		get{
			if (_instance == null) {
				_instance = new csCombineInfo ();
			}
			return _instance;
		}
	}

	public List<csItem> CombinationList{
		get{
			return combinationList;
		}
	}

	// 해당 아이템이 조합가능한지 판단 후 값을 반환한다.
	public bool isPossibility(csItem combItem){
		for (int i = 0; i < combItem.MaterialList.Count; i++) {
			csItem item = csItemList.Instance.GetItem((int)combItem.MaterialList[i].id);
			if (csInventory.Instance.HasInventory (item))
				return true;
		}
		return false;
	}
}
