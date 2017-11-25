#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName="NewSoundConfig",menuName="Sound Config",order=1)]
public class SoundConfigTable : ScriptableObject {
#if UNITY_EDITOR
	[Header("You Should Use Config Setting At SoundTable, Not Here")]
	[LockInInspector]public bool I_Am_Odls;
#endif

	[LockInInspector]
	[SerializeField]float m_adjustVolume = 1;
	[SerializeField]bool m_mute = false;
	[HideInInspector]public List<SoundTable> tableList;

	public float adjustVolume{
		get{
			return m_adjustVolume;
		}
		set{
			m_adjustVolume = value;

			if(Application.isPlaying){
				int f;
				int len = tableList.Count;
				for(f=0; f<len; f++){
					tableList[f].RefreshVolume();
				}
			}
		}
	}

	public bool mute{
		get{
			return m_mute;
		}
		set{
			m_mute = value;
			
			if(Application.isPlaying){
				int f;
				int len = tableList.Count;
				for(f=0; f<len; f++){
					tableList[f].RefreshVolume();
				}
			}
		}
	}

#if UNITY_EDITOR
	public void OnInspectorGUI(SoundTableEditor p_editor) {
		EditorGUI.BeginChangeCheck();

		GUILayout.BeginHorizontal();
		GUILayout.Label ("Mute");
		bool _mute = GUILayout.Toggle (mute,"");
		GUILayout.Label ("Adjust Volume");
		float _adjustVolume = EditorGUILayout.Slider (adjustVolume,0f,1f);
		GUILayout.EndHorizontal ();

		if(EditorGUI.EndChangeCheck()){
			adjustVolume = _adjustVolume;
			mute = _mute;
			EditorUtility.SetDirty(this);
		}
	}
#endif
}
