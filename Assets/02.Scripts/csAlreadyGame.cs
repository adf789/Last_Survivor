using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csAlreadyGame : MonoBehaviour {
	private static Dictionary<string, Sprite> itemImgs = new Dictionary<string, Sprite> ();
	private static bool isLoaded = false;
	private static Transform dragItemView;

	void Awake () {
		csItemList list = csItemList.Instance;
		csInventory inventory = csInventory.Instance;
		csCharacterStatus cs = csCharacterStatus.Instance;
	}

	// 아이템의 이미지를 로드한다.
	private static void LoadImg(){
		itemImgs.Add("Empty", Resources.Load<Sprite>("Empty"));
		itemImgs.Add("Iron", Resources.Load<Sprite>("Iron"));
		itemImgs.Add("Wood", Resources.Load<Sprite>("Wood"));
		itemImgs.Add("Crystal", Resources.Load<Sprite>("Crystal"));
		itemImgs.Add ("Meat", Resources.Load<Sprite> ("Meat"));
		itemImgs.Add ("Axe", Resources.Load<Sprite> ("Axe"));
		itemImgs.Add ("Pickaxe", Resources.Load<Sprite> ("Pickaxe"));
		itemImgs.Add ("Shovel", Resources.Load<Sprite> ("Shovel"));
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

	// 아이템을 드래그할 때 아이템의 이미지를 띄울 오브젝트를 반환한다.
	public static Transform DragItemView{
		get{
			if(dragItemView == null)
				dragItemView = GameObject.Find ("Canvas").transform.Find ("DragImg");
			return dragItemView;
		}
	}
}
