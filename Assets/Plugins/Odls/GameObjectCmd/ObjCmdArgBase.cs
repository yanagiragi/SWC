using UnityEngine;
using System.Collections;

public class ObjCmdArgBase<T> : ObjCmdBase {
	[Header("相對 ([+]:int/string/Vector [*]:Scale [^]:bool)")]
	public bool relatively;
#region "基礎功能"
	protected virtual bool Cmd(T p_arg){
		Debug.LogError ("ObjCmdBase.cs don't do anything , you should use sub class");
		return false;
	}
#endregion
	
#region "無回傳值命令 (UI用)"
	/// <summary>
	/// 依[命令參數]馬上執行
	/// </summary>
	/// <param name="p_arg">命令參數</param>
	public void DoCmdByArg(T p_arg){DoCmd(p_arg,0);}
	/// <summary>
	/// 依[命令參數]，在預設時間後執行
	/// </summary>
	/// <param name="p_arg">命令參數</param>
	public void DoCmdByArgLater(T p_arg){DoCmd(p_arg,delayTime);}
#endregion
	
#region "一般命令 (腳本用)"
	/// <summary>
	/// 依[命令參數]，在[延遲時間]後執行
	/// </summary>
	/// <param name="p_arg">命令參數</param>
	/// <param name="p_arg">延遲時間，-1 時使用預設值</param>
	public bool DoCmd(T p_arg,float p_time = -1){
		if (p_time < 0) {
			if(delayTime<0){
				Debug.LogWarning(gameObject.name + " ObjCmd delayTime : " + delayTime + " less than 0");
				delayTime = 0;
			}
			return DoCmd(p_arg,delayTime);
		}else if (p_time > 0) {
			coroutine = ObjCmdManager.DoCmdLater(p_time,this,p_arg);
			return false;
		}
		return CheckCmdResult( Cmd(p_arg) );
	}
#endregion
}
