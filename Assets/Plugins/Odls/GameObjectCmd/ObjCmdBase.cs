using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public enum E_ObjCmdType{None,Temp,Awake,Start,Update,Enable,Disable,MouseDown,MouseDrag,MouseEnter,MouseExit,MouseOver,MouseUp,Select,Deselect,Move,PointerDown,PointerEnter,PointerExit,PointerUp,IsTest,NotTest}
public enum E_ObjCmdTimeUnit{Sec,SecReal,Min,MinReal,Frame,FrameFix}
public enum E_ObjCmdTransType{Local,Global}
public class ObjCmdBase : MonoBehaviour,ISelectHandler,IDeselectHandler,IMoveHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler{
	static public bool isTest = false;
	[Button]public string doCmdBut = "DoCmdDefaultNow";
	[Header("觸發時機")]
	public E_ObjCmdType cmdType;
	[Header("自動恢復")]
	public bool autoReset = false;
	[Header("延遲時間")]
	public float delayTime;
	[Header("延遲時間單位")]
	public E_ObjCmdTimeUnit delayUnit;

	public Coroutine coroutine = null;
#region "基礎功能"
	protected virtual bool CmdDefault(){
		Debug.LogError ("ObjCmdBase.cs don't do anything , you should use sub class");
		return false;
	}
	public virtual void Reset(){
		Debug.LogError ("ObjCmdBase.cs don't do anything , you should use sub class");
	}
	protected bool CheckCmdResult(bool p_hasChange){
		//		if (!p_hasChange) {
		//			//沒有變化
		//		}
		CmdEnd ();
		return p_hasChange;
	}
	protected void CmdEnd(){
		if (OnCmdEnd != null) {
			OnCmdEnd.Invoke ();
		}
		if (cmdType == E_ObjCmdType.Temp) {
			Destroy(this);
		}
		coroutine = null;
	}

	public void Awake			()							{if(cmdType==E_ObjCmdType.Awake)					{DoCmd(delayTime);}	}
	public void Start			()							{if(cmdType==E_ObjCmdType.Start)					{DoCmd(delayTime);}	
															 if(cmdType==E_ObjCmdType.IsTest && isTest)			{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.NotTest && (!isTest))		{DoCmd(delayTime);}	}
	public void Update			()							{if(cmdType==E_ObjCmdType.Update)					{DoCmd(delayTime);}	}
	public void OnEnable		()							{if(cmdType==E_ObjCmdType.Enable)					{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.Disable && autoReset)		{Reset();}			}
	public void OnDisable		()							{if(cmdType==E_ObjCmdType.Disable)					{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.Enable && autoReset)		{Reset();}			}

	public void OnMouseDown		()							{if(cmdType==E_ObjCmdType.MouseDown)				{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.MouseUp && autoReset)		{Reset();}			}
	public void OnMouseDrag		()							{if(cmdType==E_ObjCmdType.MouseDrag)				{DoCmd(delayTime);}	}
	public void OnMouseEnter	()							{if(cmdType==E_ObjCmdType.MouseEnter)				{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.MouseExit && autoReset)	{Reset();}			}
	public void OnMouseExit		()							{if(cmdType==E_ObjCmdType.MouseExit)				{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.MouseEnter && autoReset)	{Reset();}			}
	public void OnMouseOver		()							{if(cmdType==E_ObjCmdType.MouseOver)				{DoCmd(delayTime);}	}
	public void OnMouseUp		()							{if(cmdType==E_ObjCmdType.MouseUp)					{DoCmd(delayTime);}
															 if(cmdType==E_ObjCmdType.MouseDown && autoReset)	{Reset();}			}

	public void OnSelect		(BaseEventData p_data)		{if(cmdType==E_ObjCmdType.Select)					{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.Deselect && autoReset)	{Reset();}			}
	public void OnDeselect		(BaseEventData p_data)		{if(cmdType==E_ObjCmdType.Deselect)					{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.Select && autoReset)		{Reset();}			}
	public void OnMove			(AxisEventData p_data)		{if(cmdType==E_ObjCmdType.Move)						{DoCmd(delayTime);}	}
	public void OnPointerDown	(PointerEventData p_data)	{if(cmdType==E_ObjCmdType.PointerDown)				{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.PointerUp && autoReset)	{Reset();}			}
	public void OnPointerEnter	(PointerEventData p_data)	{if(cmdType==E_ObjCmdType.PointerEnter)				{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.PointerExit && autoReset)	{Reset();}			}
	public void OnPointerExit	(PointerEventData p_data)	{if(cmdType==E_ObjCmdType.PointerExit)				{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.PointerEnter && autoReset){Reset();}			}
	public void OnPointerUp		(PointerEventData p_data)	{if(cmdType==E_ObjCmdType.PointerUp)				{DoCmd(delayTime);}		
															 if(cmdType==E_ObjCmdType.PointerDown && autoReset)	{Reset();}			}
#endregion

#region "無回傳值命令 (UI用)"
	/// <summary>
	/// 依預設參數，在[延遲時間]後執行
	/// </summary>
	/// <param name="p_arg">延遲時間</param>
	public void DoCmdDefaultLater(float p_time){DoCmd(p_time);}
	/// <summary>
	/// 依預設參數，立刻執行
	/// </summary>
	public void DoCmdDefaultNow(){DoCmd(0);}
	/// <summary>
	/// 依預設參數，在預設時間後執行
	/// </summary>
	public void DoCmdDefault(){DoCmd();}
#endregion

#region "一般命令 (腳本用)"
	public delegate void OnCmdEndHandler();
	public event OnCmdEndHandler OnCmdEnd;
	/// <summary>
	/// 依預設參數，在[延遲時間]後執行
	/// </summary>
	/// <param name="p_arg">延遲時間，-1 時使用預設值</param>
	public bool DoCmd(float p_time = -1){
		if (p_time < 0) {
			if(delayTime<0){
				Debug.LogError(gameObject.name + " ObjCmd delayTime : " + delayTime + " less than 0");
				return DoCmd(0);
			}
			return DoCmd(delayTime);
		}else if (p_time > 0) {
			coroutine = ObjCmdManager.DoCmdDefaultLater(p_time,this);
			return true;
		}
		return CheckCmdResult( CmdDefault() );
	}

	public void CancelCmd(){
		if (coroutine != null) {
			ObjCmdManager.instance.StopCoroutine (coroutine);
			coroutine = null;
		}
	}
#endregion
}
