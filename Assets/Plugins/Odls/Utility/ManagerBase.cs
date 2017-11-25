using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[DisallowMultipleComponent]
public class ManagerBase<T> : MonoBehaviour where T:ManagerBase<T>{
	public void Awake(){
		if (INSTANCE == null) {
			INSTANCE = (T)this;
			GameObject.DontDestroyOnLoad (gameObject);
//			Debug.Log (gameObject.name + " Set As Instance : " + this.GetInstanceID());
		} else if (INSTANCE.gameObject.GetInstanceID () == gameObject.GetInstanceID ()) {
			GameObject.DontDestroyOnLoad (gameObject);
//			Debug.Log (gameObject.name + " Is Instance : " + this.GetInstanceID());
		} else {
			Destroy(gameObject);
//			Debug.Log (gameObject.name + " Not Instance : " + this.GetInstanceID());
		}
	}
	static T INSTANCE;
	public static T instance{
		get{
			if(INSTANCE){
				return INSTANCE;
			}else{
				INSTANCE = GameObject.FindObjectOfType<T>();
				if(INSTANCE){
					return INSTANCE;
				}else{
					INSTANCE = new GameObject(typeof(T).ToString()).AddComponent<T>();
					GameObject _managerTop = GameObject.Find("Manager");
					if(!_managerTop){
						_managerTop = new GameObject("Manager");
					}
					INSTANCE.transform.SetParent(_managerTop.transform);
					return INSTANCE;
				}
			}
		}
	}
	public static bool hasInstance{
		get{
			return (INSTANCE != null);
		}
	}
	[Button]public string SetAsInstanceBut = "SetAsInstance";
	public void SetAsInstance(){
		if(INSTANCE && (INSTANCE.GetInstanceID() != this.GetInstanceID())){
			Debug.Log("Del : " + INSTANCE.GetInstanceID());
			DestroyImmediate(INSTANCE.gameObject);
		}

		INSTANCE = (T)this;
		Debug.Log("INSTANCE : " + INSTANCE.GetInstanceID());
		string _name = typeof(T).ToString();
		_name = Regex.Replace(_name, @"([a-z])([A-Z])", @"$1 $2");

		INSTANCE.gameObject.name = _name;

		GameObject _managerTop = GameObject.Find("Manager");
		if(!_managerTop){
			_managerTop = new GameObject("Manager");
		}
		INSTANCE.transform.SetParent(_managerTop.transform);
		INSTANCE.transform.localPosition = Vector3.zero;
	}
}

