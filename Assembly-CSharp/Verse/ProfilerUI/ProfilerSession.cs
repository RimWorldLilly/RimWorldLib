using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Waylith;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079F RID: 1951
	public sealed class ProfilerSession
	{
		// Token: 0x06003E37 RID: 15927 RVA: 0x0018BFAC File Offset: 0x0018A1AC
		public ProfilerSession()
		{
			this.Tree.FoldRecursion = ProfilerPrefs.Current.foldRecursion;
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06003E38 RID: 15928 RVA: 0x0018C05A File Offset: 0x0018A25A
		public ProfilerTreeModel Tree { get; } = new ProfilerTreeModel();

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x0018C062 File Offset: 0x0018A262
		public ProfilerSearchModel Search { get; } = new ProfilerSearchModel();

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003E3A RID: 15930 RVA: 0x0018C06A File Offset: 0x0018A26A
		// (set) Token: 0x06003E3B RID: 15931 RVA: 0x0018C072 File Offset: 0x0018A272
		public ProfilerFrameSource Source { get; private set; }

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003E3C RID: 15932 RVA: 0x0018C07B File Offset: 0x0018A27B
		public bool IsSpikes
		{
			get
			{
				return this.Source == ProfilerFrameSource.Spikes;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0018C086 File Offset: 0x0018A286
		public bool IsPaused
		{
			get
			{
				return FrameProfiler.Paused;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06003E3E RID: 15934 RVA: 0x0018C08D File Offset: 0x0018A28D
		public bool FollowingNewest
		{
			get
			{
				return this.ActivePin.FollowingNewest;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003E3F RID: 15935 RVA: 0x0018C09A File Offset: 0x0018A29A
		// (set) Token: 0x06003E40 RID: 15936 RVA: 0x0018C0A2 File Offset: 0x0018A2A2
		public bool HasData { get; private set; }

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x0018C0AB File Offset: 0x0018A2AB
		// (set) Token: 0x06003E42 RID: 15938 RVA: 0x0018C0B3 File Offset: 0x0018A2B3
		public FrameProfiler.FrameSnapshot Snapshot { get; private set; }

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x0018C0BC File Offset: 0x0018A2BC
		public int Ordinal
		{
			get
			{
				if (!this.HasData)
				{
					return -1;
				}
				return this.Snapshot.captureOrdinal;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x0018C0D3 File Offset: 0x0018A2D3
		// (set) Token: 0x06003E45 RID: 15941 RVA: 0x0018C0DB File Offset: 0x0018A2DB
		public int Span { get; private set; } = 1;

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x0018C0E4 File Offset: 0x0018A2E4
		// (set) Token: 0x06003E47 RID: 15943 RVA: 0x0018C0EC File Offset: 0x0018A2EC
		public long SelectionStartNs { get; private set; } = -1L;

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0018C0F5 File Offset: 0x0018A2F5
		// (set) Token: 0x06003E49 RID: 15945 RVA: 0x0018C0FD File Offset: 0x0018A2FD
		public long SelectionEndNs { get; private set; } = -1L;

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0018C106 File Offset: 0x0018A306
		// (set) Token: 0x06003E4B RID: 15947 RVA: 0x0018C10E File Offset: 0x0018A30E
		public int DisplayOffset { get; private set; }

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0018C117 File Offset: 0x0018A317
		// (set) Token: 0x06003E4D RID: 15949 RVA: 0x0018C11F File Offset: 0x0018A31F
		public int DisplayOffsetOldest { get; private set; }

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0018C128 File Offset: 0x0018A328
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x0018C130 File Offset: 0x0018A330
		public bool SelectionAgedOut { get; private set; }

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x0018C139 File Offset: 0x0018A339
		// (set) Token: 0x06003E51 RID: 15953 RVA: 0x0018C141 File Offset: 0x0018A341
		public int Version { get; private set; }

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0018C14A File Offset: 0x0018A34A
		// (set) Token: 0x06003E53 RID: 15955 RVA: 0x0018C152 File Offset: 0x0018A352
		public int RefreshTick { get; private set; }

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x0018C15B File Offset: 0x0018A35B
		// (set) Token: 0x06003E55 RID: 15957 RVA: 0x0018C163 File Offset: 0x0018A363
		public int RefreshIntervalFrames { get; private set; } = 1;

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x0018C16C File Offset: 0x0018A36C
		public ProfilerFrameStats Stats
		{
			get
			{
				int num = FrameProfiler.CompletedFrameCount / 10;
				if (this.statsCaptureBucket != num)
				{
					this.statsCaptureBucket = num;
					this.stats = ProfilerFrameStore.ComputeStats();
				}
				return this.stats;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x0018C1A3 File Offset: 0x0018A3A3
		public float BudgetMs
		{
			get
			{
				if (ProfilerPrefs.Current.budgetMs > 0f)
				{
					return ProfilerPrefs.Current.budgetMs;
				}
				if (EngineWindow.TargetFrameRate <= 0)
				{
					return 16.666666f;
				}
				return 1000f / (float)EngineWindow.TargetFrameRate;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06003E58 RID: 15960 RVA: 0x0018C1DB File Offset: 0x0018A3DB
		private ProfilerFrameStore.FramePin ActivePin
		{
			get
			{
				if (!this.IsSpikes)
				{
					return this.historyPin;
				}
				return this.spikePin;
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x0018C1F4 File Offset: 0x0018A3F4
		public void Invalidate()
		{
			int version = this.Version;
			this.Version = version + 1;
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0018C214 File Offset: 0x0018A414
		public void Poll()
		{
			using (FrameProfiler.BeginRead())
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				this.HasData = this.TryResolveDisplayedFrame(out frameSnapshot);
				FrameProfiler.FrameSnapshot snapshot = this.Snapshot;
				this.Snapshot = (this.HasData ? frameSnapshot : null);
				FrameProfiler.FrameSnapshot snapshot2 = this.Snapshot;
				if (snapshot2 != null)
				{
					snapshot2.Retain();
				}
				if (snapshot != null)
				{
					snapshot.Release();
				}
				this.Span = (this.HasData ? this.ResolveSelectionSpan() : 1);
				ProfilerThreadRegistry.Sync(this.Source, (this.SelectionAgedOut && this.HasData) ? this.Snapshot : null);
				this.UpdateSelectionRange();
				this.AdvanceRefreshTick();
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0018C2D0 File Offset: 0x0018A4D0
		private void AdvanceRefreshTick()
		{
			int num = Mathf.Max(Mathf.Abs(this.Ordinal - this.lastRefreshOrdinal), Mathf.Abs(this.Span - this.lastRefreshSpan));
			if (this.Version == this.lastRefreshVersion && num < this.RefreshIntervalFrames)
			{
				return;
			}
			int refreshTick = this.RefreshTick;
			this.RefreshTick = refreshTick + 1;
			this.lastRefreshVersion = this.Version;
			this.lastRefreshOrdinal = this.Ordinal;
			this.lastRefreshSpan = this.Span;
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x0018C354 File Offset: 0x0018A554
		public void ReportRefreshCost(long startTimestamp)
		{
			float num = (float)Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
			this.RefreshIntervalFrames = Mathf.Clamp(Mathf.CeilToInt(num / (this.BudgetMs * 0.1f)), 1, 60);
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0018C394 File Offset: 0x0018A594
		private void UpdateSelectionRange()
		{
			this.SelectionStartNs = -1L;
			this.SelectionEndNs = -1L;
			int offset;
			int offset2;
			FrameProfiler.FrameSnapshot frameSnapshot;
			FrameProfiler.FrameSnapshot frameSnapshot2;
			if (this.manualRangeEndNs > this.manualRangeStartNs && this.manualRangeStartNs >= 0L)
			{
				this.SelectionStartNs = this.manualRangeStartNs;
				this.SelectionEndNs = this.manualRangeEndNs;
			}
			else if (!this.IsSpikes && this.HasData && this.TryGetSelectionBand(out offset, out offset2) && this.TryGetSnapshotAt(offset2, out frameSnapshot) && this.TryGetSnapshotAt(offset, out frameSnapshot2))
			{
				this.SelectionStartNs = frameSnapshot.beginTimeNanoseconds;
				this.SelectionEndNs = frameSnapshot2.endTimeNanoseconds;
			}
			this.Tree.clipStartNs = this.SelectionStartNs;
			this.Tree.clipEndNs = this.SelectionEndNs;
			ProfilerFrameStore.FramePin activePin = this.ActivePin;
			int a = (this.selectionOldestOrdinal >= 0) ? this.selectionOldestOrdinal : activePin.PinnedOrdinal;
			this.Tree.SetWindow(this.IsSpikes, activePin.FollowingNewest, activePin.PinnedOrdinal, Mathf.Min(a, activePin.PinnedOrdinal), (this.SelectionAgedOut && this.HasData) ? this.Snapshot : null);
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0018C4B4 File Offset: 0x0018A6B4
		public void SelectTimeRange(int newestOffset, int oldestOffset, long startNs, long endNs)
		{
			if (this.IsSpikes || endNs <= startNs)
			{
				return;
			}
			this.PinHistoryFrame(newestOffset);
			if (oldestOffset > newestOffset)
			{
				this.ExtendSelection(oldestOffset);
			}
			this.manualRangeStartNs = startNs;
			this.manualRangeEndNs = endNs;
			int version = this.Version;
			this.Version = version + 1;
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x0018C500 File Offset: 0x0018A700
		public bool TryGetSnapshotAt(int offset, out FrameProfiler.FrameSnapshot snapshot)
		{
			return ProfilerFrameStore.TryGetAt(this.Source, offset, out snapshot);
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x0018C510 File Offset: 0x0018A710
		public void CollectFrames(int span, List<FrameProfiler.FrameSnapshot> into)
		{
			into.Clear();
			if (!this.HasData)
			{
				return;
			}
			if (span <= 1)
			{
				into.Add(this.Snapshot);
				return;
			}
			for (int i = this.DisplayOffset; i < this.DisplayOffset + span; i++)
			{
				FrameProfiler.FrameSnapshot item;
				if (this.TryGetSnapshotAt(i, out item))
				{
					into.Add(item);
				}
			}
			if (into.Count == 0)
			{
				into.Add(this.Snapshot);
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x0018C57C File Offset: 0x0018A77C
		public ProfilerTreeModel.WindowSummary SummarizeSelection()
		{
			ProfilerTreeModel.WindowSummary windowSummary = default(ProfilerTreeModel.WindowSummary);
			long num = 0L;
			for (int i = this.DisplayOffset; i < this.DisplayOffset + this.Span; i++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				if (this.TryGetSnapshotAt(i, out frameSnapshot))
				{
					windowSummary.framesIncluded++;
					num += (long)Mathf.RoundToInt(frameSnapshot.totalMs * 1000f);
					windowSummary.averageTickerDelta += (float)frameSnapshot.tickDelta;
					windowSummary.averageNodeCount += (float)frameSnapshot.publishedNodeCount;
					windowSummary.averageOverflowedScopes += (float)frameSnapshot.overflowedScopes;
					windowSummary.averageAllocBytes += frameSnapshot.DisplayAllocBytes;
					windowSummary.averageAllocCount += (float)frameSnapshot.allocatedObjectCount;
					windowSummary.gen0Collections += frameSnapshot.gen0Collections;
					windowSummary.gen1Collections += frameSnapshot.gen1Collections;
					windowSummary.gen2Collections += frameSnapshot.gen2Collections;
					windowSummary.averagePromotedBytes += frameSnapshot.promotedBytes;
				}
			}
			if (windowSummary.framesIncluded == 0)
			{
				return windowSummary;
			}
			windowSummary.averageFrameMs = (float)num / (1000f * (float)windowSummary.framesIncluded);
			windowSummary.averageTickerDelta /= (float)windowSummary.framesIncluded;
			windowSummary.averageNodeCount /= (float)windowSummary.framesIncluded;
			windowSummary.averageOverflowedScopes /= (float)windowSummary.framesIncluded;
			windowSummary.averageAllocBytes /= (long)windowSummary.framesIncluded;
			windowSummary.averageAllocCount /= (float)windowSummary.framesIncluded;
			windowSummary.averagePromotedBytes /= (long)windowSummary.framesIncluded;
			return windowSummary;
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x0018C714 File Offset: 0x0018A914
		public void SetPaused(bool paused)
		{
			if (FrameProfiler.Paused == paused)
			{
				return;
			}
			FrameProfiler.SetPaused(paused);
			int version = this.Version;
			this.Version = version + 1;
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x0018C740 File Offset: 0x0018A940
		public void ClearFrames()
		{
			if (this.IsSpikes)
			{
				FrameProfiler.ClearSpikes();
			}
			else
			{
				FrameProfiler.ClearHistory();
			}
			this.ActivePin.FollowNewest();
			ProfilerThreadRegistry.Reset();
			this.statsCaptureBucket = -1;
			int version = this.Version;
			this.Version = version + 1;
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x0018C788 File Offset: 0x0018A988
		public void SwitchSource(ProfilerFrameSource newSource)
		{
			if (this.Source == newSource)
			{
				return;
			}
			this.historyPin.FollowNewest();
			this.spikePin.FollowNewest();
			this.Source = newSource;
			ProfilerThreadRegistry.Reset();
			this.ClearSelection();
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x0018C7BC File Offset: 0x0018A9BC
		public void BeginSelection(int newestOffset)
		{
			if (this.IsSpikes)
			{
				if (this.spikePin.PinAtOffset(Mathf.Clamp(newestOffset, 0, FrameProfiler.SpikeCount - 1)))
				{
					this.selectionAnchorOrdinal = this.spikePin.PinnedOrdinal;
					this.selectionOldestOrdinal = this.spikePin.PinnedOrdinal;
					this.SetPaused(true);
					int version = this.Version;
					this.Version = version + 1;
					Action displayedFrameChanged = this.DisplayedFrameChanged;
					if (displayedFrameChanged == null)
					{
						return;
					}
					displayedFrameChanged();
				}
				return;
			}
			this.PinHistoryFrame(newestOffset);
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0018C840 File Offset: 0x0018AA40
		public void PinHistoryFrame(int newestOffset)
		{
			this.manualRangeStartNs = -1L;
			this.manualRangeEndNs = -1L;
			int num = ProfilerFrameStore.CountOf(ProfilerFrameSource.History);
			if (num <= 0)
			{
				return;
			}
			int newestOffset2 = Mathf.Clamp(newestOffset, 0, num - 1);
			if (!this.historyPin.PinAtOffset(newestOffset2))
			{
				return;
			}
			this.selectionAnchorOrdinal = this.historyPin.PinnedOrdinal;
			this.selectionOldestOrdinal = this.historyPin.PinnedOrdinal;
			this.SetPaused(true);
			int version = this.Version;
			this.Version = version + 1;
			Action displayedFrameChanged = this.DisplayedFrameChanged;
			if (displayedFrameChanged == null)
			{
				return;
			}
			displayedFrameChanged();
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0018C8CC File Offset: 0x0018AACC
		public void ExtendSelection(int newestOffset)
		{
			this.manualRangeStartNs = -1L;
			this.manualRangeEndNs = -1L;
			ProfilerFrameStore.FramePin activePin = this.ActivePin;
			int count = activePin.Count;
			if (count <= 0)
			{
				return;
			}
			if (activePin.FollowingNewest || this.selectionAnchorOrdinal < 0)
			{
				this.BeginSelection(newestOffset);
				return;
			}
			int offset = Mathf.Clamp(newestOffset, 0, count - 1);
			FrameProfiler.FrameSnapshot frameSnapshot;
			if (!this.TryGetSnapshotAt(offset, out frameSnapshot))
			{
				return;
			}
			int captureOrdinal = frameSnapshot.captureOrdinal;
			int num = Mathf.Max(this.selectionAnchorOrdinal, captureOrdinal);
			int num2 = Mathf.Min(this.selectionAnchorOrdinal, captureOrdinal);
			if (num2 == this.selectionOldestOrdinal && this.ActivePin.PinnedOrdinal == num)
			{
				return;
			}
			this.selectionOldestOrdinal = num2;
			int version = this.Version;
			this.Version = version + 1;
			if (activePin.PinnedOrdinal != num)
			{
				int num3 = ProfilerSession.OrdinalToOffset(this.Source, num);
				if (num3 >= 0)
				{
					activePin.PinAtOffset(num3);
				}
			}
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0018C9B0 File Offset: 0x0018ABB0
		public void ClearSelection()
		{
			this.ActivePin.FollowNewest();
			this.selectionAnchorOrdinal = -1;
			this.selectionOldestOrdinal = -1;
			this.manualRangeStartNs = -1L;
			this.manualRangeEndNs = -1L;
			int version = this.Version;
			this.Version = version + 1;
			Action selectionReset = this.SelectionReset;
			if (selectionReset == null)
			{
				return;
			}
			selectionReset();
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0018CA06 File Offset: 0x0018AC06
		public void JumpToNewest()
		{
			this.ClearSelection();
			if (!this.IsSpikes)
			{
				this.SetPaused(false);
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0018CA20 File Offset: 0x0018AC20
		public void Step(int direction)
		{
			ProfilerFrameStore.FramePin activePin = this.ActivePin;
			int count = activePin.Count;
			if (count <= 0)
			{
				return;
			}
			int num2;
			int num = (activePin.FollowingNewest ? -1 : (activePin.TryResolveOffset(out num2) ? num2 : count)) + direction;
			if (num <= -1 && direction < 0)
			{
				this.JumpToNewest();
				return;
			}
			this.BeginSelection(Mathf.Clamp(num, 0, count - 1));
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x0018CA7C File Offset: 0x0018AC7C
		public bool PinOrdinal(int ordinal, bool keepIfInsideSelection, out bool selectionChanged)
		{
			selectionChanged = false;
			ProfilerFrameStore.FramePin activePin = this.ActivePin;
			if ((keepIfInsideSelection && !activePin.FollowingNewest && this.selectionOldestOrdinal >= 0 && ordinal >= this.selectionOldestOrdinal && ordinal <= activePin.PinnedOrdinal) || (!activePin.FollowingNewest && activePin.PinnedOrdinal == ordinal))
			{
				return true;
			}
			int num = ProfilerSession.OrdinalToOffset(this.Source, ordinal);
			if (num < 0)
			{
				return false;
			}
			this.BeginSelection(num);
			selectionChanged = true;
			return true;
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x0018CAF4 File Offset: 0x0018ACF4
		public void SelectZoneTimeRange(int ordinal, long startNs, long endNs)
		{
			int num;
			if (ProfilerFrameStore.TryOrdinalToOffset(this.Source, ordinal, out num))
			{
				this.SelectTimeRange(num, num, startNs, endNs);
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x0018CB1C File Offset: 0x0018AD1C
		public bool TryGetSnapshotForOrdinal(int ordinal, out FrameProfiler.FrameSnapshot snapshot)
		{
			if (this.SelectionAgedOut && this.HasData && this.Snapshot.captureOrdinal == ordinal)
			{
				snapshot = this.Snapshot;
				return true;
			}
			snapshot = null;
			int offset;
			return ProfilerFrameStore.TryOrdinalToOffset(this.Source, ordinal, out offset) && this.TryGetSnapshotAt(offset, out snapshot);
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x0018CB70 File Offset: 0x0018AD70
		public bool TryGetSelectionBand(out int newestOffset, out int oldestOffset)
		{
			newestOffset = -1;
			oldestOffset = -1;
			if (this.IsSpikes)
			{
				if (this.DisplayOffset < 0 || this.DisplayOffset >= FrameProfiler.SpikeCount)
				{
					return false;
				}
			}
			else if (this.historyPin.FollowingNewest || this.DisplayOffset < 0 || this.DisplayOffset >= FrameProfiler.HistoryCount)
			{
				return false;
			}
			newestOffset = this.DisplayOffset;
			oldestOffset = this.DisplayOffsetOldest;
			return true;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x0018CBDC File Offset: 0x0018ADDC
		private static int OrdinalToOffset(ProfilerFrameSource source, int ordinal)
		{
			int result;
			if (!ProfilerFrameStore.TryOrdinalToOffset(source, ordinal, out result))
			{
				return -1;
			}
			return result;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x0018CBF8 File Offset: 0x0018ADF8
		private bool TryResolveDisplayedFrame(out FrameProfiler.FrameSnapshot snapshot)
		{
			if (this.IsSpikes)
			{
				int num;
				this.DisplayOffset = (this.spikePin.FollowingNewest ? 0 : (this.spikePin.TryResolveOffset(out num) ? num : -1));
				this.DisplayOffsetOldest = this.DisplayOffset;
				this.SelectionAgedOut = (!this.spikePin.FollowingNewest && this.DisplayOffset < 0);
				if (!this.spikePin.FollowingNewest && this.DisplayOffset >= 0 && this.selectionOldestOrdinal >= 0 && this.selectionOldestOrdinal != this.spikePin.PinnedOrdinal)
				{
					int num2 = ProfilerSession.OrdinalToOffset(ProfilerFrameSource.Spikes, this.selectionOldestOrdinal);
					this.DisplayOffsetOldest = ((num2 >= this.DisplayOffset) ? num2 : (FrameProfiler.SpikeCount - 1));
				}
				return this.spikePin.TryGetSnapshot(out snapshot);
			}
			this.SelectionAgedOut = false;
			if (this.historyPin.FollowingNewest)
			{
				this.DisplayOffset = 0;
				this.DisplayOffsetOldest = Mathf.Max(0, FrameProfiler.HistoryCount - 1);
				return this.historyPin.TryGetSnapshot(out snapshot);
			}
			int num3;
			if (this.historyPin.TryResolveOffset(out num3))
			{
				this.DisplayOffset = num3;
				this.DisplayOffsetOldest = Mathf.Min(num3 + (this.historyPin.PinnedOrdinal - this.selectionOldestOrdinal), FrameProfiler.HistoryCount - 1);
				return this.historyPin.TryGetSnapshot(out snapshot);
			}
			this.DisplayOffset = FrameProfiler.HistoryCount;
			this.DisplayOffsetOldest = this.DisplayOffset;
			this.SelectionAgedOut = true;
			return this.historyPin.TryGetSnapshot(out snapshot);
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x0018CD78 File Offset: 0x0018AF78
		private int ResolveSelectionSpan()
		{
			if (this.IsSpikes)
			{
				if (this.spikePin.FollowingNewest || this.DisplayOffset < 0)
				{
					return 1;
				}
				return Mathf.Min(this.DisplayOffsetOldest, FrameProfiler.SpikeCount - 1) - this.DisplayOffset + 1;
			}
			else
			{
				int historyCount = FrameProfiler.HistoryCount;
				if (this.historyPin.FollowingNewest)
				{
					return Mathf.Max(1, historyCount);
				}
				if (this.DisplayOffset >= historyCount)
				{
					return 1;
				}
				return Mathf.Min(this.DisplayOffsetOldest, historyCount - 1) - this.DisplayOffset + 1;
			}
		}

		// Token: 0x04002900 RID: 10496
		private const int StatsRecomputeIntervalFrames = 10;

		// Token: 0x04002901 RID: 10497
		private const float RefreshBudgetShare = 0.1f;

		// Token: 0x04002902 RID: 10498
		private const int MaxRefreshIntervalFrames = 60;

		// Token: 0x04002903 RID: 10499
		private readonly ProfilerFrameStore.FramePin historyPin = new ProfilerFrameStore.FramePin(ProfilerFrameSource.History);

		// Token: 0x04002904 RID: 10500
		private readonly ProfilerFrameStore.FramePin spikePin = new ProfilerFrameStore.FramePin(ProfilerFrameSource.Spikes);

		// Token: 0x04002905 RID: 10501
		private int selectionAnchorOrdinal = -1;

		// Token: 0x04002906 RID: 10502
		private int selectionOldestOrdinal = -1;

		// Token: 0x04002907 RID: 10503
		private long manualRangeStartNs = -1L;

		// Token: 0x04002908 RID: 10504
		private long manualRangeEndNs = -1L;

		// Token: 0x04002909 RID: 10505
		private int statsCaptureBucket = -1;

		// Token: 0x0400290A RID: 10506
		private int lastRefreshVersion = -1;

		// Token: 0x0400290B RID: 10507
		private int lastRefreshOrdinal = -1;

		// Token: 0x0400290C RID: 10508
		private int lastRefreshSpan = -1;

		// Token: 0x0400290D RID: 10509
		public Action DisplayedFrameChanged;

		// Token: 0x0400290E RID: 10510
		public Action SelectionReset;

		// Token: 0x0400291D RID: 10525
		private ProfilerFrameStats stats;
	}
}
