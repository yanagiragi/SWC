using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OdlsAttDemo : MonoBehaviour {
	[Header("[Upper] 轉為大寫")]
	[Upper]	public string UpperString="ABC abc";
	[Header("[Lower] 轉為小寫")]
	[Lower]	public string LowerString="ABC abc";

	[Header("[LockInPlay] 在執行時禁止更動")]
	[LockInPlay] public int LockInPlayVar;
	[LockInPlay] public string LockInPlayVar2;

	[Header("[LockInEdit] 在編輯時禁止更動")]
	[LockInEdit] public int LockInEditVar;
	[LockInEdit] public string LockInEditVar2;

	[Header("[LockInInspector] 在面板中禁止更動")]
	[LockInInspector] public int LockInInspectorVar;
	[LockInInspector] public string LockInInspectorVar2;

	[Header("[NullAlarm(是否log_可選,是否暫停_可選)] 無物件警告")]
	[NullAlarm(true,false)] public GameObject NullAlarmObj;

	[Header("[EqualAlarm(閥值,是否log_可選,是否暫停_可選)] 等於值警告")]
	[EqualAlarm(0,true,false)] public int EqualVar=0;
	[EqualAlarm("ABC",true,false)] public string EqualVar2="ABC";

	[Header("[GreaterAlarm(閥值,是否log_可選,是否暫停_可選)] 大於值警告")]
	[GreaterAlarm(0,true,false)] public float GreaterVar=1;

	[Header("[LessAlarm(閥值,是否log_可選,是否暫停_可選)] 小於值警告")]
	[LessAlarm(0,true,false)] public float LessVar=-1;

	[Header("[URL(文字標籤_可選,url)] 開啟連結")]
	[URL("pixiv","http://www.pixiv.net/")]
	public string null_1;

	[Header("[Project(文字標籤_可選,路徑)] 開啟檔案，路徑相對於專案目錄(Assets的上一層)")]
	[Project("OdlsFace","Assets/Plugins/OdlsEditorCollection/OdlsAttributes/Demo/OdlsFace.jpg")]
	public string null_2;

	[Header("[Button]+函式名 = 無引數按鈕")]
	[Button] public string NullButton="NullButtonClick";

	[Header("[Button(函式名)]+引數 = 有引數按鈕")]
	[Button("IntButtonClick")] public int IntButton=123;
	[Button("StrButtonClick")] public string StringButton="abc";
	[Button("ObjButtonClick")] public GameObject ObjButton;

	[Header("[ObjJump]跳至指定物件")]
	[ObjJump] public GameObject JumpObj;

	void Start () {

	}
	void Update () {

	}
	public void NullButtonClick(){
		Debug.Log ("NullButtonClick");
	}
	public void IntButtonClick(int p_int){
		Debug.Log ("IntButtonClick:"+p_int);
	}
	public void StrButtonClick(string p_str){
		Debug.Log ("StrButtonClick:"+p_str);
	}
	public void ObjButtonClick(GameObject p_obj){
		Debug.Log ("ObjButtonClick:"+p_obj.name);
	}
}
