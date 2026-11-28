using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FD RID: 1277
	internal static class MaterialAllocator
	{
		// Token: 0x060025E1 RID: 9697 RVA: 0x000E6CA0 File Offset: 0x000E4EA0
		public static Material Create(Material material)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Attempted to create duplicate material off main thread, do this in ensure materials initialized.");
				return null;
			}
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000E6CFC File Offset: 0x000E4EFC
		public static Material Create(Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Attempted to create material off main thread, do this in ensure materials initialized.");
				return null;
			}
			Material material = (DefDatabase<ShaderDef>.DefCount == 0) ? Material.CreateDeferred(() => new Material(shader)) : new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000E6D84 File Offset: 0x000E4F84
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Destroying material ");
				defaultInterpolatedStringHandler.AppendFormatted<Material>(material);
				defaultInterpolatedStringHandler.AppendLiteral(", but that material was not created through the MaterialTracker");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			MaterialAllocator.references.Remove(material);
			Object.Destroy(material);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000E6DE8 File Offset: 0x000E4FE8
		private static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Material allocator has allocated ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(MaterialAllocator.references.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" materials; this may be a sign of a material leak");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000E6E73 File Offset: 0x000E5073
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000E6E7C File Offset: 0x000E507C
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000E6F38 File Offset: 0x000E5138
		[DebugOutput("System", false)]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000E6FC4 File Offset: 0x000E51C4
		[DebugOutput("System", false)]
		public static void MaterialDelta()
		{
			IEnumerable<string> source = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			foreach (string text in (from k in source
			select new KeyValuePair<string, int>(k, currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0)) into kvp
			orderby kvp.Value descending
			select kvp into g
			select string.Format("{0}: {1}", g.Value, g.Key)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x04001B4F RID: 6991
		private static bool trackMaterialStackTrace = false;

		// Token: 0x04001B50 RID: 6992
		private static readonly Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x04001B51 RID: 6993
		private static int nextWarningThreshold;

		// Token: 0x04001B52 RID: 6994
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x0200274B RID: 10059
		private struct MaterialInfo
		{
			// Token: 0x0400A0DB RID: 41179
			public string stackTrace;
		}
	}
}
