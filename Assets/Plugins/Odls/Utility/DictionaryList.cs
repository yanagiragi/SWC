using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryList<keyT,valueT>{
	Dictionary<keyT, List<valueT>> dictionary = new Dictionary<keyT, List<valueT>>();
	public void Add(keyT p_key, valueT p_value){
		List<valueT> _list = GetValue(p_key);
		_list.Add(p_value);
	}
	public void AddRange(keyT p_key, IEnumerable<valueT> p_values){
		List<valueT> _list = GetValue(p_key);
		_list.AddRange(p_values);
	}
	public void Clear(){
		foreach(List<valueT> _list in dictionary.Values){
			_list.Clear();
		}
		dictionary.Clear();
	}

	public void Remove(keyT p_key){
		List<valueT> _list = null;
		if(dictionary.TryGetValue(p_key, out _list)){
			_list.Clear();
			dictionary.Remove(p_key);
		}
	}

	public List<valueT> GetValue(keyT p_key){
		List<valueT> _list = null;
		if(!dictionary.TryGetValue(p_key, out _list)){
			_list = new List<valueT>();
			dictionary.Add(p_key, _list);
		}
		return _list;
	}

	public Dictionary<keyT, List<valueT>>.ValueCollection Values{
		get{
			return dictionary.Values;
		}
	}
	public Dictionary<keyT, List<valueT>>.KeyCollection Keys{
		get{
			return dictionary.Keys;
		}
	}
}
