using System;
using System.Collections.Generic;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A2 RID: 1954
	internal static class ProfilerThreadRegistry
	{
		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003E96 RID: 16022 RVA: 0x0018EB88 File Offset: 0x0018CD88
		// (set) Token: 0x06003E97 RID: 16023 RVA: 0x0018EB8F File Offset: 0x0018CD8F
		public static int Version { get; private set; }

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0018EB97 File Offset: 0x0018CD97
		// (set) Token: 0x06003E99 RID: 16025 RVA: 0x0018EB9E File Offset: 0x0018CD9E
		public static int SelectionVersion { get; private set; }

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003E9A RID: 16026 RVA: 0x0018EBA6 File Offset: 0x0018CDA6
		public static IReadOnlyList<ProfilerThreadRegistry.Entry> Entries
		{
			get
			{
				return ProfilerThreadRegistry.entries;
			}
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0018EBB0 File Offset: 0x0018CDB0
		public static void Sync(ProfilerFrameSource source, FrameProfiler.FrameSnapshot agedOutFrame)
		{
			using (FrameProfiler.BeginRead())
			{
				FrameProfiler.CollectTimelines(ProfilerThreadRegistry.timelineScratch);
				ProfilerThreadRegistry.presentThreadIds.Clear();
				if (agedOutFrame != null)
				{
					ProfilerThreadRegistry.AddThreadsOf(agedOutFrame);
				}
				else
				{
					if (source == ProfilerFrameSource.History)
					{
						int num = ProfilerFrameStore.CountOf(source);
						FrameProfiler.FrameSnapshot frameSnapshot;
						FrameProfiler.FrameSnapshot frameSnapshot2;
						if (!ProfilerFrameStore.TryGetAt(source, 0, out frameSnapshot) || !ProfilerFrameStore.TryGetAt(source, num - 1, out frameSnapshot2))
						{
							goto IL_EA;
						}
						long beginTimeNanoseconds = frameSnapshot2.beginTimeNanoseconds;
						long endNs = frameSnapshot.beginTimeNanoseconds + frameSnapshot.nodesEndNanoseconds;
						using (List<ThreadTimeline>.Enumerator enumerator = ProfilerThreadRegistry.timelineScratch.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ThreadTimeline threadTimeline = enumerator.Current;
								if (threadTimeline.isMain || FrameProfiler.HasChunks(threadTimeline, beginTimeNanoseconds, endNs))
								{
									ProfilerThreadRegistry.presentThreadIds.Add(threadTimeline.threadId);
								}
							}
							goto IL_EA;
						}
					}
					for (int i = 0; i < ProfilerFrameStore.CountOf(source); i++)
					{
						FrameProfiler.FrameSnapshot frame;
						if (ProfilerFrameStore.TryGetAt(source, i, out frame))
						{
							ProfilerThreadRegistry.AddThreadsOf(frame);
						}
					}
				}
				IL_EA:
				ProfilerThreadRegistry.Apply();
			}
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0018ECD8 File Offset: 0x0018CED8
		private static void AddThreadsOf(FrameProfiler.FrameSnapshot frame)
		{
			long beginTimeNanoseconds = frame.beginTimeNanoseconds;
			long endNs = beginTimeNanoseconds + frame.nodesEndNanoseconds;
			foreach (ThreadTimeline threadTimeline in ProfilerThreadRegistry.timelineScratch)
			{
				if (threadTimeline.isMain || FrameProfiler.HasChunks(frame, threadTimeline.threadId, beginTimeNanoseconds, endNs))
				{
					ProfilerThreadRegistry.presentThreadIds.Add(threadTimeline.threadId);
				}
			}
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0018ED5C File Offset: 0x0018CF5C
		private static void Apply()
		{
			bool flag = false;
			for (int i = ProfilerThreadRegistry.entries.Count - 1; i >= 0; i--)
			{
				if (!ProfilerThreadRegistry.presentThreadIds.Contains(ProfilerThreadRegistry.entries[i].threadId))
				{
					ProfilerThreadRegistry.selectedThreadIds.Remove(ProfilerThreadRegistry.entries[i].threadId);
					ProfilerThreadRegistry.entries.RemoveAt(i);
					flag = true;
				}
			}
			foreach (ThreadTimeline threadTimeline in ProfilerThreadRegistry.timelineScratch)
			{
				if (ProfilerThreadRegistry.presentThreadIds.Contains(threadTimeline.threadId) && ProfilerThreadRegistry.IndexOf(threadTimeline.threadId) < 0)
				{
					ProfilerThreadRegistry.Insert(new ProfilerThreadRegistry.Entry
					{
						threadId = threadTimeline.threadId,
						nameLabelId = threadTimeline.nameLabelId,
						isMain = threadTimeline.isMain
					});
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			ProfilerThreadRegistry.Version++;
			ProfilerThreadRegistry.SelectionVersion++;
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0018EE78 File Offset: 0x0018D078
		private static int IndexOf(int threadId)
		{
			for (int i = 0; i < ProfilerThreadRegistry.entries.Count; i++)
			{
				if (ProfilerThreadRegistry.entries[i].threadId == threadId)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0018EEB0 File Offset: 0x0018D0B0
		private static void Insert(ProfilerThreadRegistry.Entry entry)
		{
			int index = ProfilerThreadRegistry.entries.Count;
			if (entry.isMain)
			{
				index = 0;
			}
			else
			{
				string label = FrameProfiler.GetLabel(entry.nameLabelId);
				for (int i = 0; i < ProfilerThreadRegistry.entries.Count; i++)
				{
					if (!ProfilerThreadRegistry.entries[i].isMain && string.CompareOrdinal(label, FrameProfiler.GetLabel(ProfilerThreadRegistry.entries[i].nameLabelId)) < 0)
					{
						index = i;
						break;
					}
				}
			}
			ProfilerThreadRegistry.entries.Insert(index, entry);
			if (entry.isMain)
			{
				ProfilerThreadRegistry.selectedThreadIds.Add(entry.threadId);
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x0018EF50 File Offset: 0x0018D150
		public static bool AllSelected
		{
			get
			{
				for (int i = 0; i < ProfilerThreadRegistry.entries.Count; i++)
				{
					if (!ProfilerThreadRegistry.selectedThreadIds.Contains(ProfilerThreadRegistry.entries[i].threadId))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0018EF91 File Offset: 0x0018D191
		public static bool IsSelected(int threadId)
		{
			return ProfilerThreadRegistry.selectedThreadIds.Contains(threadId);
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0018EFA0 File Offset: 0x0018D1A0
		public static void GetSelectionSummary(out int selected, out int singleIndex)
		{
			if (ProfilerThreadRegistry.summaryVersion != ProfilerThreadRegistry.Version || ProfilerThreadRegistry.summarySelectionVersion != ProfilerThreadRegistry.SelectionVersion)
			{
				ProfilerThreadRegistry.summaryVersion = ProfilerThreadRegistry.Version;
				ProfilerThreadRegistry.summarySelectionVersion = ProfilerThreadRegistry.SelectionVersion;
				ProfilerThreadRegistry.summarySelected = 0;
				ProfilerThreadRegistry.summarySingleIndex = -1;
				for (int i = 0; i < ProfilerThreadRegistry.entries.Count; i++)
				{
					if (ProfilerThreadRegistry.selectedThreadIds.Contains(ProfilerThreadRegistry.entries[i].threadId))
					{
						ProfilerThreadRegistry.summarySelected++;
						ProfilerThreadRegistry.summarySingleIndex = i;
					}
				}
			}
			selected = ProfilerThreadRegistry.summarySelected;
			singleIndex = ProfilerThreadRegistry.summarySingleIndex;
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0018F036 File Offset: 0x0018D236
		public static void SetSelected(int threadId, bool value)
		{
			if (value ? (!ProfilerThreadRegistry.selectedThreadIds.Add(threadId)) : (!ProfilerThreadRegistry.selectedThreadIds.Remove(threadId)))
			{
				return;
			}
			ProfilerThreadRegistry.SelectionVersion++;
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0018F068 File Offset: 0x0018D268
		public static void SelectOnly(int threadId)
		{
			if (ProfilerThreadRegistry.selectedThreadIds.Count == 1 && ProfilerThreadRegistry.selectedThreadIds.Contains(threadId))
			{
				return;
			}
			ProfilerThreadRegistry.selectedThreadIds.Clear();
			ProfilerThreadRegistry.selectedThreadIds.Add(threadId);
			ProfilerThreadRegistry.SelectionVersion++;
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x0018F0A7 File Offset: 0x0018D2A7
		public static void Reset()
		{
			ProfilerThreadRegistry.entries.Clear();
			ProfilerThreadRegistry.selectedThreadIds.Clear();
			ProfilerThreadRegistry.Version++;
			ProfilerThreadRegistry.SelectionVersion++;
		}

		// Token: 0x0400294E RID: 10574
		private static readonly List<ProfilerThreadRegistry.Entry> entries = new List<ProfilerThreadRegistry.Entry>();

		// Token: 0x0400294F RID: 10575
		private static readonly HashSet<int> selectedThreadIds = new HashSet<int>();

		// Token: 0x04002950 RID: 10576
		private static readonly HashSet<int> presentThreadIds = new HashSet<int>();

		// Token: 0x04002951 RID: 10577
		private static readonly List<ThreadTimeline> timelineScratch = new List<ThreadTimeline>();

		// Token: 0x04002954 RID: 10580
		private static int summaryVersion = -1;

		// Token: 0x04002955 RID: 10581
		private static int summarySelectionVersion = -1;

		// Token: 0x04002956 RID: 10582
		private static int summarySelected;

		// Token: 0x04002957 RID: 10583
		private static int summarySingleIndex;

		// Token: 0x02002AA5 RID: 10917
		public struct Entry
		{
			// Token: 0x0400AEBA RID: 44730
			public int threadId;

			// Token: 0x0400AEBB RID: 44731
			public ProfilerLabelId nameLabelId;

			// Token: 0x0400AEBC RID: 44732
			public bool isMain;
		}
	}
}
