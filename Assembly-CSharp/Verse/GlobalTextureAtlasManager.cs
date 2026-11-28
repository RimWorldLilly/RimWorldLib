using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Brutal.VulkanApi;
using RimWorld;
using UnityEngine;
using Waylith.Graphics;
using Waylith.Graphics.Textures;
using Waylith.Logging;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x020004F0 RID: 1264
	public static class GlobalTextureAtlasManager
	{
		// Token: 0x0600257F RID: 9599 RVA: 0x000E3D81 File Offset: 0x000E1F81
		public static void ClearStaticAtlasBuildQueue()
		{
			GlobalTextureAtlasManager.buildQueue.Clear();
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000E3D90 File Offset: 0x000E1F90
		public static bool TryInsertStatic(TextureAtlasGroup group, UnityEngine.Texture2D texture, UnityEngine.Texture2D mask = null)
		{
			if (texture.width >= 512 || texture.height >= 512)
			{
				return false;
			}
			if (mask != null && (texture.width != mask.width || texture.height != mask.height))
			{
				Log.Warning(string.Concat(new string[]
				{
					"Texture ",
					texture.name,
					" has dimensions of ",
					texture.width.ToString(),
					" x ",
					texture.height.ToString(),
					", but its mask has ",
					mask.width.ToString(),
					" x ",
					mask.height.ToString(),
					". This is not supported, texture will be excluded from atlas"
				}));
				return false;
			}
			TextureAtlasGroupKey key = new TextureAtlasGroupKey
			{
				group = group,
				hasMask = (mask != null)
			};
			ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>> valueTuple;
			if (!GlobalTextureAtlasManager.buildQueue.TryGetValue(key, out valueTuple))
			{
				valueTuple = new ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>>(new List<UnityEngine.Texture2D>(), new HashSet<UnityEngine.Texture2D>());
				GlobalTextureAtlasManager.buildQueue.Add(key, valueTuple);
			}
			if (valueTuple.Item2.Add(texture))
			{
				valueTuple.Item1.Add(texture);
			}
			if (mask != null)
			{
				if (GlobalTextureAtlasManager.buildQueueMasks.ContainsKey(texture))
				{
					if (GlobalTextureAtlasManager.buildQueueMasks[texture] != mask)
					{
						Log.Error(string.Concat(new string[]
						{
							"Same texture with 2 different masks inserted into texture atlas manager (",
							texture.name,
							") - ",
							mask.name,
							" | ",
							GlobalTextureAtlasManager.buildQueueMasks[texture].name,
							"!"
						}));
					}
				}
				else
				{
					GlobalTextureAtlasManager.buildQueueMasks.Add(texture, mask);
				}
			}
			return true;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000E3F6C File Offset: 0x000E216C
		public static void BakeStaticAtlases()
		{
			BuildingsDamageSectionLayerUtility.TryInsertIntoAtlas();
			MinifiedThing.TryInsertIntoAtlas();
			GlobalTextureAtlasManager.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.pixels = 0;
			CS$<>8__locals1.currentBatch = new List<UnityEngine.Texture2D>();
			try
			{
				foreach (KeyValuePair<TextureAtlasGroupKey, ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>>> keyValuePair in GlobalTextureAtlasManager.buildQueue.OrderBy((KeyValuePair<TextureAtlasGroupKey, ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>>> g) => g.Key.ToString(), StringComparer.Ordinal))
				{
					keyValuePair.Value.Item1.Sort(delegate(UnityEngine.Texture2D a, UnityEngine.Texture2D b)
					{
						Waylith.Rendering.Texture2D coreTexture = a.CoreTexture;
						string strA = ((coreTexture != null) ? coreTexture.SourcePath : null) ?? a.name;
						Waylith.Rendering.Texture2D coreTexture2 = b.CoreTexture;
						return string.CompareOrdinal(strA, ((coreTexture2 != null) ? coreTexture2.SourcePath : null) ?? b.name);
					});
					foreach (UnityEngine.Texture2D texture2D in keyValuePair.Value.Item1)
					{
						int num = (texture2D.width + 8) * (texture2D.height + 8);
						if (num + CS$<>8__locals1.pixels > StaticTextureAtlas.MaxPixelsPerAtlas)
						{
							GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|10_0(keyValuePair.Key, ref CS$<>8__locals1);
						}
						CS$<>8__locals1.pixels += num;
						CS$<>8__locals1.currentBatch.Add(texture2D);
					}
					GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|10_0(keyValuePair.Key, ref CS$<>8__locals1);
				}
			}
			finally
			{
				GlobalTextureAtlasManager.evictableSources = (from texture in GlobalTextureAtlasManager.staticTextureAtlases.SelectMany((StaticTextureAtlas atlas) => atlas.Members)
				where texture != null
				select texture).ToHashSet<UnityEngine.Texture2D>();
				GlobalTextureAtlasManager.evictableCores = (from texture in GlobalTextureAtlasManager.evictableSources
				select texture.CoreTexture into core
				where core != null
				select core).ToHashSet<Waylith.Rendering.Texture2D>();
				Waylith.Rendering.Texture2D.EndDeferredLoading(new Func<Waylith.Rendering.Texture2D, bool>(GlobalTextureAtlasManager.LeavesSourceEvicted));
			}
			GlobalTextureAtlasManager.DrainCacheWrites();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Static atlases: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(GlobalTextureAtlasManager.staticTextureAtlases.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" pages, ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(GlobalTextureAtlasManager.staticTextureAtlases.Count((StaticTextureAtlas atlas) => atlas.AdoptedFromCache));
			defaultInterpolatedStringHandler.AppendLiteral(" loaded from cache, page limit ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(StaticTextureAtlas.MaxPagePixels / 1048576);
			defaultInterpolatedStringHandler.AppendLiteral(" Mpx.");
			VerboseLog.Log(defaultInterpolatedStringHandler.ToStringAndClear());
			GlobalTextureAtlasManager.EvictAtlasSourceTextures();
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002582 RID: 9602 RVA: 0x000E4264 File Offset: 0x000E2464
		private static bool AtlasSourceEvictionEnabled
		{
			get
			{
				return !GenCommandLine.CommandLineArgPassed("noEvictAtlasSources");
			}
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000E4273 File Offset: 0x000E2473
		public static bool LeavesSourceEvicted(UnityEngine.Texture2D texture)
		{
			return GlobalTextureAtlasManager.AtlasSourceEvictionEnabled && GlobalTextureAtlasManager.evictableSources.Contains(texture);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000E4289 File Offset: 0x000E2489
		public static bool LeavesSourceEvicted(Waylith.Rendering.Texture2D texture)
		{
			return GlobalTextureAtlasManager.AtlasSourceEvictionEnabled && GlobalTextureAtlasManager.evictableCores.Contains(texture);
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002585 RID: 9605 RVA: 0x000E429F File Offset: 0x000E249F
		internal static bool AtlasCompressionEnabled
		{
			get
			{
				return Prefs.TextureCompression && TextureCompressionSettings.Enabled && !GenCommandLine.CommandLineArgPassed("noCompressAtlases");
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000E42C0 File Offset: 0x000E24C0
		internal static void CompressAtlas(StaticTextureAtlas atlas)
		{
			if (!GlobalTextureAtlasManager.AtlasCompressionEnabled)
			{
				return;
			}
			using (new DeepProfilerScope("Compress atlas textures"))
			{
				BcEncodeTarget target = TextureCompressionSettings.Target;
				float quality = TextureCompressionSettings.Quality;
				GlobalTextureAtlasManager.CompressAtlasPage(atlas.ColorTexture, atlas.CacheKey(false), target, quality);
				GlobalTextureAtlasManager.CompressAtlasPage(atlas.MaskTexture, atlas.CacheKey(true), target, quality);
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000E4330 File Offset: 0x000E2530
		private static void CompressAtlasPage(UnityEngine.Texture2D page, string cacheKey, BcEncodeTarget target, float quality)
		{
			Waylith.Rendering.Texture2D texture2D = (page != null) ? page.CoreTexture : null;
			if (((texture2D != null) ? texture2D.CpuData : null) == null)
			{
				return;
			}
			if (TexturePixelData.IsBlockCompressed(texture2D.CpuData.Format))
			{
				return;
			}
			LoadedTextureData loadedTextureData;
			if (cacheKey != null && GpuTextureService.TryLoadKeyedTextureCache(cacheKey, out loadedTextureData) && texture2D.TryAdoptCompressed(loadedTextureData))
			{
				GlobalTextureAtlasManager.ReleaseCpuLevels(texture2D);
				return;
			}
			if (GlobalTextureAtlasManager.TryEncodeOnGpu(page, cacheKey, target, quality))
			{
				return;
			}
			if (!texture2D.CompressInPlace(target, quality, default(TextureSwizzle)))
			{
				return;
			}
			if (cacheKey != null)
			{
				GlobalTextureAtlasManager.QueueCacheWrite(GpuTextureService.BeginWriteKeyedTextureCache(cacheKey, texture2D));
			}
			GlobalTextureAtlasManager.ReleaseCpuLevels(texture2D);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000E43C0 File Offset: 0x000E25C0
		private static bool TryEncodeOnGpu(UnityEngine.Texture2D page, string cacheKey, BcEncodeTarget target, float quality)
		{
			Waylith.Rendering.Texture2D coreTexture = page.CoreTexture;
			if (target == BcEncodeTarget.Bc7)
			{
				TexturePixelData cpuData = coreTexture.CpuData;
				if (cpuData != null && !cpuData.HasData && coreTexture.GpuResource != null && TextureBlockEncoder.CanEncodeBc7Fast)
				{
					GpuTextureAllocation gpuTextureAllocation = GpuTextureService.CreateEmptyTexture2D(page.name, page.width, page.height, coreTexture.MipCount, TexturePixelData.ToVkFormat(LoadedTextureFormat.Bc7), VkImageUsageFlags.None, GpuMemoryCategory.Atlas);
					byte[][] array = (gpuTextureAllocation == null) ? null : TextureBlockEncoder.TryEncodeBc7Fast(coreTexture.GpuResource, gpuTextureAllocation, page.width, page.height, coreTexture.MipCount, quality);
					if (array == null)
					{
						if (gpuTextureAllocation != null)
						{
							gpuTextureAllocation.Dispose();
						}
						return false;
					}
					coreTexture.AdoptEncodedImage(gpuTextureAllocation, LoadedTextureFormat.Bc7, array.Length);
					if (cacheKey != null)
					{
						GlobalTextureAtlasManager.QueueCacheWrite(GpuTextureService.BeginWriteKeyedTextureCache(cacheKey, new LoadedTextureData(page.width, page.height, array.Length, LoadedTextureFormat.Bc7, array, false, null, default(TextureSwizzle))));
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000E449D File Offset: 0x000E269D
		private static void ReleaseCpuLevels(Waylith.Rendering.Texture2D core)
		{
			TexturePixelData cpuData = core.CpuData;
			if (cpuData != null)
			{
				cpuData.ReleaseTracked();
			}
			TexturePixelData cpuData2 = core.CpuData;
			if (cpuData2 == null)
			{
				return;
			}
			cpuData2.Release();
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000E44C0 File Offset: 0x000E26C0
		private static void QueueCacheWrite(Task write)
		{
			GlobalTextureAtlasManager.pendingCacheWrites.RemoveAll((Task pending) => pending.IsCompleted);
			if (!write.IsCompleted)
			{
				GlobalTextureAtlasManager.pendingCacheWrites.Add(write);
			}
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000E4500 File Offset: 0x000E2700
		private static void DrainCacheWrites()
		{
			if (GlobalTextureAtlasManager.pendingCacheWrites.Count == 0)
			{
				return;
			}
			using (new DeepProfilerScope("Atlas cache write drain"))
			{
				Task.WaitAll(GlobalTextureAtlasManager.pendingCacheWrites.ToArray());
			}
			GlobalTextureAtlasManager.pendingCacheWrites.Clear();
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000E455C File Offset: 0x000E275C
		public static void EvictAtlasSourceTextures()
		{
			if (!GlobalTextureAtlasManager.AtlasSourceEvictionEnabled)
			{
				return;
			}
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				staticTextureAtlas.EvictSourceTextures();
			}
			GpuTextureService.RetireDeferredDisposalsAfterIdle();
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000E45B8 File Offset: 0x000E27B8
		public static bool TryGetStaticTile(TextureAtlasGroup group, UnityEngine.Texture2D texture, out StaticTextureAtlasTile tile, bool ignoreFoundInOtherAtlas = false)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas.groupKey.group == group && staticTextureAtlas.TryGetTile(texture, out tile))
				{
					return true;
				}
			}
			foreach (StaticTextureAtlas staticTextureAtlas2 in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas2.TryGetTile(texture, out tile))
				{
					if (!ignoreFoundInOtherAtlas)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Found texture ",
							texture.name,
							" in another atlas group than requested (found in ",
							staticTextureAtlas2.groupKey.ToString(),
							", requested in ",
							group.ToString(),
							")!"
						}));
					}
					return true;
				}
			}
			tile = null;
			return false;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000E46D8 File Offset: 0x000E28D8
		public static bool TryGetPawnFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew, bool allowCreatingNew = true)
		{
			using (List<PawnTextureAtlas>.Enumerator enumerator = GlobalTextureAtlasManager.pawnTextureAtlases.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryGetFrameSet(pawn, out frameSet, out createdNew))
					{
						return true;
					}
				}
			}
			if (allowCreatingNew)
			{
				PawnTextureAtlas pawnTextureAtlas = new PawnTextureAtlas();
				GlobalTextureAtlasManager.pawnTextureAtlases.Add(pawnTextureAtlas);
				return pawnTextureAtlas.TryGetFrameSet(pawn, out frameSet, out createdNew);
			}
			createdNew = false;
			frameSet = null;
			return false;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000E4758 File Offset: 0x000E2958
		public static bool TryMarkPawnFrameSetDirty(Pawn pawn)
		{
			PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
			bool flag;
			if (!GlobalTextureAtlasManager.TryGetPawnFrameSet(pawn, out pawnTextureAtlasFrameSet, out flag, false))
			{
				return false;
			}
			for (int i = 0; i < pawnTextureAtlasFrameSet.isDirty.Length; i++)
			{
				pawnTextureAtlasFrameSet.isDirty[i] = true;
			}
			return true;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000E4794 File Offset: 0x000E2994
		public static void GlobalTextureAtlasManagerUpdate()
		{
			if (GlobalTextureAtlasManager.rebakeAtlas)
			{
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				PortraitsCache.Clear();
				GlobalTextureAtlasManager.rebakeAtlas = false;
			}
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.GC();
			}
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000E47FC File Offset: 0x000E29FC
		public static void FreeAllRuntimeAtlases()
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.Destroy();
			}
			GlobalTextureAtlasManager.pawnTextureAtlases.Clear();
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000E4858 File Offset: 0x000E2A58
		public static void DumpPawnAtlases(string folder)
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(pawnTextureAtlas.RawTexture, folder + "\\dump_" + pawnTextureAtlas.RawTexture.GetInstanceID().ToString() + ".png");
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000E48E4 File Offset: 0x000E2AE4
		public static void DumpStaticAtlases(string folder)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.ColorTexture, folder + "\\" + staticTextureAtlas.ColorTexture.name + ".png");
				if (staticTextureAtlas.MaskTexture != null)
				{
					TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.MaskTexture, folder + "\\" + staticTextureAtlas.MaskTexture.name + ".png");
				}
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000E49F8 File Offset: 0x000E2BF8
		[CompilerGenerated]
		internal static void <BakeStaticAtlases>g__FlushBatch|10_0(TextureAtlasGroupKey groupKey, ref GlobalTextureAtlasManager.<>c__DisplayClass10_0 A_1)
		{
			StaticTextureAtlas staticTextureAtlas = new StaticTextureAtlas(groupKey);
			foreach (UnityEngine.Texture2D texture2D in A_1.currentBatch)
			{
				UnityEngine.Texture2D mask;
				if (!groupKey.hasMask || !GlobalTextureAtlasManager.buildQueueMasks.TryGetValue(texture2D, out mask))
				{
					mask = null;
				}
				staticTextureAtlas.Insert(texture2D, mask);
			}
			staticTextureAtlas.Bake(false);
			GlobalTextureAtlasManager.staticTextureAtlases.Add(staticTextureAtlas);
			if (GlobalTextureAtlasManager.AtlasSourceEvictionEnabled)
			{
				staticTextureAtlas.EvictSourceTextures();
			}
			GpuTextureService.RetireDeferredDisposalsAfterIdle();
			A_1.pixels = 0;
			A_1.currentBatch.Clear();
		}

		// Token: 0x04001B13 RID: 6931
		public static bool rebakeAtlas = false;

		// Token: 0x04001B14 RID: 6932
		private static List<PawnTextureAtlas> pawnTextureAtlases = new List<PawnTextureAtlas>();

		// Token: 0x04001B15 RID: 6933
		private static List<StaticTextureAtlas> staticTextureAtlases = new List<StaticTextureAtlas>();

		// Token: 0x04001B16 RID: 6934
		private static HashSet<UnityEngine.Texture2D> evictableSources = new HashSet<UnityEngine.Texture2D>();

		// Token: 0x04001B17 RID: 6935
		private static HashSet<Waylith.Rendering.Texture2D> evictableCores = new HashSet<Waylith.Rendering.Texture2D>();

		// Token: 0x04001B18 RID: 6936
		private static readonly List<Task> pendingCacheWrites = new List<Task>();

		// Token: 0x04001B19 RID: 6937
		private static Dictionary<TextureAtlasGroupKey, ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>>> buildQueue = new Dictionary<TextureAtlasGroupKey, ValueTuple<List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>>>();

		// Token: 0x04001B1A RID: 6938
		private static Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D> buildQueueMasks = new Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D>();
	}
}
