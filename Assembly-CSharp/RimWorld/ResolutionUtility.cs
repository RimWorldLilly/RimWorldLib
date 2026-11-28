using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Waylith;

namespace RimWorld
{
	// Token: 0x02001CC4 RID: 7364
	public static class ResolutionUtility
	{
		// Token: 0x1700226C RID: 8812
		// (get) Token: 0x0600BD24 RID: 48420 RVA: 0x0043D2C4 File Offset: 0x0043B4C4
		public static float DpiScale
		{
			get
			{
				if (!Prefs.RespectSystemScaling)
				{
					return 1f;
				}
				return Screen.dpi / 96f;
			}
		}

		// Token: 0x1700226D RID: 8813
		// (get) Token: 0x0600BD25 RID: 48421 RVA: 0x0043D2E0 File Offset: 0x0043B4E0
		public static UnityEngine.Resolution NativeResolution
		{
			get
			{
				UnityEngine.Resolution[] resolutions = Screen.resolutions;
				if (resolutions.Length == 0)
				{
					return Screen.currentResolution;
				}
				UnityEngine.Resolution result = resolutions[0];
				for (int i = 1; i < resolutions.Length; i++)
				{
					if (resolutions[i].width > result.width || (resolutions[i].width == result.width && resolutions[i].height > result.height))
					{
						result = resolutions[i];
					}
				}
				return result;
			}
		}

		// Token: 0x1700226E RID: 8814
		// (get) Token: 0x0600BD26 RID: 48422 RVA: 0x0043D35B File Offset: 0x0043B55B
		public static bool BorderlessFullscreen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BD27 RID: 48423 RVA: 0x0043D360 File Offset: 0x0043B560
		public static void SafeSetResolution(UnityEngine.Resolution res)
		{
			if (Screen.width == res.width && Screen.height == res.height)
			{
				return;
			}
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			ResolutionUtility.SetResolutionRaw(res.width, res.height, Screen.fullScreen);
			Prefs.ScreenWidth = res.width;
			Prefs.ScreenHeight = res.height;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
		}

		// Token: 0x0600BD28 RID: 48424 RVA: 0x0043D3DC File Offset: 0x0043B5DC
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen == fullScreen)
			{
				return;
			}
			bool fullScreen2 = Screen.fullScreen;
			Screen.fullScreen = fullScreen;
			Prefs.FullScreen = fullScreen;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
		}

		// Token: 0x0600BD29 RID: 48425 RVA: 0x0043D414 File Offset: 0x0043B614
		public static void SafeSetFullScreenMode(FullScreenMode mode)
		{
			FullScreenMode fullScreenMode = Screen.fullScreenMode;
			if (fullScreenMode == mode)
			{
				return;
			}
			IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
			Screen.fullScreenMode = mode;
			if (fullScreenMode == FullScreenMode.Windowed)
			{
				UnityEngine.Resolution nativeResolution = ResolutionUtility.NativeResolution;
				ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, mode);
				Prefs.ScreenWidth = nativeResolution.width;
				Prefs.ScreenHeight = nativeResolution.height;
			}
			Prefs.FullScreenMode = mode;
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreenMode, oldRes));
		}

		// Token: 0x0600BD2A RID: 48426 RVA: 0x0043D490 File Offset: 0x0043B690
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScalePreference == newScale)
			{
				return;
			}
			float uiscalePreference = Prefs.UIScalePreference;
			Prefs.UIScalePreference = newScale;
			GenUI.ClearLabelWidthCache();
			Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscalePreference));
		}

		// Token: 0x0600BD2B RID: 48427 RVA: 0x0043D4C7 File Offset: 0x0043B6C7
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			scale *= ResolutionUtility.DpiScale;
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x0600BD2C RID: 48428 RVA: 0x0043D4F0 File Offset: 0x0043B6F0
		public static void SetResolutionRaw(int w, int h, bool fullScreen)
		{
			if (Application.isBatchMode)
			{
				return;
			}
			if (w <= 0 || h <= 0)
			{
				Log.Error("Tried to set resolution to " + w.ToString() + "x" + h.ToString());
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreen != fullScreen)
			{
				Screen.SetResolution(w, h, fullScreen);
			}
		}

		// Token: 0x0600BD2D RID: 48429 RVA: 0x0043D550 File Offset: 0x0043B750
		public static void SetResolutionRaw(int w, int h, FullScreenMode mode)
		{
			if (Application.isBatchMode)
			{
				return;
			}
			if (w <= 0 || h <= 0)
			{
				Log.Error("Tried to set resolution to " + w.ToString() + "x" + h.ToString());
				return;
			}
			if (Screen.width != w || Screen.height != h || Screen.fullScreenMode != mode)
			{
				Screen.SetResolution(w, h, mode);
			}
		}

		// Token: 0x0600BD2E RID: 48430 RVA: 0x0043D5B0 File Offset: 0x0043B7B0
		public static void SetNativeResolutionRaw()
		{
			UnityEngine.Resolution nativeResolution = ResolutionUtility.NativeResolution;
			ResolutionUtility.SetResolutionRaw(nativeResolution.width, nativeResolution.height, !ResolutionUtility.BorderlessFullscreen);
		}

		// Token: 0x0600BD2F RID: 48431 RVA: 0x0043D5E0 File Offset: 0x0043B7E0
		public static float GetRecommendedUIScale(int screenWidth, int screenHeight)
		{
			if (screenWidth == 0 || screenHeight == 0)
			{
				UnityEngine.Resolution currentResolution = Screen.currentResolution;
				screenWidth = currentResolution.width;
				screenHeight = currentResolution.height;
			}
			if (screenWidth <= 1024 || screenHeight <= 768)
			{
				return 1f;
			}
			int num = Dialog_Options.UIScales.Length - 1;
			while (num >= 0 && Dialog_Options.UIScales[num] >= 1f)
			{
				float num2 = Dialog_Options.UIScales[num] * ResolutionUtility.DpiScale;
				int num3 = Mathf.FloorToInt((float)screenWidth / num2);
				int num4 = Mathf.FloorToInt((float)screenHeight / num2);
				if (num3 >= 1700 && num4 >= 910)
				{
					return Dialog_Options.UIScales[num];
				}
				num--;
			}
			return 1f;
		}

		// Token: 0x0600BD30 RID: 48432 RVA: 0x0043D680 File Offset: 0x0043B880
		public static void Update()
		{
			if (RealTime.frameCount % 30 == 0 && !LongEventHandler.AnyEventNowOrWaiting)
			{
				bool flag = false;
				if (Screen.fullScreenMode != Prefs.FullScreenMode)
				{
					Prefs.FullScreenMode = Screen.fullScreenMode;
					if (Screen.fullScreen)
					{
						UnityEngine.Resolution nativeResolution = ResolutionUtility.NativeResolution;
						Prefs.ScreenWidth = nativeResolution.width;
						Prefs.ScreenHeight = nativeResolution.height;
					}
					flag = true;
				}
				if (!Screen.fullScreen)
				{
					if (Screen.windowWidth != Prefs.ScreenWidth)
					{
						Prefs.ScreenWidth = Screen.windowWidth;
						flag = true;
					}
					if (Screen.windowHeight != Prefs.ScreenHeight)
					{
						Prefs.ScreenHeight = Screen.windowHeight;
						flag = true;
					}
				}
				if (!EngineApplication.Instance.Headless && UI.screenWidth < 1024 && UI.screenHeight < 768 && !Prefs.DevMode)
				{
					int screenWidth = UI.screenWidth;
					int screenHeight = UI.screenHeight;
					Prefs.UIScalePreference = ResolutionUtility.GetRecommendedUIScale(0, 0);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Resolution too small (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(screenWidth);
					defaultInterpolatedStringHandler.AppendLiteral("x");
					defaultInterpolatedStringHandler.AppendFormatted<int>(screenHeight);
					defaultInterpolatedStringHandler.AppendLiteral("), resetting UI scale to ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(Prefs.UIScalePreference);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 72627836);
					flag = true;
				}
				if (flag)
				{
					Prefs.Save();
				}
			}
		}

		// Token: 0x04006C21 RID: 27681
		public const int MinResolutionWidth = 1024;

		// Token: 0x04006C22 RID: 27682
		public const int MinResolutionHeight = 768;

		// Token: 0x04006C23 RID: 27683
		public const int MinRecommendedResolutionWidth = 1700;

		// Token: 0x04006C24 RID: 27684
		public const int MinRecommendedResolutionHeight = 910;
	}
}
