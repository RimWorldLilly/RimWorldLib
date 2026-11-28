using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000787 RID: 1927
	public sealed class ProfilerAllocTable : ProfilerRowSection
	{
		// Token: 0x06003CAD RID: 15533 RVA: 0x0017EE18 File Offset: 0x0017D018
		public ProfilerAllocTable(ProfilerSession session) : base(session)
		{
			ProfilerRowListView view = this.View;
			ProfilerColumn[] array = new ProfilerColumn[3];
			array[0] = new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%", null);
			array[1] = new ProfilerColumn(ProfilerColumnKind.Total, 100f, () => "bytes/frame", null);
			array[2] = new ProfilerColumn(ProfilerColumnKind.Calls, 90f, () => "objs/frame", null);
			view.Columns = array;
			this.View.LabelHeader = (() => this.GroupingHeader);
			this.View.RootCrumb = new Func<string>(ProfilerCallTreeTable.RootCrumbLabel);
			this.View.ActivateLeaf = new Action<int>(this.ActivateLeafAt);
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x0017EF85 File Offset: 0x0017D185
		// (set) Token: 0x06003CAF RID: 15535 RVA: 0x0017EF8D File Offset: 0x0017D18D
		public bool IsInverted
		{
			get
			{
				return this.inverted;
			}
			set
			{
				if (value == this.inverted)
				{
					return;
				}
				this.inverted = value;
				this.expandedTypes.Clear();
				this.View.SelectQuietly(-1);
				this.Invalidate();
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x0017EFBD File Offset: 0x0017D1BD
		private string GroupingHeader
		{
			get
			{
				if (!this.inverted)
				{
					return "type / zone";
				}
				return "zone / type";
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x0017EFD2 File Offset: 0x0017D1D2
		protected override string CopyHeader
		{
			get
			{
				return "%\tbytes/frame\tobjs/frame\t" + this.GroupingHeader;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x0017EFE4 File Offset: 0x0017D1E4
		protected override float FooterHeight
		{
			get
			{
				if (this.footer.Length <= 0)
				{
					return 0f;
				}
				return 22f;
			}
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x0017EFFF File Offset: 0x0017D1FF
		public override void Invalidate()
		{
			this.refreshGate.Invalidate();
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x0017F00C File Offset: 0x0017D20C
		protected override void AppendCopyContext(StringBuilder builder)
		{
			if (this.footer.Length > 0)
			{
				builder.AppendLine(this.footer);
			}
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x0017F02C File Offset: 0x0017D22C
		private void ActivateLeafAt(int index)
		{
			ValueTuple<int, int> valueTuple = this.displayRows[index];
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			if (item2 >= 0)
			{
				Action<ProfilerLabelId> zoneJumpRequested = this.ZoneJumpRequested;
				if (zoneJumpRequested == null)
				{
					return;
				}
				zoneJumpRequested(this.aggregated[item].zones[item2].labelId);
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x0017F084 File Offset: 0x0017D284
		public override void Refresh()
		{
			bool flag = this.lastTracking != FrameProfiler.AllocationTracking;
			if (!this.refreshGate.ShouldRefresh(this.session) && !flag)
			{
				return;
			}
			this.lastTracking = FrameProfiler.AllocationTracking;
			this.session.CollectFrames(this.session.Span, this.frames);
			this.Aggregate();
			if (this.inverted)
			{
				this.InvertAggregation();
			}
			bool flag2 = this.expandMatchesOnRefresh;
			if (flag2)
			{
				this.expandMatchesOnRefresh = false;
				if (this.session.Search.IsActive)
				{
					this.ExpandSearchMatches();
				}
			}
			this.SyncRows();
			if (flag2 && this.session.Search.IsActive)
			{
				this.View.SelectFirstMatch();
			}
			this.WriteFooter();
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x0017F148 File Offset: 0x0017D348
		private void WriteFooter()
		{
			if (this.aggregated.Count == 0 || (this.overflowCount <= 0L && this.overflowBytes <= 0L && this.untrackedBytes <= 0L))
			{
				this.footer = "";
				return;
			}
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			if (this.overflowBytes > 0L || this.overflowCount > 0L)
			{
				scratch.Append("over table capacity: ");
				ProfilerTreeModel.AppendAllocBytes(scratch, this.overflowBytes, " ");
				scratch.Append(" (").Append(this.overflowCount.ToString("n0")).Append(" objs)/frame");
			}
			if (this.untrackedBytes > 0L)
			{
				if (scratch.Length > 0)
				{
					scratch.Append(" / ");
				}
				scratch.Append("untracked (main thread): ");
				ProfilerTreeModel.AppendAllocBytes(scratch, this.untrackedBytes, " ");
				scratch.Append("/frame");
			}
			this.footer = scratch.ToString();
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0017F249 File Offset: 0x0017D449
		public override void Notify_SearchChanged()
		{
			this.expandMatchesOnRefresh = true;
			this.Invalidate();
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x0017F258 File Offset: 0x0017D458
		private bool ZoneMatches(ProfilerAllocTable.ZoneRow zone)
		{
			if (zone.typeName == null)
			{
				return this.session.Search.Matches(zone.labelId);
			}
			return this.session.Search.Matches(zone.typeName);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x0017F290 File Offset: 0x0017D490
		private void ExpandSearchMatches()
		{
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				ProfilerAllocTable.TypeRow typeRow = this.aggregated[i];
				for (int j = 0; j < typeRow.zones.Count; j++)
				{
					if (this.ZoneMatches(typeRow.zones[j]))
					{
						this.expandedTypes.Add(typeRow.name);
						break;
					}
				}
			}
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0017F300 File Offset: 0x0017D500
		public override void ExpandAll()
		{
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				if (this.aggregated[i].zones.Count > 0)
				{
					this.expandedTypes.Add(this.aggregated[i].name);
				}
			}
			this.SyncRows();
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x0017F35F File Offset: 0x0017D55F
		public override void CollapseAll()
		{
			this.expandedTypes.Clear();
			this.SyncRows();
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0017F374 File Offset: 0x0017D574
		protected override void ToggleExpansionAt(int index)
		{
			if (index < 0 || index >= this.displayRows.Count)
			{
				return;
			}
			ValueTuple<int, int> valueTuple = this.displayRows[index];
			int item = valueTuple.Item1;
			if (valueTuple.Item2 >= 0)
			{
				return;
			}
			this.View.Select(index);
			string name = this.aggregated[item].name;
			if (!this.expandedTypes.Add(name))
			{
				this.expandedTypes.Remove(name);
			}
			this.SyncRows();
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0017F3EF File Offset: 0x0017D5EF
		private ProfilerAllocTable.TypeRow RentTypeRow(string name)
		{
			ProfilerAllocTable.TypeRow typeRow = (this.typeRowPool.Count > 0) ? this.typeRowPool.Pop() : new ProfilerAllocTable.TypeRow();
			typeRow.name = name;
			typeRow.bytes = 0L;
			typeRow.count = 0L;
			return typeRow;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0017F428 File Offset: 0x0017D628
		private ProfilerAllocTable.ZoneRow RentZoneRow(ProfilerLabelId labelId)
		{
			ProfilerAllocTable.ZoneRow zoneRow = (this.zoneRowPool.Count > 0) ? this.zoneRowPool.Pop() : new ProfilerAllocTable.ZoneRow();
			zoneRow.labelId = labelId;
			zoneRow.typeName = null;
			zoneRow.bytes = 0L;
			zoneRow.count = 0L;
			return zoneRow;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x0017F468 File Offset: 0x0017D668
		private void ReleaseTypeRow(ProfilerAllocTable.TypeRow row)
		{
			for (int i = 0; i < row.zones.Count; i++)
			{
				this.zoneRowPool.Push(row.zones[i]);
			}
			row.zones.Clear();
			this.typeRowPool.Push(row);
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0017F4BC File Offset: 0x0017D6BC
		private void Aggregate()
		{
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				this.ReleaseTypeRow(this.aggregated[i]);
			}
			this.aggregated.Clear();
			this.typeLookup.Clear();
			this.rowLookup.Clear();
			this.zoneLookup.Clear();
			this.overflowCount = 0L;
			this.overflowBytes = 0L;
			this.untrackedBytes = 0L;
			bool flag = !ProfilerThreadRegistry.AllSelected;
			for (int j = 0; j < this.frames.Count; j++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot = this.frames[j];
				if (frameSnapshot.allocationTracked)
				{
					this.overflowCount += (long)frameSnapshot.allocOverflowCount;
					this.overflowBytes += frameSnapshot.allocOverflowBytes;
					for (int k = 0; k < frameSnapshot.topAllocTypeCount; k++)
					{
						FrameProfiler.TypeAllocStat typeAllocStat = frameSnapshot.topAllocTypes[k];
						if (!flag || ProfilerThreadRegistry.IsSelected(typeAllocStat.threadId))
						{
							int count;
							if (!this.typeLookup.TryGetValue(typeAllocStat.type, out count))
							{
								count = this.aggregated.Count;
								this.typeLookup.Add(typeAllocStat.type, count);
								this.aggregated.Add(this.RentTypeRow(ProfilerTypeNames.Get(typeAllocStat.type)));
							}
							ProfilerAllocTable.TypeRow typeRow = this.aggregated[count];
							typeRow.bytes += typeAllocStat.bytes;
							typeRow.count += (long)typeAllocStat.count;
							int count2;
							if (!this.zoneLookup.TryGetValue(new ValueTuple<int, ProfilerLabelId>(count, typeAllocStat.labelId), out count2))
							{
								count2 = typeRow.zones.Count;
								this.zoneLookup.Add(new ValueTuple<int, ProfilerLabelId>(count, typeAllocStat.labelId), count2);
								typeRow.zones.Add(this.RentZoneRow(typeAllocStat.labelId));
							}
							ProfilerAllocTable.ZoneRow zoneRow = typeRow.zones[count2];
							zoneRow.bytes += typeAllocStat.bytes;
							zoneRow.count += (long)typeAllocStat.count;
						}
					}
					this.untrackedBytes += frameSnapshot.mainThreadAllocatedBytes - frameSnapshot.mainThreadTrackedAllocBytes;
				}
			}
			this.untrackedBytes = Math.Max(0L, this.untrackedBytes);
			if (this.frames.Count > 1)
			{
				this.overflowCount /= (long)this.frames.Count;
				this.overflowBytes /= (long)this.frames.Count;
				this.untrackedBytes /= (long)this.frames.Count;
				for (int l = 0; l < this.aggregated.Count; l++)
				{
					ProfilerAllocTable.TypeRow typeRow2 = this.aggregated[l];
					typeRow2.bytes /= (long)this.frames.Count;
					typeRow2.count /= (long)this.frames.Count;
					for (int m = 0; m < typeRow2.zones.Count; m++)
					{
						typeRow2.zones[m].bytes /= (long)this.frames.Count;
						typeRow2.zones[m].count /= (long)this.frames.Count;
					}
				}
			}
			for (int n = this.aggregated.Count - 1; n >= 0; n--)
			{
				ProfilerAllocTable.TypeRow typeRow3 = this.aggregated[n];
				if (typeRow3.bytes <= 0L && typeRow3.count <= 0L)
				{
					this.ReleaseTypeRow(typeRow3);
					this.aggregated.RemoveAt(n);
				}
				else
				{
					for (int num = typeRow3.zones.Count - 1; num >= 0; num--)
					{
						if (typeRow3.zones[num].bytes <= 0L && typeRow3.zones[num].count <= 0L)
						{
							this.zoneRowPool.Push(typeRow3.zones[num]);
							typeRow3.zones.RemoveAt(num);
						}
					}
					typeRow3.zones.Sort(ProfilerAllocTable.ZoneRowComparison);
				}
			}
			this.aggregated.Sort(ProfilerAllocTable.TypeRowComparison);
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0017F944 File Offset: 0x0017DB44
		private void InvertAggregation()
		{
			this.invertedScratch.Clear();
			this.rowLookup.Clear();
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				ProfilerAllocTable.TypeRow typeRow = this.aggregated[i];
				for (int j = 0; j < typeRow.zones.Count; j++)
				{
					ProfilerAllocTable.ZoneRow zoneRow = typeRow.zones[j];
					string label = FrameProfiler.GetLabel(zoneRow.labelId);
					int count;
					if (!this.rowLookup.TryGetValue(label, out count))
					{
						count = this.invertedScratch.Count;
						this.rowLookup.Add(label, count);
						this.invertedScratch.Add(this.RentTypeRow(label));
					}
					ProfilerAllocTable.TypeRow typeRow2 = this.invertedScratch[count];
					typeRow2.bytes += zoneRow.bytes;
					typeRow2.count += zoneRow.count;
					ProfilerAllocTable.ZoneRow zoneRow2 = this.RentZoneRow(zoneRow.labelId);
					zoneRow2.typeName = typeRow.name;
					zoneRow2.bytes = zoneRow.bytes;
					zoneRow2.count = zoneRow.count;
					typeRow2.zones.Add(zoneRow2);
				}
			}
			for (int k = 0; k < this.aggregated.Count; k++)
			{
				this.ReleaseTypeRow(this.aggregated[k]);
			}
			this.aggregated.Clear();
			this.aggregated.AddRange(this.invertedScratch);
			this.invertedScratch.Clear();
			for (int l = 0; l < this.aggregated.Count; l++)
			{
				this.aggregated[l].zones.Sort(ProfilerAllocTable.ZoneRowComparison);
			}
			this.aggregated.Sort(ProfilerAllocTable.TypeRowComparison);
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0017FB14 File Offset: 0x0017DD14
		private void SyncRows()
		{
			long totalBytes = this.TotalBytes();
			this.displayRows.Clear();
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				this.displayRows.Add(new ValueTuple<int, int>(i, -1));
				if (this.expandedTypes.Contains(this.aggregated[i].name))
				{
					for (int j = 0; j < this.aggregated[i].zones.Count; j++)
					{
						this.displayRows.Add(new ValueTuple<int, int>(i, j));
					}
				}
			}
			this.View.Resize(this.displayRows.Count);
			for (int k = 0; k < this.displayRows.Count; k++)
			{
				this.Shape(this.displayRows[k].Item1, this.displayRows[k].Item2, this.View.Rows[k], totalBytes);
			}
			this.View.Notify_RowsChanged();
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0017FC20 File Offset: 0x0017DE20
		private void Shape(int typeIndex, int zoneIndex, ProfilerRowData data, long totalBytes)
		{
			ProfilerAllocTable.TypeRow typeRow = this.aggregated[typeIndex];
			data.Reset();
			data.TotalIsBytes = true;
			data.CallsGrouped = true;
			if (zoneIndex < 0)
			{
				data.BarFraction = Mathf.Clamp01((float)typeRow.bytes / (float)totalBytes);
				data.TotalBytes = typeRow.bytes;
				data.Calls = typeRow.count;
				data.Label = typeRow.name;
				data.HasChildren = (typeRow.zones.Count > 0);
				data.Expanded = this.expandedTypes.Contains(typeRow.name);
				data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(typeRow.name));
				return;
			}
			ProfilerAllocTable.ZoneRow zoneRow = typeRow.zones[zoneIndex];
			data.BarFraction = Mathf.Clamp01((float)zoneRow.bytes / (float)totalBytes);
			data.TotalBytes = zoneRow.bytes;
			data.Calls = zoneRow.count;
			data.Label = (zoneRow.typeName ?? FrameProfiler.GetLabel(zoneRow.labelId));
			data.Depth = 1;
			data.Dimmed = (this.session.Search.IsActive && !this.ZoneMatches(zoneRow));
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0017FD74 File Offset: 0x0017DF74
		protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
		{
			if (index >= this.displayRows.Count)
			{
				return false;
			}
			ValueTuple<int, int> valueTuple = this.displayRows[index];
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			long totalBytes = this.TotalBytes();
			int num = item2;
			int num2 = (item2 < 0) ? (this.aggregated[item].zones.Count - 1) : item2;
			for (int i = num; i <= num2; i++)
			{
				ProfilerRowData profilerRowData = new ProfilerRowData();
				this.Shape(item, i, profilerRowData, totalBytes);
				into.Add(profilerRowData);
			}
			return true;
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0017FE00 File Offset: 0x0017E000
		private long TotalBytes()
		{
			long num = 0L;
			for (int i = 0; i < this.aggregated.Count; i++)
			{
				num += this.aggregated[i].bytes;
			}
			return Math.Max(1L, num);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x0017FE44 File Offset: 0x0017E044
		public override void Draw(Rect rect)
		{
			if (this.aggregated.Count > 0)
			{
				base.Draw(rect);
				return;
			}
			if (FrameProfiler.AllocationTracking)
			{
				ProfilerGui.Label(rect, "No tracked allocations in the selected frames.", ProfilerGui.TextFaint, TextAnchor.MiddleCenter, GameFont.Tiny);
				return;
			}
			string text = FrameProfiler.AllocationTrackingSupported ? "Allocation tracking is off." : "Allocation tracking is not supported by this runtime.";
			ProfilerGui.Label(new Rect(rect.x, rect.center.y - 24f, rect.width, 20f), text, ProfilerGui.TextFaint, TextAnchor.MiddleCenter, GameFont.Tiny);
			if (FrameProfiler.AllocationTrackingSupported && ProfilerGui.Chip(new Rect(rect.center.x - 40f, rect.center.y + 2f, 80f, 22f), "Enable it", false, true, null))
			{
				Action enableTrackingRequested = this.EnableTrackingRequested;
				if (enableTrackingRequested == null)
				{
					return;
				}
				enableTrackingRequested();
			}
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x0017FF30 File Offset: 0x0017E130
		protected override void DrawFooter(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
			ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f, rect.height - 1f), this.footer, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x04002774 RID: 10100
		private static readonly Comparison<ProfilerAllocTable.TypeRow> TypeRowComparison = (ProfilerAllocTable.TypeRow lhs, ProfilerAllocTable.TypeRow rhs) => rhs.bytes.CompareTo(lhs.bytes);

		// Token: 0x04002775 RID: 10101
		private static readonly Comparison<ProfilerAllocTable.ZoneRow> ZoneRowComparison = (ProfilerAllocTable.ZoneRow lhs, ProfilerAllocTable.ZoneRow rhs) => rhs.bytes.CompareTo(lhs.bytes);

		// Token: 0x04002776 RID: 10102
		private readonly List<FrameProfiler.FrameSnapshot> frames = new List<FrameProfiler.FrameSnapshot>();

		// Token: 0x04002777 RID: 10103
		private readonly Dictionary<Type, int> typeLookup = new Dictionary<Type, int>();

		// Token: 0x04002778 RID: 10104
		private readonly Dictionary<string, int> rowLookup = new Dictionary<string, int>();

		// Token: 0x04002779 RID: 10105
		[TupleElementNames(new string[]
		{
			"typeIndex",
			"labelId"
		})]
		private readonly Dictionary<ValueTuple<int, ProfilerLabelId>, int> zoneLookup = new Dictionary<ValueTuple<int, ProfilerLabelId>, int>();

		// Token: 0x0400277A RID: 10106
		private readonly List<ProfilerAllocTable.TypeRow> aggregated = new List<ProfilerAllocTable.TypeRow>();

		// Token: 0x0400277B RID: 10107
		private readonly List<ProfilerAllocTable.TypeRow> invertedScratch = new List<ProfilerAllocTable.TypeRow>();

		// Token: 0x0400277C RID: 10108
		private readonly HashSet<string> expandedTypes = new HashSet<string>();

		// Token: 0x0400277D RID: 10109
		[TupleElementNames(new string[]
		{
			"typeIndex",
			"zoneIndex"
		})]
		private readonly List<ValueTuple<int, int>> displayRows = new List<ValueTuple<int, int>>();

		// Token: 0x0400277E RID: 10110
		private readonly Stack<ProfilerAllocTable.TypeRow> typeRowPool = new Stack<ProfilerAllocTable.TypeRow>();

		// Token: 0x0400277F RID: 10111
		private readonly Stack<ProfilerAllocTable.ZoneRow> zoneRowPool = new Stack<ProfilerAllocTable.ZoneRow>();

		// Token: 0x04002780 RID: 10112
		private ProfilerRefreshGate refreshGate;

		// Token: 0x04002781 RID: 10113
		private bool lastTracking;

		// Token: 0x04002782 RID: 10114
		private bool expandMatchesOnRefresh;

		// Token: 0x04002783 RID: 10115
		private bool inverted;

		// Token: 0x04002784 RID: 10116
		private long overflowCount;

		// Token: 0x04002785 RID: 10117
		private long overflowBytes;

		// Token: 0x04002786 RID: 10118
		private long untrackedBytes;

		// Token: 0x04002787 RID: 10119
		private string footer = "";

		// Token: 0x04002788 RID: 10120
		public Action<ProfilerLabelId> ZoneJumpRequested;

		// Token: 0x04002789 RID: 10121
		public Action EnableTrackingRequested;

		// Token: 0x02002A93 RID: 10899
		private sealed class ZoneRow
		{
			// Token: 0x0400AE65 RID: 44645
			public ProfilerLabelId labelId;

			// Token: 0x0400AE66 RID: 44646
			public string typeName;

			// Token: 0x0400AE67 RID: 44647
			public long bytes;

			// Token: 0x0400AE68 RID: 44648
			public long count;
		}

		// Token: 0x02002A94 RID: 10900
		private sealed class TypeRow
		{
			// Token: 0x0400AE69 RID: 44649
			public string name;

			// Token: 0x0400AE6A RID: 44650
			public long bytes;

			// Token: 0x0400AE6B RID: 44651
			public long count;

			// Token: 0x0400AE6C RID: 44652
			public readonly List<ProfilerAllocTable.ZoneRow> zones = new List<ProfilerAllocTable.ZoneRow>();
		}
	}
}
