using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class StringExtend{
	static StringBuilder stringBuilder = new StringBuilder("");

#region "Array"
	static public string ArrayToString<T>(T[] p_array,string p_splitString = ","){
		StringBuilder _str = new StringBuilder("[");
		int f;
		int len = p_array.Length;
		for (f=0; f<len; f++) {
			_str.Append(((f==0)?"":p_splitString) + p_array[f].ToString());
		}
		_str.Append ("]");
		return _str.ToString ();
	}
	static public string CollectionToString<T>(ICollection<T> p_collection,string p_splitString = ","){
		StringBuilder _str = new StringBuilder("[");

		bool _isFirst = true;
		foreach (T _obj in p_collection) {
			_str.Append((_isFirst?"":p_splitString) + _obj.ToString());
			_isFirst = false;
		}

		_str.Append ("]");
		return _str.ToString ();
	}
#endregion

#region "Rich Text"
	public static string RichColor(string p_str,Color p_color){
		string _str = string.Format("<color=#{1:X}{2:X}{3:X}{4:X}>{0}</color>",
		                            p_str,
		                            ((int)(p_color.r*255)).ToString("X2"),
		                            ((int)(p_color.g*255)).ToString("X2"),
		                            ((int)(p_color.b*255)).ToString("X2"),
		                            ((int)(p_color.a*255)).ToString("X2"));
		return _str;
	}

	public static string RichSize(string p_str,float p_size){
		string _str = string.Format("<size={1}>{0}</size>",
		                            p_str,
		                            p_size);
		return _str;
	}

	public static string RichBold(string p_str){
		string _str = string.Format("<b>{0}</b>",
		                            p_str);
		return _str;
	}

	public static string RichItalic(string p_str){
		string _str = string.Format("<i>{0}</i>",
		                            p_str);
		return _str;
	}

	public static string SetColor(string p_str,string p_target,Color p_color){
		return p_str.Replace(p_target,RichColor(p_target,p_color));
	}
	
	public static string SetSize(string p_str,string p_target,float p_size){
		return p_str.Replace(p_target,RichSize(p_target,p_size));
	}
	
	public static string SetBold(string p_str,string p_target){
		return p_str.Replace(p_target,RichBold(p_target));
	}
	
	public static string SetItalic(string p_str,string p_target){
		return p_str.Replace(p_target,RichItalic(p_target));
	}
#endregion

#region "Unicode"
	// 改自: http://trufflepenne.blogspot.tw/2013/03/cunicode.html
	public static string StringToUnicode(string srcText){
		string dst = "";
		char[] src = srcText.ToCharArray();
		for (int i = 0; i < src.Length; i++)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
			string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
			dst += str;
		}
		return dst;
	}
	private static string DecodeUTF8 ( string p_code )
	{
		byte[] bytes = new byte[2];
		bytes[1] = byte.Parse(int.Parse(p_code.Substring(0,2),System.Globalization.NumberStyles.HexNumber).ToString()); 
		bytes[0] = byte.Parse(int.Parse(p_code.Substring(2, 2),System.Globalization.NumberStyles.HexNumber).ToString()); 
		
		return Encoding.Unicode.GetString(bytes);
	}
	
	public static string UnicodeToString ( string p_utf8str ){
		int i, _size, _len;
		string _dst, _str;
		
		string[] _strs = p_utf8str.Split('\\');
		_size = _strs.Length;
		_dst = _strs [0];
		
		for ( i=1; i<_size; i++ )
		{
			_str = _strs[i];
			_len = _str.Length;
			
			if ( _len == 0 || _str.Substring(0,1) != "u" ) {
				_dst += '\\' + _str;
				continue;
			}
			
			if ( _len < 5 ) {
				_dst += '\\' + _str;
				continue;
			}
			
			if ( _len > 5 ) {
				_dst += DecodeUTF8(_str.Substring(1,4));
				_dst += _str.Substring(5);
				continue;
			}
			
			_dst += DecodeUTF8(_str.Substring(1,4));
		}
		
		//i don`t know why input string is "xxxx" ... so skip "
		_len = _dst.Length;
		if (_len > 1 && _dst [0] == '"' && _dst [_len - 1] == '"')
			_dst = _dst.Substring (1,_len-2);
		
		return _dst;
	}
#endregion

#region "String"
	static public string SetChar(string p_str, int p_index, char p_char){
		stringBuilder.Length = 0;
		stringBuilder.Append(p_str);
		stringBuilder[p_index] = p_char;
		return stringBuilder.ToString();
	}
#endregion
}
