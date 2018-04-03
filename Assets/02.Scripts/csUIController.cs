using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 각종 UI를 관리하기 위한 클래스이다.
public class csUIController : MonoBehaviour {
	private static csUIController _instance;
	private RectTransform scrollGuide;
	private Transform timeImageT;
	private Slider hpBarS, fatigueBarS;
	private bool isShowInventory, isShowWorktable;
	private float[] scrollIndex = {1.5f, 12.5f, 23.5f, 34.5f, 45.5f, 56.5f, 67.5f, 78.5f, 89.5f};
	private int curScrollIndex = 0;

	public static csUIController Instance{
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<csUIController> ();
			}
			return _instance;
		}
	}

	// 자주 사용될 오브젝트들을 미리 초기화한다.
	void Start () {
		Cursor.SetCursor (Resources.Load<Texture2D> ("Cursor"), new Vector2 (0, 0), CursorMode.Auto);
		Cursor.visible = false;
		hpBarS = csAlreadyGame.HpObj.GetComponent<Slider> ();
		fatigueBarS = csAlreadyGame.FatigueBarObj.GetComponent<Slider> ();
		scrollGuide = csAlreadyGame.QuickBarObj.transform.Find ("Guide").GetComponent<RectTransform>();
		timeImageT = GameObject.Find ("Canvas").transform.Find ("TimeView").GetChild (0).GetChild (0);
		Init ();
		isShowInventory = false;
	}

	// 실시간으로 Update될 UI와 버튼을 눌렀을 시 출력되는 UI들을 정의한다.
	void Update () {
		// 현재 플레이어의 상태UI
		UpdateStatusUI ();
		// 현재 시간UI
		UpdateTimeUI ();
		// 현재 퀵바의 가이드 오브젝트가 가리키고 있는 도구 착용
		UseTools ();

		// "I" 버튼으로 인벤토리UI 출력
		if (Input.GetButtonDown ("I")) {
			OpenInventory ();
			csAlreadyGame.SelectItemView.gameObject.SetActive (false);

		}
		// "J" 버튼으로 조합창UI 출력
		if (Input.GetButtonDown ("J")) {
			OpenWorktable ();
			csAlreadyGame.WorktableObj.GetComponent<csWorktable> ().UpdateList ();
		}
		// 인벤토리, 조합창이 비활성화 되었을 시 마우스 휠 또는 숫자 버튼으로 퀵바의 가이드를 조정
		if (!isShowInventory && !isShowWorktable) {
			float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
			// 마우스 휠
			SetScrollIndex (mouseScroll);
			// 숫자 버튼
			NumKeyDown ();
		}
	}

	// 인벤토리 및 퀵바의 슬롯들을 초기화한다.
	private void Init(){
		Transform inventoryTransform = csAlreadyGame.InventoryObj.transform.GetChild (0).GetChild (0);
		for (int i = 0; i < inventoryTransform.childCount; i++) {
			inventoryTransform.GetChild (i).GetComponent<csInventorySlot> ().Init ();
		}
		for (int i = 0; i < csAlreadyGame.CrateObj.transform.childCount; i++) {
			csAlreadyGame.CrateObj.transform.GetChild (i).GetComponent<csCrateSlot> ().Init ();
		}
		csAlreadyGame.InventoryObj.SetActive (false);
		csAlreadyGame.CrateObj.SetActive (false);
		csAlreadyGame.WorktableObj.SetActive (false);

		Transform quickBarObj = csAlreadyGame.QuickBarObj.transform;
		for(int i = 0; i < quickBarObj.childCount; i++){
			if (quickBarObj.GetChild (i).name.Equals ("Guide"))
				continue;
			quickBarObj.GetChild (i).GetComponent<csInventorySlot> ().Init ();
		}
	}

	// 해가 움직이는 속도에 따라 현재 시간대를 보여주는 UI를 업데이트한다.
	private void UpdateTimeUI(){
		timeImageT.Rotate (Vector3.forward, csSunMove.Instance.TurnSpeed * Time.deltaTime);
	}

	// 현재 체력 수치와 배고픔 수치를 업데이트한다.
	private void UpdateStatusUI(){
		hpBarS.value = csCharacterStatus.Instance.curHp / csCharacterStatus.Instance.maxHp;
		fatigueBarS.value = csCharacterStatus.Instance.curFatigue / csCharacterStatus.Instance.maxFatigue;
	}

	// 숫자키를 누름에 따라 퀵바의 가이드 위치가 변경됨.
	private void NumKeyDown(){
		int temp = curScrollIndex;
		if (Input.GetKeyDown (KeyCode.Alpha1))
			temp = 0;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			temp = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha3))
			temp = 2;
		else if (Input.GetKeyDown (KeyCode.Alpha4))
			temp = 3;
		else if (Input.GetKeyDown (KeyCode.Alpha5))
			temp = 4;
		else if (Input.GetKeyDown (KeyCode.Alpha6))
			temp = 5;
		else if (Input.GetKeyDown (KeyCode.Alpha7))
			temp = 6;
		else if (Input.GetKeyDown (KeyCode.Alpha8))
			temp = 7;
		else if (Input.GetKeyDown (KeyCode.Alpha9))
			temp = 8;

		if (temp != curScrollIndex) {
			curScrollIndex = temp;
			MoveScrollGuide ();
		}
	}

	// 조합창 GUI를 열고 닫는다.
	private void OpenWorktable(){
		isShowWorktable = !isShowWorktable;
		Cursor.visible = isShowWorktable;
		csAlreadyGame.WorktableObj.SetActive (isShowWorktable);
		csCharacterStatus.Instance.isStop = isShowWorktable;
		csCameraController.isStop = isShowWorktable;
	}

	// 인벤토리 GUI를 열고 닫는다.
	private void OpenInventory(){
		isShowInventory = !isShowInventory;
		Cursor.visible = isShowInventory;
		csAlreadyGame.InventoryObj.SetActive (isShowInventory);
		csCharacterStatus.Instance.isStop = isShowInventory;
		csCameraController.isStop = isShowInventory;
	}

	// 퀵바의 가이드 오브젝트 위치를 조절한다.
	private void MoveScrollGuide(){
		Vector2 tempVector2 = scrollGuide.anchoredPosition;
		tempVector2.x = scrollIndex [curScrollIndex];
		scrollGuide.anchoredPosition = tempVector2;
	}

	// 퀵바의 Guide 오브젝트 위치의 아이템의 종류에 따라 손의 도구를 바꾼다.
	public void UseTools(){
		csItem item = csInventory.Instance.GetToInventory (curScrollIndex);
		csCharacterStatus.Instance.SetEquipment (item.Name);
		
	}

	// 마우스의 휠 움직임에 따라 Guide 오브젝트 움직임 방향을 정한다.
	private void SetScrollIndex(float mouse){
		if (mouse == 0f)
			return;

		if (mouse > 0f) {
			// 마우스의 휠을 올릴 때
			curScrollIndex--;
			if (curScrollIndex < 0)
				curScrollIndex = 8;
		} else if (mouse < 0f) {
			// 마우스의 휠을 내릴 때
			curScrollIndex++;
			if (curScrollIndex > 8)
				curScrollIndex = 0;
		}
		MoveScrollGuide ();
	}

	// 현재 스크롤바에 위치한 아이템을 반환한다.
	public csItem CurScrollItem(){
		return csInventory.Instance.GetToInventory (curScrollIndex);
	}
}
