//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditor.iOS.Xcode;
//using UnityEditor.Callbacks;
//using System.IO;
//#endif
//using UnityEngine;
//using System.Collections;
//
//public class IosPermissions{
//	#if UNITY_EDITOR && UNITY_IOS
//	[PostProcessBuild(0)]
//	public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject){
//		string _plistPath = Path.Combine (pathToBuiltProject, "./Info.plist");
//		PlistDocument _plist = new PlistDocument();
//		_plist.ReadFromString (File.ReadAllText(_plistPath));
//		PlistElementDict _rootDict = _plist.root;
//		_rootDict.SetString("NSAppleMusicUsageDescription","Allow this App to access your Media Library?");
//		_rootDict.SetString("NSBluetoothPeripheralUsageDescription","Whether to allow this app to use Bluetooth?");
//		_rootDict.SetString("NSCalendarsUsageDescription","Allow this app to use calendars?");
//		_rootDict.SetString("NSCameraUsageDescription","Allow App to use this camera?");
//		_rootDict.SetString("NSContactsUsageDescription","Allow this app to access your contacts?");
//		_rootDict.SetString("NSLocationAlwaysUsageDescription","Allow this app to access location?");
//		_rootDict.SetString("NSLocationUsageDescription","Allow this app to access location?");
//		_rootDict.SetString("NSLocationWhenInUseUsageDescription","Allow this app to access location?");
//		_rootDict.SetString("NSMicrophoneUsageDescription","Allow this app to use the microphone?");
//		_rootDict.SetString("NSPhotoLibraryUsageDescription","Allow this App to access your Photo Library?");
//		File.WriteAllText(_plistPath,_plist.WriteToString());
//	}
//	#endif
//}
