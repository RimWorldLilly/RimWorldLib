using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000107 RID: 263
	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x000340A9 File Offset: 0x000322A9
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000340B0 File Offset: 0x000322B0
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x000340B7 File Offset: 0x000322B7
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000340C4 File Offset: 0x000322C4
		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				string sourceName = modContentPack.ToString();
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (!hashSet.Add(t.defName))
					{
						string[] array = new string[7];
						array[0] = "Mod ";
						int num = 1;
						ModContentPack modContentPack2 = modContentPack;
						array[num] = ((modContentPack2 != null) ? modContentPack2.ToString() : null);
						array[2] = " has multiple ";
						int num2 = 3;
						Type typeFromHandle = typeof(T);
						array[num2] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
						array[4] = "s named ";
						array[5] = t.defName;
						array[6] = ". Skipping.";
						Log.Error(string.Concat(array));
					}
					else
					{
						DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(t, sourceName);
					}
				}
			}
			foreach (!0 def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(def, "Patches");
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00034278 File Offset: 0x00032478
		public static void Add(IEnumerable<T> defs)
		{
			foreach (!0 def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000342C0 File Offset: 0x000324C0
		public static void Add(T def)
		{
			if (def == null)
			{
				Log.Error("Tried to add null def to DefDatabase.");
				return;
			}
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				string str = "Adding duplicate ";
				Type typeFromHandle = typeof(T);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " name: " + def.defName);
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f).ToString();
				def.ResolveDefNameHash();
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				string str2 = "Too many ";
				Type typeFromHandle2 = typeof(T);
				Log.Error(str2 + ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null) + "; over " + ushort.MaxValue.ToString());
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000343E9 File Offset: 0x000325E9
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00034412 File Offset: 0x00032612
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
			DefDatabase<T>.defsByShortHash.Clear();
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00034434 File Offset: 0x00032634
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0003446C File Offset: 0x0003266C
		public static void ResolveAllReferences(bool onlyExactlyMyType = true, bool parallel = false)
		{
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ResolveAllReferences " + typeof(T).FullName);
			try
			{
				Action<T> action = delegate(T def)
				{
					if (onlyExactlyMyType && def.GetType() != typeof(T))
					{
						return;
					}
					DeepProfiler.Start("Resolver call");
					try
					{
						def.ResolveReferences();
					}
					catch (Exception ex)
					{
						string str = "Error while resolving references for def ";
						T t = def;
						string str2 = (t != null) ? t.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (parallel)
				{
					GenThreading.ParallelForEach<T>(DefDatabase<T>.defsList, action, -1);
				}
				else
				{
					for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
					{
						action(DefDatabase<T>.defsList[i]);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00034548 File Offset: 0x00032748
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
			for (int j = 0; j < DefDatabase<T>.defsList.Count; j++)
			{
				DefDatabase<T>.defsList[j].PostSetIndices();
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000345AC File Offset: 0x000327AC
		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string str in t.ConfigErrors())
						{
							string str2 = "Config error in ";
							T t2 = t;
							Log.Error(str2 + ((t2 != null) ? t2.ToString() : null) + ": " + str);
						}
					}
				}
				catch (Exception ex)
				{
					string str3 = "Exception in ConfigErrors() of ";
					string defName = t.defName;
					string str4 = ": ";
					Exception ex2 = ex;
					Log.Error(str3 + defName + str4 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x000346A4 File Offset: 0x000328A4
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				string[] array = new string[7];
				array[0] = "Failed to find ";
				int num = 1;
				Type typeFromHandle = typeof(T);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[2] = " named ";
				array[3] = defName;
				array[4] = ". There are ";
				array[5] = DefDatabase<T>.defsList.Count.ToString();
				array[6] = " defs of this type loaded.";
				Log.Error(string.Concat(array));
				return default(T);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return default(T);
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0003474B File Offset: 0x0003294B
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00034754 File Offset: 0x00032954
		public static T GetByShortHash(ushort shortHash)
		{
			T result;
			if (DefDatabase<T>.defsByShortHash.TryGetValue(shortHash, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0003477C File Offset: 0x0003297C
		public static void InitializeShortHashDictionary()
		{
			DefDatabase<T>.defsByShortHash.EnsureCapacity(DefDatabase<T>.defsList.Count);
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsByShortHash[DefDatabase<T>.defsList[i].shortHash] = DefDatabase<T>.defsList[i];
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000347DD File Offset: 0x000329DD
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0003480C File Offset: 0x00032A0C
		[CompilerGenerated]
		internal static void <AddAllInMods>g__AddDef|9_0(T def, string sourceName)
		{
			if (def.defName == "UnnamedDef")
			{
				string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
				Log.Error(string.Concat(new string[]
				{
					typeof(T).Name,
					" in ",
					sourceName,
					" with label ",
					def.label,
					" lacks a defName. Giving name ",
					text
				}));
				def.defName = text;
				def.ResolveDefNameHash();
			}
			T def2;
			if (DefDatabase<T>.defsByName.TryGetValue(def.defName, out def2))
			{
				DefDatabase<T>.Remove(def2);
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04000443 RID: 1091
		private static readonly List<T> defsList = new List<T>();

		// Token: 0x04000444 RID: 1092
		private static readonly Dictionary<string, T> defsByName = new Dictionary<string, T>();

		// Token: 0x04000445 RID: 1093
		private static readonly Dictionary<ushort, T> defsByShortHash = new Dictionary<ushort, T>();
	}
}
