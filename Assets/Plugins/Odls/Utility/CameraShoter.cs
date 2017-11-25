using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShoter : MonoBehaviour {
	[LockInInspector]public bool hasShot = false;
	[LockInInspector]public Sprite shotSprite = null;
	[NullAlarm]public RenderTexture renderTexture;
	public float size = -1;
	Camera shotCmr;
	void Start () {
#if UNITY_EDITOR
		Vector2 _viewSize = GameObjectExtend.GetViewSize();
		size *= _viewSize.x / _viewSize.y;
#else
		size *= (float)Screen.width / (float)Screen.height;
#endif
	}
	void Update () {}
	public void ClearShot() {
		renderTexture.DiscardContents ();
		if (shotSprite != null) {
			Destroy(shotSprite.texture);
			shotSprite = null;
		}
		hasShot = false;
	}
	[Button]public string ShotBut = "Shot";
	public void Shot() {
		if (!shotCmr) {
			shotCmr = GetComponent<Camera>();
		}

		bool _lastActive = shotCmr.gameObject.activeSelf;
		float _lastSize = shotCmr.orthographicSize;

		if (size >= 0) {
			shotCmr.orthographicSize = size;
		}
		shotCmr.gameObject.SetActive (true);
		shotCmr.targetTexture = renderTexture;
		shotCmr.Render();

		shotCmr.targetTexture = null;
		shotCmr.orthographicSize = _lastSize;
		shotCmr.gameObject.SetActive (_lastActive);
		hasShot = true;
	}

	public Sprite GetShot() {
		Shot ();
		Rect _rtRect = new Rect (0, 0, renderTexture.width, renderTexture.height);
		RenderTexture.active = renderTexture;
		Texture2D _shotTex = new Texture2D((int)_rtRect.width,(int)_rtRect.height, TextureFormat.RGB24,false);  
		_shotTex.ReadPixels(_rtRect, 0, 0);
		_shotTex.Apply();
		RenderTexture.active = null;
		ClearShot ();
		shotSprite = Sprite.Create(_shotTex,_rtRect,new Vector2(0.5f,0.5f));		

		return shotSprite;
	}
}
