using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtendGUI {
	static void Lable(ref Rect p_rect, string p_lable = "") {	
		Rect _rect = new Rect(p_rect);
		if (p_lable != "") {
			int _width = p_lable.Length * 8;
			_rect.width = _width;
			GUI.Label(_rect,p_lable);
			p_rect.x += _width;
			p_rect.width -= _width;
		}
	}

	public static int Flag(Rect p_rect, int p_value, string[] p_strs,string p_lable = "") {		
		Lable (ref p_rect,p_lable);

		int _tempValue = p_value;
		
		int f;
		int len = p_strs.Length;

		if (len <= 0) {
			GUI.Label(p_rect,"No Flag String");
			return p_value;
		}

		int _butWidth = (int)p_rect.width / len;
		p_rect.width = _butWidth;

		int _mask;
		bool _selected;
		for(f=len-1; f>=0; f--){
			_selected = IntExtend.FlagIsOnByIndex(p_value,f);
			if(_selected){
				GUI.color = Color.white;
			}else{
				GUI.color = Color.gray;
			}
			if(GUI.Button (p_rect,p_strs[f])){
				if(_selected){
					_tempValue = IntExtend.SetFlagAtIndex(_tempValue,f,false);
				}else{
					_tempValue = IntExtend.SetFlagAtIndex(_tempValue,f,true);
				}
			}
			p_rect.x += _butWidth;
		}
		GUI.color = Color.white;
		return _tempValue;
	}
}
