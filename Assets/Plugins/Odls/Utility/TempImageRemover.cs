#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(Image))]
public class TempImageRemover : MonoBehaviour {
	public string imagePath = "";
	void Start () {
		Destroy (gameObject);
	}
	void Update () {}
	public void RemoveImage () {
		gameObject.SetActive(false);
#if UNITY_EDITOR
		Image _image = GetComponent<Image>();
		if(_image.sprite){
			imagePath = AssetDatabase.GetAssetPath(_image.sprite);
			_image.sprite = null;
		}
#endif
	}
	[Button]public string reloadImageBut = "ReloadImage";
	public void ReloadImage () {
		gameObject.SetActive(true);
#if UNITY_EDITOR
		if(imagePath == ""){
			return;
		}

		Image _image = GetComponent<Image>();
		Sprite _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath);
		if(_sprite == null){
			Debug.LogError("TempImage[" + gameObject.name + "] ReloadImage Failed : " + imagePath);
			return;
		}

		imagePath = "";
		_image.sprite = _sprite;
#endif
	}

	[Button]public string reloadAllImageBut = "ReloadAllImage";
	public void ReloadAllImage () {
		TempImageRemover[] _tempImages = GameObjectExtend.GetComponentsInScene<TempImageRemover>();
		int f;
		int len = _tempImages.Length;
		for(f=0; f<len; f++){
			_tempImages[f].ReloadImage();
		}
		Debug.Log ("Reload " + len + " TempImage");
	}

	[Button]public string removeAllImageBut = "RemoveAllImage";
	public void RemoveAllImage () {
		TempImageRemover[] _tempImages = GameObjectExtend.GetComponentsInScene<TempImageRemover>();
		int f;
		int len = _tempImages.Length;
		for(f=0; f<len; f++){
			_tempImages[f].RemoveImage();
		}
		Debug.Log ("Remove " + len + " TempImage");
	}
}
