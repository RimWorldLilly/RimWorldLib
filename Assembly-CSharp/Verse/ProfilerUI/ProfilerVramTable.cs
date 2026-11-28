using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RimWorld;
using UnityEngine;
using Waylith;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A6 RID: 1958
	public sealed class ProfilerVramTable : ProfilerRowSection
	{
		// Token: 0x06003EF1 RID: 16113 RVA: 0x00190E84 File Offset: 0x0018F084
		public ProfilerVramTable(ProfilerSession session) : base(session)
		{
			this.View.LabelHeader = (() => "label");
			this.View.DetailHeader = "detail";
			this.View.RootCrumb = (() => "GPU");
			this.BuildColumns();
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00190F38 File Offset: 0x0018F138
		private void BuildColumns()
		{
			this.lastHadBaseline = this.HasBaseline;
			List<ProfilerColumn> list = new List<ProfilerColumn>();
			list.Add(new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%", null));
			list.Add(new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => "bytes", null));
			List<ProfilerColumn> list2 = list;
			if (this.HasBaseline)
			{
				list2.Add(new ProfilerColumn(ProfilerColumnKind.Delta, 90f, () => "Δ base", null));
			}
			list2.Add(new ProfilerColumn(ProfilerColumnKind.Calls, 64f, () => "allocs", null));
			this.View.Columns = list2.ToArray();
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x00191034 File Offset: 0x0018F234
		public bool HasBaseline
		{
			get
			{
				return VramProfiler.HasBaseline;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06003EF4 RID: 16116 RVA: 0x0019103B File Offset: 0x0018F23B
		// (set) Token: 0x06003EF5 RID: 16117 RVA: 0x00191043 File Offset: 0x0018F243
		public bool SortByDelta
		{
			get
			{
				return this.sortByDelta;
			}
			set
			{
				this.sortByDelta = value;
				this.captureRequested = true;
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x00191053 File Offset: 0x0018F253
		protected override string CopyHeader
		{
			get
			{
				if (!this.HasBaseline)
				{
					return "%\tbytes\tallocs\tlabel\tdetail";
				}
				return "%\tbytes\tΔ\tallocs\tlabel\tdetail";
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06003EF7 RID: 16119 RVA: 0x00191068 File Offset: 0x0018F268
		protected override float FooterHeight
		{
			get
			{
				return 22f;
			}
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0019106F File Offset: 0x0018F26F
		public override void Invalidate()
		{
			this.captureRequested = true;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00191078 File Offset: 0x0018F278
		public void CaptureBaseline()
		{
			VramProfiler.CaptureBaseline();
			this.captureRequested = true;
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00191086 File Offset: 0x0018F286
		public void ClearBaseline()
		{
			VramProfiler.ClearBaseline();
			this.captureRequested = true;
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00191094 File Offset: 0x0018F294
		public override void Notify_SearchChanged()
		{
			this.expandMatchesOnRefresh = true;
			this.captureRequested = true;
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x001910A4 File Offset: 0x0018F2A4
		public override void Refresh()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (!this.captureRequested && timestamp - this.lastCaptureTimestamp < ProfilerVramTable.CaptureIntervalTicks)
			{
				return;
			}
			this.lastCaptureTimestamp = timestamp;
			this.captureRequested = false;
			if (this.lastHadBaseline != this.HasBaseline)
			{
				this.BuildColumns();
			}
			VramProfiler.Capture(this.sortByDelta && this.HasBaseline);
			if (this.expandMatchesOnRefresh)
			{
				this.expandMatchesOnRefresh = false;
				if (this.session.Search.IsActive)
				{
					this.ExpandSearchMatches(VramProfiler.Root);
				}
			}
			this.SyncRows();
			this.WriteFooter();
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00191140 File Offset: 0x0018F340
		private bool ExpandSearchMatches(VramProfiler.Node node)
		{
			bool flag = false;
			for (int i = 0; i < node.Children.Count; i++)
			{
				if (this.ExpandSearchMatches(node.Children[i]))
				{
					flag = true;
				}
			}
			if (flag && node.FullPath.Length > 0)
			{
				this.expandedPaths.Add(node.FullPath);
			}
			return flag || this.session.Search.Matches(node.Segment);
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x001911B9 File Offset: 0x0018F3B9
		private static string SelfKey(VramProfiler.Node node)
		{
			return node.FullPath + "\n(self)";
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x001911CC File Offset: 0x0018F3CC
		private void SyncRows()
		{
			this.entries.Clear();
			this.AppendChildren(this.entries, VramProfiler.Root, 0, false);
			this.View.Resize(this.entries.Count);
			long totalBytes = Math.Max(1L, VramProfiler.Root.TotalBytes);
			for (int i = 0; i < this.entries.Count; i++)
			{
				ProfilerVramTable.Entry entry = this.entries[i];
				this.Shape(entry, this.View.Rows[i], totalBytes);
			}
			this.View.Notify_RowsChanged();
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00191268 File Offset: 0x0018F468
		private void AppendChildren(List<ProfilerVramTable.Entry> into, VramProfiler.Node parent, int depth, bool allGroups)
		{
			for (int i = 0; i < parent.Children.Count; i++)
			{
				this.AppendNode(into, parent.Children[i], depth, allGroups);
			}
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x001912A4 File Offset: 0x0018F4A4
		private void AppendNode(List<ProfilerVramTable.Entry> into, VramProfiler.Node node, int depth, bool allGroups)
		{
			into.Add(new ProfilerVramTable.Entry
			{
				node = node,
				depth = depth,
				kind = ProfilerVramTable.EntryKind.Node
			});
			if (!node.HasChildren)
			{
				if (this.expandedPaths.Contains(node.FullPath))
				{
					this.AppendIndividuals(into, node, depth + 1);
				}
				return;
			}
			if (!allGroups && !this.expandedPaths.Contains(node.FullPath))
			{
				return;
			}
			if (node.DirectBytes > 0L || node.DirectBaselineBytes > 0L)
			{
				into.Add(new ProfilerVramTable.Entry
				{
					node = node,
					depth = depth + 1,
					kind = ProfilerVramTable.EntryKind.Self
				});
				if (this.expandedPaths.Contains(ProfilerVramTable.SelfKey(node)))
				{
					this.AppendIndividuals(into, node, depth + 2);
				}
			}
			this.AppendChildren(into, node, depth + 1, allGroups);
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x00191384 File Offset: 0x0018F584
		protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
		{
			if (index >= this.entries.Count)
			{
				return false;
			}
			ProfilerVramTable.Entry entry = this.entries[index];
			List<ProfilerVramTable.Entry> list = new List<ProfilerVramTable.Entry>();
			if (entry.kind == ProfilerVramTable.EntryKind.Node && entry.node.HasChildren)
			{
				this.AppendNode(list, entry.node, entry.depth, true);
			}
			else
			{
				list.Add(entry);
				if (entry.kind == ProfilerVramTable.EntryKind.Self || (entry.kind == ProfilerVramTable.EntryKind.Node && entry.node.DirectCount > 0L))
				{
					this.AppendIndividuals(list, entry.node, entry.depth + 1);
				}
			}
			long totalBytes = Math.Max(1L, VramProfiler.Root.TotalBytes);
			for (int i = 0; i < list.Count; i++)
			{
				ProfilerRowData profilerRowData = new ProfilerRowData();
				ProfilerVramTable.Entry entry2 = list[i];
				this.Shape(entry2, profilerRowData, totalBytes);
				into.Add(profilerRowData);
			}
			return true;
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00191464 File Offset: 0x0018F664
		private void AppendIndividuals(List<ProfilerVramTable.Entry> into, VramProfiler.Node node, int depth)
		{
			VramProfiler.CollectIndividuals(node.FullPath, this.sortByDelta && this.HasBaseline, this.individualsScratch);
			int num = Math.Min(this.individualsScratch.Count, 200);
			ProfilerVramTable.Entry item;
			for (int i = 0; i < num; i++)
			{
				VramProfiler.IndividualAllocation individualAllocation = this.individualsScratch[i];
				item = new ProfilerVramTable.Entry
				{
					node = node,
					depth = depth,
					kind = ProfilerVramTable.EntryKind.Individual,
					name = (individualAllocation.Freed ? (individualAllocation.Name + " (freed)") : individualAllocation.Name),
					bytes = individualAllocation.Bytes,
					deltaBytes = individualAllocation.DeltaBytes,
					detail = individualAllocation.Detail
				};
				into.Add(item);
			}
			if (this.individualsScratch.Count <= num)
			{
				return;
			}
			long num2 = 0L;
			long num3 = 0L;
			for (int j = num; j < this.individualsScratch.Count; j++)
			{
				num2 += this.individualsScratch[j].Bytes;
				num3 += this.individualsScratch[j].DeltaBytes;
			}
			item = new ProfilerVramTable.Entry
			{
				node = node,
				depth = depth,
				kind = ProfilerVramTable.EntryKind.Overflow,
				name = "… " + (this.individualsScratch.Count - num).ToString() + " more",
				bytes = num2,
				deltaBytes = num3
			};
			into.Add(item);
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0019160C File Offset: 0x0018F80C
		private void Shape(in ProfilerVramTable.Entry entry, ProfilerRowData data, long totalBytes)
		{
			VramProfiler.Node node = entry.node;
			data.Reset();
			data.TotalIsBytes = true;
			data.HasDelta = this.HasBaseline;
			data.DeltaIsBytes = true;
			data.CallsGrouped = true;
			data.Depth = entry.depth;
			ProfilerVramTable.EntryKind kind = entry.kind;
			if (kind != ProfilerVramTable.EntryKind.Self)
			{
				if (kind - ProfilerVramTable.EntryKind.Individual > 1)
				{
					bool flag = node.HasChildren || node.DirectCount > 0L;
					data.TotalBytes = node.TotalBytes;
					data.DeltaBytes = node.DeltaBytes;
					data.Calls = node.TotalCount;
					data.Label = node.Segment;
					data.HasChildren = flag;
					data.Expanded = (flag && this.expandedPaths.Contains(node.FullPath));
					data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(node.FullPath));
					data.Detail = (node.SoleDetail ?? "");
				}
				else
				{
					data.TotalBytes = entry.bytes;
					data.DeltaBytes = entry.deltaBytes;
					data.Label = entry.name;
					data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(entry.name));
					data.Detail = (entry.detail ?? "");
				}
			}
			else
			{
				data.TotalBytes = node.DirectBytes;
				data.DeltaBytes = node.DirectDeltaBytes;
				data.Calls = node.DirectCount;
				data.Label = "(self)";
				data.HasChildren = true;
				data.Expanded = this.expandedPaths.Contains(ProfilerVramTable.SelfKey(node));
				data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(node.FullPath));
				data.Detail = "";
			}
			data.BarFraction = Mathf.Clamp01((float)data.TotalBytes / (float)totalBytes);
			data.DeltaSeverity = Math.Sign(data.DeltaBytes);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00191848 File Offset: 0x0018FA48
		protected override void ToggleExpansionAt(int index)
		{
			if (index < 0 || index >= this.entries.Count)
			{
				return;
			}
			ProfilerVramTable.Entry entry = this.entries[index];
			string text = null;
			if (entry.kind == ProfilerVramTable.EntryKind.Node && (entry.node.HasChildren || entry.node.DirectCount > 0L))
			{
				text = entry.node.FullPath;
			}
			else if (entry.kind == ProfilerVramTable.EntryKind.Self)
			{
				text = ProfilerVramTable.SelfKey(entry.node);
			}
			if (text == null)
			{
				return;
			}
			this.View.Select(index);
			if (!this.expandedPaths.Add(text))
			{
				this.expandedPaths.Remove(text);
			}
			this.SyncRows();
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x001918EF File Offset: 0x0018FAEF
		public override void ExpandAll()
		{
			this.ExpandSubtree(VramProfiler.Root);
			this.SyncRows();
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00191904 File Offset: 0x0018FB04
		private void ExpandSubtree(VramProfiler.Node node)
		{
			for (int i = 0; i < node.Children.Count; i++)
			{
				VramProfiler.Node node2 = node.Children[i];
				if (node2.HasChildren)
				{
					this.expandedPaths.Add(node2.FullPath);
					this.ExpandSubtree(node2);
				}
			}
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00191955 File Offset: 0x0018FB55
		public override void CollapseAll()
		{
			this.expandedPaths.Clear();
			this.SyncRows();
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x00191968 File Offset: 0x0018FB68
		private void WriteFooter()
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append("tracked ").Append(VramProfiler.FormatBytes(VramProfiler.Root.TotalBytes));
			if (this.HasBaseline)
			{
				scratch.Append(" / Δ since baseline ");
				ProfilerVramTable.AppendSignedBytes(scratch, VramProfiler.Root.DeltaBytes);
			}
			GpuMemorySummary gpuMemorySummary;
			if (GpuMemoryReport.TryGetSummary(out gpuMemorySummary))
			{
				scratch.Append(gpuMemorySummary.DriverReported ? " / driver " : " / VMA est. ").Append(VramProfiler.FormatBytes(gpuMemorySummary.DeviceLocalDriverUsageBytes)).Append(" used / ").Append(VramProfiler.FormatBytes(gpuMemorySummary.DeviceLocalBudgetBytes)).Append(" budget").Append(" / sysmem ").Append(VramProfiler.FormatBytes(gpuMemorySummary.HostVmaAllocationBytes));
			}
			this.footer = scratch.ToString();
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00191A48 File Offset: 0x0018FC48
		private static void AppendSignedBytes(StringBuilder builder, long bytes)
		{
			if (bytes == 0L)
			{
				builder.Append('0');
				return;
			}
			builder.Append((bytes > 0L) ? '+' : '-').Append(VramProfiler.FormatBytes(Math.Abs(bytes)));
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x00191A79 File Offset: 0x0018FC79
		protected override void AppendCopyContext(StringBuilder builder)
		{
			builder.AppendLine(this.footer);
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x00191A88 File Offset: 0x0018FC88
		protected override void DrawFooter(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
			ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f, rect.height - 1f), this.footer, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00191B0C File Offset: 0x0018FD0C
		public void Export()
		{
			VramProfiler.Capture(this.sortByDelta && this.HasBaseline);
			StringBuilder stringBuilder = new StringBuilder(16384);
			stringBuilder.Append("VRAM report ").Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).AppendLine();
			stringBuilder.AppendLine(GpuMemoryReport.BuildCompactSummary());
			GpuMemoryHistory.AppendGrowthSummary(stringBuilder, 12, 1048576L);
			stringBuilder.Append("Frame rate cap: ").AppendLine(EngineApplication.FrameRateCapDescription);
			stringBuilder.Append("Foreign modules: ").AppendLine(string.Join(", ", ProcessModules.ForeignModuleLines()));
			stringBuilder.AppendLine();
			if (this.HasBaseline)
			{
				stringBuilder.Append("Δ since baseline: ");
				ProfilerVramTable.AppendSignedBytes(stringBuilder, VramProfiler.Root.DeltaBytes);
				stringBuilder.AppendLine().AppendLine();
			}
			StringBuilder stringBuilder2 = new StringBuilder(16384);
			stringBuilder2.AppendLine(this.CopyHeader);
			this.AppendExportRows(stringBuilder2, VramProfiler.Root, 0);
			stringBuilder.Append(ProfilerClipboard.AlignColumns(stringBuilder2.ToString()));
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, "VramReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
			File.WriteAllText(text, stringBuilder.ToString());
			Messages.Message("VRAM report written to " + text, MessageTypeDefOf.SilentInput, false);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x00191C70 File Offset: 0x0018FE70
		private void AppendExportRows(StringBuilder builder, VramProfiler.Node parent, int depth)
		{
			long num = Math.Max(1L, VramProfiler.Root.TotalBytes);
			for (int i = 0; i < parent.Children.Count; i++)
			{
				VramProfiler.Node node = parent.Children[i];
				builder.Append(((float)node.TotalBytes * 100f / (float)num).ToString("0.0")).Append('\t');
				builder.Append(VramProfiler.FormatBytes(node.TotalBytes)).Append('\t');
				if (this.HasBaseline)
				{
					ProfilerVramTable.AppendSignedBytes(builder, node.DeltaBytes);
					builder.Append('\t');
				}
				builder.Append(node.TotalCount).Append('\t');
				for (int j = 0; j < depth; j++)
				{
					builder.Append("  ");
				}
				builder.Append(node.Segment).Append('\t').Append(node.SoleDetail).AppendLine();
				this.AppendExportIndividuals(builder, node, depth + 1);
				this.AppendExportRows(builder, node, depth + 1);
			}
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x00191D84 File Offset: 0x0018FF84
		private void AppendExportIndividuals(StringBuilder builder, VramProfiler.Node node, int depth)
		{
			if (!this.HasBaseline || node.DirectCount == 0L || Math.Abs(node.DirectDeltaBytes) < 262144L)
			{
				return;
			}
			this.individualsScratch.Clear();
			VramProfiler.CollectIndividuals(node.FullPath, true, this.individualsScratch);
			int num = 0;
			int num2 = 0;
			while (num2 < this.individualsScratch.Count && num < 48)
			{
				VramProfiler.IndividualAllocation individualAllocation = this.individualsScratch[num2];
				if (individualAllocation.DeltaBytes != 0L)
				{
					builder.Append('\t');
					builder.Append(individualAllocation.Freed ? "freed" : VramProfiler.FormatBytes(individualAllocation.Bytes)).Append('\t');
					ProfilerVramTable.AppendSignedBytes(builder, individualAllocation.DeltaBytes);
					builder.Append('\t').Append('\t');
					for (int i = 0; i < depth; i++)
					{
						builder.Append("  ");
					}
					builder.Append(individualAllocation.Name).Append('\t').Append(individualAllocation.Detail).AppendLine();
					num++;
				}
				num2++;
			}
		}

		// Token: 0x0400298B RID: 10635
		private const int MaxIndividualRows = 200;

		// Token: 0x0400298C RID: 10636
		private const long ExportIndividualsMinDeltaBytes = 262144L;

		// Token: 0x0400298D RID: 10637
		private const int ExportIndividualsCap = 48;

		// Token: 0x0400298E RID: 10638
		private static readonly long CaptureIntervalTicks = Stopwatch.Frequency / 2L;

		// Token: 0x0400298F RID: 10639
		private readonly HashSet<string> expandedPaths = new HashSet<string>();

		// Token: 0x04002990 RID: 10640
		private readonly List<ProfilerVramTable.Entry> entries = new List<ProfilerVramTable.Entry>();

		// Token: 0x04002991 RID: 10641
		private readonly List<VramProfiler.IndividualAllocation> individualsScratch = new List<VramProfiler.IndividualAllocation>();

		// Token: 0x04002992 RID: 10642
		private long lastCaptureTimestamp;

		// Token: 0x04002993 RID: 10643
		private bool captureRequested = true;

		// Token: 0x04002994 RID: 10644
		private bool expandMatchesOnRefresh;

		// Token: 0x04002995 RID: 10645
		private bool sortByDelta;

		// Token: 0x04002996 RID: 10646
		private bool lastHadBaseline;

		// Token: 0x04002997 RID: 10647
		private string footer = "";

		// Token: 0x02002AAD RID: 10925
		private enum EntryKind : byte
		{
			// Token: 0x0400AEE6 RID: 44774
			Node,
			// Token: 0x0400AEE7 RID: 44775
			Self,
			// Token: 0x0400AEE8 RID: 44776
			Individual,
			// Token: 0x0400AEE9 RID: 44777
			Overflow
		}

		// Token: 0x02002AAE RID: 10926
		private struct Entry
		{
			// Token: 0x0400AEEA RID: 44778
			public VramProfiler.Node node;

			// Token: 0x0400AEEB RID: 44779
			public int depth;

			// Token: 0x0400AEEC RID: 44780
			public ProfilerVramTable.EntryKind kind;

			// Token: 0x0400AEED RID: 44781
			public string name;

			// Token: 0x0400AEEE RID: 44782
			public long bytes;

			// Token: 0x0400AEEF RID: 44783
			public long deltaBytes;

			// Token: 0x0400AEF0 RID: 44784
			public string detail;
		}
	}
}
