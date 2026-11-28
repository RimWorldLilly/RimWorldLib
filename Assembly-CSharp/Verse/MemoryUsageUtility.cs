using System;
using Unity.Profiling;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006BF RID: 1727
	public static class MemoryUsageUtility
	{
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060036C9 RID: 14025 RVA: 0x0015FC16 File Offset: 0x0015DE16
		public static long TrackedMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.totalMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060036CA RID: 14026 RVA: 0x0015FC22 File Offset: 0x0015DE22
		public static long OsMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.systemMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x060036CB RID: 14027 RVA: 0x0015FC2E File Offset: 0x0015DE2E
		public static long GraphicsMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gfxMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060036CC RID: 14028 RVA: 0x0015FC3A File Offset: 0x0015DE3A
		public static long ManagedMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gcMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060036CD RID: 14029 RVA: 0x0015FC46 File Offset: 0x0015DE46
		public static long ManagedMemoryReservedBytes
		{
			get
			{
				return MemoryUsageUtility.gcReservedMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x0015FC52 File Offset: 0x0015DE52
		public static long AudioMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.audioMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x0015FC5E File Offset: 0x0015DE5E
		public static long TextureMemoryUsageBytes
		{
			get
			{
				return (long)Texture.currentTextureMemory;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060036D0 RID: 14032 RVA: 0x0015FC65 File Offset: 0x0015DE65
		public static long GraphicsTextureMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gfxTextureMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060036D1 RID: 14033 RVA: 0x0015FC71 File Offset: 0x0015DE71
		public static long GraphicsMeshMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gfxMeshMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x0015FC7D File Offset: 0x0015DE7D
		public static long GraphicsMemoryTotalBytes
		{
			get
			{
				return (long)SystemInfo.graphicsMemorySize * 1024L * 1024L;
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x0015FC94 File Offset: 0x0015DE94
		static MemoryUsageUtility()
		{
			MemoryUsageUtility.systemMemoryRecorder.Stop();
			MemoryUsageUtility.totalMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMemoryRecorder.Stop();
			MemoryUsageUtility.gcMemoryRecorder.Stop();
			MemoryUsageUtility.gcReservedMemoryRecorder.Stop();
			MemoryUsageUtility.audioMemoryRecorder.Stop();
			MemoryUsageUtility.gfxTextureMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMeshMemoryRecorder.Stop();
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x0015FDAC File Offset: 0x0015DFAC
		public static void SetShouldRecord(bool shouldRecord)
		{
			if (shouldRecord)
			{
				MemoryUsageUtility.systemMemoryRecorder.Start();
				MemoryUsageUtility.totalMemoryRecorder.Start();
				MemoryUsageUtility.gfxMemoryRecorder.Start();
				MemoryUsageUtility.gcMemoryRecorder.Start();
				MemoryUsageUtility.gcReservedMemoryRecorder.Start();
				MemoryUsageUtility.audioMemoryRecorder.Start();
				MemoryUsageUtility.gfxTextureMemoryRecorder.Start();
				MemoryUsageUtility.gfxMeshMemoryRecorder.Start();
				return;
			}
			MemoryUsageUtility.systemMemoryRecorder.Stop();
			MemoryUsageUtility.totalMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMemoryRecorder.Stop();
			MemoryUsageUtility.gcMemoryRecorder.Stop();
			MemoryUsageUtility.gcReservedMemoryRecorder.Stop();
			MemoryUsageUtility.audioMemoryRecorder.Stop();
			MemoryUsageUtility.gfxTextureMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMeshMemoryRecorder.Stop();
		}

		// Token: 0x04002406 RID: 9222
		private static ProfilerRecorder systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04002407 RID: 9223
		private static ProfilerRecorder totalMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04002408 RID: 9224
		private static ProfilerRecorder gfxMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04002409 RID: 9225
		private static ProfilerRecorder gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x0400240A RID: 9226
		private static ProfilerRecorder gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x0400240B RID: 9227
		private static ProfilerRecorder audioMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x0400240C RID: 9228
		private static ProfilerRecorder gfxTextureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x0400240D RID: 9229
		private static ProfilerRecorder gfxMeshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory", 1, ProfilerRecorderOptions.Default);
	}
}
