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

	private void InitCombination(){
		XmlNodeList nodeList = csXMLManager.XmlNodeList ("combineList");
		for (int i = 1; i < nodeList.Count; i++) {
			XmlNode node = nodeList.Item (i);
			csItem item = csItemList.Instance.GetItem(node.Attributes.GetNamedItem ("combItem").Value);
			combinationList.Add (item);
			for (int j = 0; j < (node.Attributes.Count - 1) / 2; j++) {
				csItem material = csItemList.Instance.GetItem(node.Attributes.GetNamedItem ("item" + j).Value);
				int count;
				int.TryParse(node.Attributes.GetNamedItem ("count" + j).Value, out count);
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

	public bool isPossibility(csItem combItem){
		for (int i = 0; i < combItem.MaterialList.Count; i++) {
			csItem item = csItemList.Instance.GetItem((int)combItem.MaterialList[i].id);
			if (csInventory.Instance.HasInventory (item))
				return true;
		}
		return false;
	}
}
