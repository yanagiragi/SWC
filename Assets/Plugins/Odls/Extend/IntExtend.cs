using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum E_TIME_UNIT{NONE,YEAR,MONTH,WEEK, DAY, HOUR, MINUTE, SECOND}
public class IntExtend {
#region "String"
	public static string ToStringWithPlus(int p_int,string p_format = ""){
		if (p_int >= 0) {
			return "+" + p_int.ToString (p_format);
		} else {
			return p_int.ToString (p_format);
		}
	}
#endregion 


#region "Mask"
	static List<int> maskList = new List<int>();
	static int maskLen = 0;
	public static int GetFlagMaskByIndexs(params int[] p_indexs) {	
		int f, _index;
		int _mask = 0;
		int len = p_indexs.Length;

		for (f=0; f<len; f++) {
			_index = p_indexs[f];
			if (_index >= maskLen) {
				int f2;
				for (f2=maskLen; f2<=_index; f2++) {
					maskList.Add (1 << f2);
					maskLen++;
				}
			}
			_mask += maskList [_index];
		}
		return _mask;
	}
	public static int[] GetIndexsByFlagMask(int p_flag) {
		List<int> _indexList = new List<int>();
		int _index = 0;
		while(p_flag != 0){
			if((p_flag & 1) == 1){
				_indexList.Add(_index);
			}
			p_flag = p_flag >> 1;
			_index++;
		}
		return _indexList.ToArray ();
	}
	public static int GetIndexByFlagMask(int p_flag) {
		int _index = 0;
		while(p_flag != 0){
			if((p_flag & 1) == 1){
				return _index;
			}
			p_flag = p_flag >> 1;
			_index++;
		}
		return -1;
	}
	public static bool FlagIsOnByIndex(int p_flag,int p_index) {
		int _mask = GetFlagMaskByIndexs (p_index);
		return FlagIsOnByMask(p_flag,_mask);
	}
	public static bool FlagIsOnByMask(int p_flag,int p_mask) {	
		return (p_flag & p_mask)!=0;
	}
	public static int SetFlagAtIndex(int p_flag,int p_index,bool p_on) {
		int _mask = GetFlagMaskByIndexs (p_index);
		if (FlagIsOnByMask (p_flag, _mask) == p_on) {
			return p_flag;
		}
		if (p_on) {
			return p_flag + _mask;
		} else {
			return p_flag - _mask;
		}
	}
#endregion

#region "Time"
	public static int BestUnitTime(TimeSpan p_dateTime,out E_TIME_UNIT p_unit){
		int _day = p_dateTime.Days;
		int _year = _day / 365;
		if (_year > 0) {
			p_unit = E_TIME_UNIT.YEAR;
			return _year;
		} else {
			int _month = _day / 30;
			if (_month > 0) {
				p_unit = E_TIME_UNIT.MONTH;
				return _month;
			} else {
				int _week = _day / 7;
				if (_week > 0) {
					p_unit = E_TIME_UNIT.WEEK;
					return _week;
				} else {
					if (_day > 0) {
						p_unit = E_TIME_UNIT.DAY;
						return _day;
					} else {
						int _hour = p_dateTime.Hours;
						if (_hour > 0) {
							p_unit = E_TIME_UNIT.HOUR;
							return _hour;
						} else {
							int _minute = p_dateTime.Minutes;
							if (_minute > 0) {
								p_unit = E_TIME_UNIT.MINUTE;
								return _minute;
							} else {
								int _second = p_dateTime.Seconds;
								if (_second > 0) {
									p_unit = E_TIME_UNIT.SECOND;
									return _second;
								} else {
									p_unit = E_TIME_UNIT.NONE;
									return 0;
								}
							}
						}
					}
				}
			}
		}
	}
#endregion

#region "Math"
	public static bool isInRange(int p_num, int p_min, int p_max){
		if (p_num < p_min) {
			return false;
		}
		if (p_num > p_max) {
			return false;
		}
		return true;
	}
	public static bool isIndexOf(int p_num,object[] p_array){
		return isInRange(p_num,0,p_array.Length-1);
	}
#endregion
}
