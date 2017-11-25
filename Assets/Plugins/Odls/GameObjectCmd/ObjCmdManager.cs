using UnityEngine;
using System.Collections;

public class ObjCmdManager : ManagerBase<ObjCmdManager> {
	void Start () {}
	void Update () {}
	static public Coroutine DoCmdLater<T> (float p_time,ObjCmdArgBase<T> p_objCmd,T p_arg) {
		return instance.StartCoroutine (instance.IeDoCmdLater<T>(p_time,p_objCmd,p_arg));
	}
	IEnumerator IeDoCmdLater<T> (float p_time,ObjCmdArgBase<T> p_objCmd,T p_arg) {
		yield return StartCoroutine(IeWaitForTime(p_time,p_objCmd.delayUnit));
		p_objCmd.DoCmd (p_arg,0);
	}
	static public Coroutine DoCmdDefaultLater (float p_time,ObjCmdBase p_objCmd) {
		return instance.StartCoroutine (instance.IeDoCmdDefaultLater(p_time,p_objCmd));
	}
	IEnumerator IeDoCmdDefaultLater (float p_time,ObjCmdBase p_objCmd) {
		yield return StartCoroutine(IeWaitForTime(p_time,p_objCmd.delayUnit));
		p_objCmd.DoCmd (0);
	}
	IEnumerator IeWaitForTime (float p_time,E_ObjCmdTimeUnit p_unit) {
		int f;
		switch (p_unit) {
		case E_ObjCmdTimeUnit.Sec:
			yield return new WaitForSeconds(p_time);
			break;
		case E_ObjCmdTimeUnit.SecReal:
			float _time = Time.realtimeSinceStartup;
			yield return new WaitUntil(
				()=> (
					(Time.realtimeSinceStartup - _time) >= p_time)
				);
			break;
		case E_ObjCmdTimeUnit.Min:
			yield return IeWaitForTime(p_time*60f,E_ObjCmdTimeUnit.Sec);
			break;
		case E_ObjCmdTimeUnit.MinReal:
			yield return IeWaitForTime(p_time*60f,E_ObjCmdTimeUnit.SecReal);
			break;
		case E_ObjCmdTimeUnit.Frame:
			for(f=0; f<p_time; f++){
				yield return new WaitForEndOfFrame();
			}
			break;
		case E_ObjCmdTimeUnit.FrameFix:
			for(f=0; f<p_time; f++){
				yield return new WaitForFixedUpdate();
			}
			break;
		}
	}
	static public Cmd NewCmd<Cmd>(GameObject p_obj,
	                                float p_time=0,
	                                E_ObjCmdTimeUnit p_unit=E_ObjCmdTimeUnit.Sec,
	                                E_ObjCmdType p_temp=E_ObjCmdType.Temp
	                                ) where Cmd:ObjCmdBase{
		Cmd _objCmd = p_obj.AddComponent<Cmd>();
		_objCmd.cmdType = p_temp;
		_objCmd.delayUnit = p_unit;
		_objCmd.DoCmd (p_time);
		return _objCmd;
	}
	static public Cmd NewCmd<Cmd,T>(GameObject p_obj,
	                                T p_arg,
	                                bool p_relatively=false,
	                                float p_time=0,
	                                E_ObjCmdTimeUnit p_unit=E_ObjCmdTimeUnit.Sec,
	                                E_ObjCmdType p_temp=E_ObjCmdType.Temp
	                                ) where Cmd:ObjCmdArgBase<T>{
		Cmd _objCmd = p_obj.AddComponent<Cmd>();
		_objCmd.cmdType = p_temp;
		_objCmd.delayUnit = p_unit;
		_objCmd.relatively = p_relatively;
		_objCmd.DoCmd (p_arg,p_time);
		return _objCmd;
	}
}
