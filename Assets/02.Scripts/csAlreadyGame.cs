using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAlreadyGame : MonoBehaviour {
	private static Dictionary<string, Sprite> itemImgs = new Dictionary<string, Sprite> ();
	private static bool isLoaded = false;

	void Awake () {
		csItemList list = csItemList.Instance;
		csInventory inventory = csInventory.Instance;
		csCharacterStatus cs = csCharacterStatus.Instance;
	}

	private static void LoadImg(){
		itemImgs.Add("Empty", Resources.Load<Sprite>("Empty"));
		itemImgs.Add("Iron", Resources.Load<Sprite>("Iron"));
		itemImgs.Add("Wood", Resources.Load<Sprite>("Wood"));
		itemImgs.Add("Crystal", Resources.Load<Sprite>("Crystal"));
		itemImgs.Add ("Meat", Resources.Load<Sprite> ("Meat"));
	}

	public static Sprite GetImg(string key){
		if (!isLoaded) {
			LoadImg ();
			isLoaded = true;
		}

		if (!itemImgs.ContainsKey (key))
			return itemImgs ["Empty"];
		return itemImgs [key];
	}
}
