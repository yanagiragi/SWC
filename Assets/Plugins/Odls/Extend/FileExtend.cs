using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileExtend {
	static public string GetDirectoryName(string p_path){
		if (Path.GetExtension (p_path) != "") {
			return Path.GetDirectoryName (p_path);
		} else {
			return p_path;
		}
	}
	static public void PrepareDirectory(string p_path){
		p_path = GetDirectoryName (p_path);
		if (!Directory.Exists (p_path)) {
			Directory.CreateDirectory (p_path);
		}
	}
	static public List<string> GetFileInDirectory(string p_path, bool filtMeta = true){
		List<string> _list = new List<string>();
		p_path = GetDirectoryName (p_path);
		if (Directory.Exists (p_path)) {
			string[] _files = Directory.GetFiles(p_path,"*",SearchOption.AllDirectories);
			int f;
			int len = _files.Length;
			for(f=0; f<len; f++){
				if(filtMeta && Path.GetExtension(_files[f]).ToLower() == ".meta"){
					continue;
				}
				_list.Add(_files[f].Replace("\\","/"));
			}
		}
		return _list;
	}
}