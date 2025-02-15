﻿//========= Copyright 2015, Valve Corporation, All rights reserved. ===========
//
// Purpose: Prompt developers to use settings most compatible with SteamVR.
//
//=============================================================================

using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class SteamVR_Settings : EditorWindow
{
	const bool forceShow = false; // Set to true to get the dialog to show back up in the case you clicked Ignore All.

	const string ignore = "ignore.";
	const string useRecommended = "Use recommended ({0})";
	const string currentValue = " (current = {0})";

	const string showUnitySplashScreen = "Show Unity Splashscreen";
	const string defaultIsFullScreen = "Default is Fullscreen";
	const string defaultScreenSize = "Default Screen Size";
	const string runInBackground = "Run In Background";
	const string displayResolutionDialog = "Display Resolution Dialog";
	const string resizableWindow = "Resizable Window";
	const string fullscreenMode = "D3D11 Fullscreen Mode";
	const string visibleInBackground = "Visible In Background";
	const string renderingPath = "Rendering Path";
//	const string colorSpace = "Color Space";
	const string stereoscopicRendering = "Stereoscopic Rendering";
#if !UNITY_5_0
	const string virtualRealitySupported = "Virtual Reality Support";
#endif
	const bool recommended_ShowUnitySplashScreen = false;
	const bool recommended_DefaultIsFullScreen = false;
	const int recommended_DefaultScreenWidth = 1024;
	const int recommended_DefaultScreenHeight = 768;
	const bool recommended_RunInBackground = true;
	const ResolutionDialogSetting recommended_DisplayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;
	const bool recommended_ResizableWindow = true;
	const D3D11FullscreenMode recommended_FullscreenMode = D3D11FullscreenMode.FullscreenWindow;
	const bool recommended_VisibleInBackground = true;
	const RenderingPath recommended_RenderPath = RenderingPath.Forward;
//	const ColorSpace recommended_ColorSpace = ColorSpace.Linear;
	const bool recommended_StereoscopicRendering = false;
#if !UNITY_5_0
	const bool recommended_VirtualRealitySupported = false;
#endif
	static SteamVR_Settings window;


	static SteamVR_Settings()
	{
		EditorApplication.update += Update;
	}

	static void Update()
	{
		bool show =
			(!EditorPrefs.HasKey(ignore + showUnitySplashScreen) &&
				PlayerSettings.showUnitySplashScreen != recommended_ShowUnitySplashScreen) ||
			(!EditorPrefs.HasKey(ignore + defaultIsFullScreen) &&
				PlayerSettings.defaultIsFullScreen != recommended_DefaultIsFullScreen) ||
			(!EditorPrefs.HasKey(ignore + defaultScreenSize) &&
				(PlayerSettings.defaultScreenWidth != recommended_DefaultScreenWidth ||
				PlayerSettings.defaultScreenHeight != recommended_DefaultScreenHeight)) ||
			(!EditorPrefs.HasKey(ignore + runInBackground) &&
				PlayerSettings.runInBackground != recommended_RunInBackground) ||
			(!EditorPrefs.HasKey(ignore + displayResolutionDialog) &&
				PlayerSettings.displayResolutionDialog != recommended_DisplayResolutionDialog) ||
			(!EditorPrefs.HasKey(ignore + resizableWindow) &&
				PlayerSettings.resizableWindow != recommended_ResizableWindow) ||
			(!EditorPrefs.HasKey(ignore + fullscreenMode) &&
				PlayerSettings.d3d11FullscreenMode != recommended_FullscreenMode) ||
			(!EditorPrefs.HasKey(ignore + visibleInBackground) &&
				PlayerSettings.visibleInBackground != recommended_VisibleInBackground) ||
			(!EditorPrefs.HasKey(ignore + renderingPath) &&
				PlayerSettings.renderingPath != recommended_RenderPath) ||
//			(!EditorPrefs.HasKey(ignore + colorSpace) &&
//				PlayerSettings.colorSpace != recommended_ColorSpace) ||
			(!EditorPrefs.HasKey(ignore + stereoscopicRendering) &&
				PlayerSettings.stereoscopic3D != recommended_StereoscopicRendering) ||
#if !UNITY_5_0
			(!EditorPrefs.HasKey(ignore + virtualRealitySupported) &&
				PlayerSettings.virtualRealitySupported != recommended_VirtualRealitySupported) ||
#endif
			forceShow;

		if (show)
		{
			window = GetWindow<SteamVR_Settings>(true);
			window.minSize = new Vector2(320, 440);
			//window.title = "SteamVR";
		}

		EditorApplication.update -= Update;
	}

	Vector2 scrollPosition;
	bool toggleState;

	string GetResourcePath()
	{
		var ms = MonoScript.FromScriptableObject(this);
		var path = AssetDatabase.GetAssetPath(ms);
		path = Path.GetDirectoryName(path);
		return path.Substring(0, path.Length - "Editor".Length) + "Textures/";
	}

	public void OnGUI()
	{
		var resourcePath = GetResourcePath();
#if UNITY_5_0	// old-n-busted
		var logo = Resources.LoadAssetAtPath<Texture2D>(resourcePath + "logo.png");
#else			// new hotness
		var logo = AssetDatabase.LoadAssetAtPath<Texture2D>(resourcePath + "logo.png");
#endif
		var rect = GUILayoutUtility.GetRect(position.width, 150, GUI.skin.box);
		if (logo)
			GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);

		EditorGUILayout.HelpBox("Recommended project settings for SteamVR:", MessageType.Warning);

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		int numItems = 0;

		if (!EditorPrefs.HasKey(ignore + showUnitySplashScreen) &&
			PlayerSettings.showUnitySplashScreen != recommended_ShowUnitySplashScreen)
		{
			++numItems;

			GUILayout.Label(showUnitySplashScreen + string.Format(currentValue, PlayerSettings.showUnitySplashScreen));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_ShowUnitySplashScreen)))
			{
				PlayerSettings.showUnitySplashScreen = recommended_ShowUnitySplashScreen;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + showUnitySplashScreen, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + defaultIsFullScreen) &&
			PlayerSettings.defaultIsFullScreen != recommended_DefaultIsFullScreen)
		{
			++numItems;

			GUILayout.Label(defaultIsFullScreen + string.Format(currentValue, PlayerSettings.defaultIsFullScreen));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_DefaultIsFullScreen)))
			{
				PlayerSettings.defaultIsFullScreen = recommended_DefaultIsFullScreen;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + defaultIsFullScreen, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + defaultScreenSize) &&
			(PlayerSettings.defaultScreenWidth != recommended_DefaultScreenWidth ||
			PlayerSettings.defaultScreenHeight != recommended_DefaultScreenHeight))
		{
			++numItems;

			GUILayout.Label(defaultScreenSize + string.Format(" ({0}x{1})", PlayerSettings.defaultScreenWidth, PlayerSettings.defaultScreenHeight));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format("Use recommended ({0}x{1})", recommended_DefaultScreenWidth, recommended_DefaultScreenHeight)))
			{
				PlayerSettings.defaultScreenWidth = recommended_DefaultScreenWidth;
				PlayerSettings.defaultScreenHeight = recommended_DefaultScreenHeight;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + defaultScreenSize, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + runInBackground) &&
			PlayerSettings.runInBackground != recommended_RunInBackground)
		{
			++numItems;

			GUILayout.Label(runInBackground + string.Format(currentValue, PlayerSettings.runInBackground));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_RunInBackground)))
			{
				PlayerSettings.runInBackground = recommended_RunInBackground;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + runInBackground, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + displayResolutionDialog) &&
			PlayerSettings.displayResolutionDialog != recommended_DisplayResolutionDialog)
		{
			++numItems;

			GUILayout.Label(displayResolutionDialog + string.Format(currentValue, PlayerSettings.displayResolutionDialog));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_DisplayResolutionDialog)))
			{
				PlayerSettings.displayResolutionDialog = recommended_DisplayResolutionDialog;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + displayResolutionDialog, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + resizableWindow) &&
			PlayerSettings.resizableWindow != recommended_ResizableWindow)
		{
			++numItems;

			GUILayout.Label(resizableWindow + string.Format(currentValue, PlayerSettings.resizableWindow));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_ResizableWindow)))
			{
				PlayerSettings.resizableWindow = recommended_ResizableWindow;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + resizableWindow, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + fullscreenMode) &&
			PlayerSettings.d3d11FullscreenMode != recommended_FullscreenMode)
		{
			++numItems;

			GUILayout.Label(fullscreenMode + string.Format(currentValue, PlayerSettings.d3d11FullscreenMode));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_FullscreenMode)))
			{
				PlayerSettings.d3d11FullscreenMode = recommended_FullscreenMode;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + fullscreenMode, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + visibleInBackground) &&
			PlayerSettings.visibleInBackground != recommended_VisibleInBackground)
		{
			++numItems;

			GUILayout.Label(visibleInBackground + string.Format(currentValue, PlayerSettings.visibleInBackground));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_VisibleInBackground)))
			{
				PlayerSettings.visibleInBackground = recommended_VisibleInBackground;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + visibleInBackground, true);
			}

			GUILayout.EndHorizontal();
		}

		if (!EditorPrefs.HasKey(ignore + renderingPath) &&
			PlayerSettings.renderingPath != recommended_RenderPath)
		{
			++numItems;

			GUILayout.Label(renderingPath + string.Format(currentValue, PlayerSettings.renderingPath));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_RenderPath) + " - required for MSAA"))
			{
				PlayerSettings.renderingPath = recommended_RenderPath;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + renderingPath, true);
			}

			GUILayout.EndHorizontal();
		}
/*
		if (!EditorPrefs.HasKey(ignore + colorSpace) &&
			PlayerSettings.colorSpace != recommended_ColorSpace)
		{
			++numItems;

			GUILayout.Label(colorSpace + string.Format(currentValue, PlayerSettings.colorSpace));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_ColorSpace)))
			{
				PlayerSettings.colorSpace = recommended_ColorSpace;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + colorSpace, true);
			}

			GUILayout.EndHorizontal();
		}
*/
		if (!EditorPrefs.HasKey(ignore + stereoscopicRendering) &&
			PlayerSettings.stereoscopic3D != recommended_StereoscopicRendering)
		{
			++numItems;

			GUILayout.Label(stereoscopicRendering + string.Format(currentValue, PlayerSettings.stereoscopic3D));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_StereoscopicRendering)))
			{
				PlayerSettings.stereoscopic3D = recommended_StereoscopicRendering;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + stereoscopicRendering, true);
			}

			GUILayout.EndHorizontal();
		}
#if !UNITY_5_0
		if (!EditorPrefs.HasKey(ignore + virtualRealitySupported) &&
			PlayerSettings.virtualRealitySupported != recommended_VirtualRealitySupported)
		{
			++numItems;

			GUILayout.Label(virtualRealitySupported + string.Format(currentValue, PlayerSettings.virtualRealitySupported));

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(string.Format(useRecommended, recommended_VirtualRealitySupported)))
			{
				PlayerSettings.virtualRealitySupported = recommended_VirtualRealitySupported;
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Ignore"))
			{
				EditorPrefs.SetBool(ignore + virtualRealitySupported, true);
			}

			GUILayout.EndHorizontal();
		}
#endif
		GUILayout.BeginHorizontal();

		GUILayout.FlexibleSpace();

		if (GUILayout.Button("Clear All Ignores"))
		{
			EditorPrefs.DeleteKey(ignore + showUnitySplashScreen);
			EditorPrefs.DeleteKey(ignore + defaultIsFullScreen);
			EditorPrefs.DeleteKey(ignore + defaultScreenSize);
			EditorPrefs.DeleteKey(ignore + runInBackground);
			EditorPrefs.DeleteKey(ignore + displayResolutionDialog);
			EditorPrefs.DeleteKey(ignore + resizableWindow);
			EditorPrefs.DeleteKey(ignore + fullscreenMode);
			EditorPrefs.DeleteKey(ignore + visibleInBackground);
			EditorPrefs.DeleteKey(ignore + renderingPath);
//			EditorPrefs.DeleteKey(ignore + colorSpace);
			EditorPrefs.DeleteKey(ignore + stereoscopicRendering);
#if !UNITY_5_0
			EditorPrefs.DeleteKey(ignore + virtualRealitySupported);
#endif
		}

		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();

		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();

		if (numItems > 0)
		{
			if (GUILayout.Button("Accept All"))
			{
				// Only set those that have not been explicitly ignored.
				if (!EditorPrefs.HasKey(ignore + showUnitySplashScreen))
					PlayerSettings.showUnitySplashScreen = recommended_ShowUnitySplashScreen;
				if (!EditorPrefs.HasKey(ignore + defaultIsFullScreen))
					PlayerSettings.defaultIsFullScreen = recommended_DefaultIsFullScreen;
				if (!EditorPrefs.HasKey(ignore + defaultScreenSize))
				{
					PlayerSettings.defaultScreenWidth = recommended_DefaultScreenWidth;
					PlayerSettings.defaultScreenHeight = recommended_DefaultScreenHeight;
				}
				if (!EditorPrefs.HasKey(ignore + runInBackground))
					PlayerSettings.runInBackground = recommended_RunInBackground;
				if (!EditorPrefs.HasKey(ignore + displayResolutionDialog))
					PlayerSettings.displayResolutionDialog = recommended_DisplayResolutionDialog;
				if (!EditorPrefs.HasKey(ignore + resizableWindow))
					PlayerSettings.resizableWindow = recommended_ResizableWindow;
				if (!EditorPrefs.HasKey(ignore + fullscreenMode))
					PlayerSettings.d3d11FullscreenMode = recommended_FullscreenMode;
				if (!EditorPrefs.HasKey(ignore + visibleInBackground))
					PlayerSettings.visibleInBackground = recommended_VisibleInBackground;
				if (!EditorPrefs.HasKey(ignore + renderingPath))
					PlayerSettings.renderingPath = recommended_RenderPath;
//				if (!EditorPrefs.HasKey(ignore + colorSpace))
//					PlayerSettings.colorSpace = recommended_ColorSpace;
				if (!EditorPrefs.HasKey(ignore + stereoscopicRendering))
					PlayerSettings.stereoscopic3D = recommended_StereoscopicRendering;
#if !UNITY_5_0
				if (!EditorPrefs.HasKey(ignore + virtualRealitySupported))
					PlayerSettings.virtualRealitySupported = recommended_VirtualRealitySupported;
#endif
				EditorUtility.DisplayDialog("Accept All", "You made the right choice!", "Ok");

				Close();
			}

			if (GUILayout.Button("Ignore All"))
			{
				if (EditorUtility.DisplayDialog("Ignore All", "Are you sure?", "Yes, Ignore All", "Cancel"))
				{
					// Only ignore those that do not currently match our recommended settings.
					if (PlayerSettings.showUnitySplashScreen != recommended_ShowUnitySplashScreen)
						EditorPrefs.SetBool(ignore + showUnitySplashScreen, true);
					if (PlayerSettings.defaultIsFullScreen != recommended_DefaultIsFullScreen)
						EditorPrefs.SetBool(ignore + defaultIsFullScreen, true);
					if (PlayerSettings.defaultScreenWidth != recommended_DefaultScreenWidth ||
						PlayerSettings.defaultScreenHeight != recommended_DefaultScreenHeight)
					{
						EditorPrefs.SetBool(ignore + defaultScreenSize, true);
					}
					if (PlayerSettings.runInBackground != recommended_RunInBackground)
						EditorPrefs.SetBool(ignore + runInBackground, true);
					if (PlayerSettings.displayResolutionDialog != recommended_DisplayResolutionDialog)
						EditorPrefs.SetBool(ignore + displayResolutionDialog, true);
					if (PlayerSettings.resizableWindow != recommended_ResizableWindow)
						EditorPrefs.SetBool(ignore + resizableWindow, true);
					if (PlayerSettings.d3d11FullscreenMode != recommended_FullscreenMode)
						EditorPrefs.SetBool(ignore + fullscreenMode, true);
					if (PlayerSettings.visibleInBackground != recommended_VisibleInBackground)
						EditorPrefs.SetBool(ignore + visibleInBackground, true);
					if (PlayerSettings.renderingPath != recommended_RenderPath)
						EditorPrefs.SetBool(ignore + renderingPath, true);
//					if (PlayerSettings.colorSpace != recommended_ColorSpace)
//						EditorPrefs.SetBool(ignore + colorSpace, true);
					if (PlayerSettings.stereoscopic3D != recommended_StereoscopicRendering)
						EditorPrefs.SetBool(ignore + stereoscopicRendering, true);
#if !UNITY_5_0
					if (PlayerSettings.virtualRealitySupported != recommended_VirtualRealitySupported)
						EditorPrefs.SetBool(ignore + virtualRealitySupported, true);
#endif
					Close();
				}
			}
		}
		else if (GUILayout.Button("Close"))
		{
			Close();
		}

		GUILayout.EndHorizontal();
	}
}

