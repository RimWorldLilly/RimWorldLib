using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x0200078F RID: 1935
	[NullableContext(2)]
	[Nullable(0)]
	internal static class ProfilerFrameStore
	{
		// Token: 0x06003D68 RID: 15720 RVA: 0x00186F15 File Offset: 0x00185115
		public static int CountOf(ProfilerFrameSource source)
		{
			if (source != ProfilerFrameSource.History)
			{
				return FrameProfiler.SpikeCount;
			}
			return FrameProfiler.HistoryCount;
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00186F25 File Offset: 0x00185125
		public static bool TryGetAt(ProfilerFrameSource source, int newestOffset, [NotNullWhen(true)] out FrameProfiler.FrameSnapshot snapshot)
		{
			if (source != ProfilerFrameSource.History)
			{
				return FrameProfiler.TryGetSpikeSnapshot(newestOffset, out snapshot);
			}
			return FrameProfiler.TryGetHistorySnapshot(newestOffset, out snapshot);
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00186F3C File Offset: 0x0018513C
		public static bool TryOrdinalToOffset(ProfilerFrameSource source, int ordinal, out int newestOffset)
		{
			newestOffset = -1;
			if (ordinal < 0)
			{
				return false;
			}
			if (source != ProfilerFrameSource.History)
			{
				for (int i = 0; i < FrameProfiler.SpikeCount; i++)
				{
					FrameProfiler.FrameSnapshot frameSnapshot;
					if (ProfilerFrameStore.TryGetAt(source, i, out frameSnapshot) && frameSnapshot.captureOrdinal == ordinal)
					{
						newestOffset = i;
						return true;
					}
				}
				return false;
			}
			FrameProfiler.FrameSnapshot frameSnapshot2;
			if (!ProfilerFrameStore.TryGetAt(source, 0, out frameSnapshot2))
			{
				return false;
			}
			int num = frameSnapshot2.captureOrdinal - ordinal;
			if (num < 0 || num >= FrameProfiler.HistoryCount)
			{
				return false;
			}
			newestOffset = num;
			return true;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x00186FA8 File Offset: 0x001851A8
		public static ProfilerFrameStats ComputeStats()
		{
			int historyCount = FrameProfiler.HistoryCount;
			if (ProfilerFrameStore.statsScratch.Length < historyCount)
			{
				ProfilerFrameStore.statsScratch = new float[FrameProfiler.HistoryCapacity];
			}
			int num = 0;
			for (int i = 0; i < historyCount; i++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				if (FrameProfiler.TryGetHistorySnapshot(i, out frameSnapshot))
				{
					ProfilerFrameStore.statsScratch[num++] = frameSnapshot.totalMs;
				}
			}
			if (num == 0)
			{
				return default(ProfilerFrameStats);
			}
			Array.Sort<float>(ProfilerFrameStore.statsScratch, 0, num);
			return new ProfilerFrameStats
			{
				medianMs = ProfilerFrameStore.statsScratch[num / 2],
				p99Ms = ProfilerFrameStore.statsScratch[Math.Min(num - 1, (int)((float)num * 0.99f))]
			};
		}

		// Token: 0x0400282E RID: 10286
		[Nullable(1)]
		private static float[] statsScratch = Array.Empty<float>();

		// Token: 0x02002A9F RID: 10911
		[Nullable(0)]
		public sealed class FramePin
		{
			// Token: 0x06010707 RID: 67335 RVA: 0x005C7FE1 File Offset: 0x005C61E1
			public FramePin(ProfilerFrameSource source)
			{
				this.source = source;
			}

			// Token: 0x17002BE8 RID: 11240
			// (get) Token: 0x06010708 RID: 67336 RVA: 0x005C7FF7 File Offset: 0x005C61F7
			public bool FollowingNewest
			{
				get
				{
					return this.PinnedOrdinal < 0;
				}
			}

			// Token: 0x17002BE9 RID: 11241
			// (get) Token: 0x06010709 RID: 67337 RVA: 0x005C8002 File Offset: 0x005C6202
			// (set) Token: 0x0601070A RID: 67338 RVA: 0x005C800A File Offset: 0x005C620A
			public int PinnedOrdinal { get; private set; } = -1;

			// Token: 0x17002BEA RID: 11242
			// (get) Token: 0x0601070B RID: 67339 RVA: 0x005C8013 File Offset: 0x005C6213
			public int Count
			{
				get
				{
					return ProfilerFrameStore.CountOf(this.source);
				}
			}

			// Token: 0x0601070C RID: 67340 RVA: 0x005C8020 File Offset: 0x005C6220
			public void FollowNewest()
			{
				this.PinnedOrdinal = -1;
				FrameProfiler.FrameSnapshot frameSnapshot = this.retainedCopy;
				if (frameSnapshot != null)
				{
					frameSnapshot.Release();
				}
				this.retainedCopy = null;
			}

			// Token: 0x0601070D RID: 67341 RVA: 0x005C8044 File Offset: 0x005C6244
			public bool PinAtOffset(int newestOffset)
			{
				bool result;
				using (FrameProfiler.BeginRead())
				{
					FrameProfiler.FrameSnapshot frameSnapshot;
					if (!ProfilerFrameStore.TryGetAt(this.source, newestOffset, out frameSnapshot))
					{
						result = false;
					}
					else
					{
						this.PinnedOrdinal = frameSnapshot.captureOrdinal;
						this.RefreshRetainedCopy(frameSnapshot);
						result = true;
					}
				}
				return result;
			}

			// Token: 0x0601070E RID: 67342 RVA: 0x005C80A4 File Offset: 0x005C62A4
			public bool TryResolveOffset(out int newestOffset)
			{
				newestOffset = -1;
				return !this.FollowingNewest && ProfilerFrameStore.TryOrdinalToOffset(this.source, this.PinnedOrdinal, out newestOffset);
			}

			// Token: 0x0601070F RID: 67343 RVA: 0x005C80C8 File Offset: 0x005C62C8
			public bool TryGetSnapshot([NotNullWhen(true)] out FrameProfiler.FrameSnapshot snapshot)
			{
				bool result;
				using (FrameProfiler.BeginRead())
				{
					int newestOffset;
					if (this.FollowingNewest)
					{
						result = ProfilerFrameStore.TryGetAt(this.source, 0, out snapshot);
					}
					else if (this.TryResolveOffset(out newestOffset) && ProfilerFrameStore.TryGetAt(this.source, newestOffset, out snapshot))
					{
						this.RefreshRetainedCopy(snapshot);
						result = true;
					}
					else if (this.retainedCopy != null)
					{
						snapshot = this.retainedCopy;
						result = true;
					}
					else
					{
						snapshot = null;
						result = false;
					}
				}
				return result;
			}

			// Token: 0x06010710 RID: 67344 RVA: 0x005C8154 File Offset: 0x005C6354
			[NullableContext(1)]
			private void RefreshRetainedCopy(FrameProfiler.FrameSnapshot snapshot)
			{
				if (this.retainedCopy == snapshot)
				{
					return;
				}
				snapshot.Retain();
				snapshot.RetainZones();
				FrameProfiler.FrameSnapshot frameSnapshot = this.retainedCopy;
				if (frameSnapshot != null)
				{
					frameSnapshot.Release();
				}
				this.retainedCopy = snapshot;
			}

			// Token: 0x0400AEA3 RID: 44707
			private readonly ProfilerFrameSource source;

			// Token: 0x0400AEA4 RID: 44708
			private FrameProfiler.FrameSnapshot retainedCopy;
		}
	}
}
