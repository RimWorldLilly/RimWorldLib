using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Waylith.Profiling;
using Waylith.Profiling.Instrumentation;

namespace Verse.ProfilerUI
{
	// Token: 0x02000795 RID: 1941
	public static class ProfilerPrefs
	{
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0018993E File Offset: 0x00187B3E
		public static ProfilerPrefsData Current
		{
			get
			{
				ProfilerPrefsData result;
				if ((result = ProfilerPrefs.data) == null)
				{
					result = (ProfilerPrefs.data = ProfilerPrefs.Load());
				}
				return result;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x00189954 File Offset: 0x00187B54
		private static string FilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Profiler.xml");
			}
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00189968 File Offset: 0x00187B68
		private static ProfilerPrefsData Load()
		{
			ProfilerInstrumentation.ExcludedAssemblies = new string[]
			{
				"0Harmony",
				"MonoMod.*",
				"Mono.Cecil*"
			};
			ProfilerPrefsData profilerPrefsData = null;
			try
			{
				if (File.Exists(ProfilerPrefs.FilePath))
				{
					profilerPrefsData = DirectXmlLoader.ItemFromXmlFile<ProfilerPrefsData>(ProfilerPrefs.FilePath, false);
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Could not load profiler prefs, using defaults: " + ex.Message);
			}
			if (profilerPrefsData == null)
			{
				profilerPrefsData = new ProfilerPrefsData();
			}
			ProfilerPrefsData profilerPrefsData2 = profilerPrefsData;
			if (profilerPrefsData2.instrumentationTargets == null)
			{
				profilerPrefsData2.instrumentationTargets = new List<string>(ProfilerPrefs.DefaultTargets);
			}
			return profilerPrefsData;
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x00189A00 File Offset: 0x00187C00
		public static void Reset()
		{
			Rect windowRect = ProfilerPrefs.Current.windowRect;
			ProfilerPrefs.data = new ProfilerPrefsData
			{
				windowRect = windowRect,
				instrumentationTargets = new List<string>(ProfilerPrefs.DefaultTargets)
			};
			ProfilerPrefs.Apply();
			ProfilerPrefs.Save();
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00189A44 File Offset: 0x00187C44
		public static void Save()
		{
			try
			{
				DirectXmlSaver.SaveDataObject(ProfilerPrefs.Current, ProfilerPrefs.FilePath);
			}
			catch (Exception ex)
			{
				Log.Warning("Could not save profiler prefs: " + ex.Message);
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x00189A8C File Offset: 0x00187C8C
		public static void Apply()
		{
			ProfilerPrefsData profilerPrefsData = ProfilerPrefs.Current;
			FrameProfiler.SetHistoryCapacity(profilerPrefsData.historyCapacity);
			profilerPrefsData.historyCapacity = FrameProfiler.HistoryCapacity;
			if (FrameProfiler.AllocationTrackingSupported)
			{
				FrameProfiler.SetAllocationTracking(profilerPrefsData.allocTracking);
			}
			profilerPrefsData.allocTracking = FrameProfiler.AllocationTracking;
			FrameProfiler.SetMainThreadOnly(profilerPrefsData.mainThreadOnly);
			profilerPrefsData.mainThreadOnly = FrameProfiler.MainThreadOnly;
			FrameProfiler.IgnoreThrottle = profilerPrefsData.ignoreThrottle;
			FrameProfiler.AutoMuteTrivialLabels = profilerPrefsData.autoMuteTrivial;
			ProfilerInstrumentation.SetTargets(profilerPrefsData.instrumentationTargets);
			profilerPrefsData.instrumentationTargets = new List<string>(ProfilerInstrumentation.Targets);
			if (ProfilerInstrumentation.IsSupported)
			{
				ProfilerInstrumentation.SetEnabled(profilerPrefsData.instrumentation);
			}
			profilerPrefsData.instrumentation = ProfilerInstrumentation.IsEnabled;
			profilerPrefsData.budgetMs = ((profilerPrefsData.budgetMs <= 0f) ? 0f : Mathf.Clamp(profilerPrefsData.budgetMs, 1f, 1000f));
		}

		// Token: 0x040028A5 RID: 10405
		private static readonly string[] DefaultTargets = new string[]
		{
			"Assembly-CSharp!Verse.*::*",
			"Assembly-CSharp!RimWorld.*::*"
		};

		// Token: 0x040028A6 RID: 10406
		private static ProfilerPrefsData data;
	}
}
