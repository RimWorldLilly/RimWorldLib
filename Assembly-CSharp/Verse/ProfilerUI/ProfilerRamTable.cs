using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using RimWorld;
using UnityEngine;
using Waylith.Diagnostics;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000797 RID: 1943
	public sealed class ProfilerRamTable : ProfilerRowSection
	{
		// Token: 0x06003DDA RID: 15834 RVA: 0x00189B9C File Offset: 0x00187D9C
		public ProfilerRamTable(ProfilerSession session) : base(session)
		{
			this.View.LabelHeader = (() => "bucket");
			this.View.DetailHeader = "detail";
			this.View.RootCrumb = (() => "RAM");
			ProfilerRowListView view = this.View;
			ProfilerColumn[] array = new ProfilerColumn[3];
			array[0] = new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%", null);
			array[1] = new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => "bytes", null);
			array[2] = new ProfilerColumn(ProfilerColumnKind.Calls, 64f, () => "count", null);
			view.Columns = array;
			this.expandedSections.Add("Process");
			this.expandedSections.Add("Managed heap (GC)");
			this.expandedSections.Add("Native heap (malloc)");
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x00189D0A File Offset: 0x00187F0A
		protected override string CopyHeader
		{
			get
			{
				return "%\tbytes\tcount\tbucket\tdetail";
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x00189D11 File Offset: 0x00187F11
		protected override float FooterHeight
		{
			get
			{
				return 22f;
			}
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00189D18 File Offset: 0x00187F18
		public override void Invalidate()
		{
			this.captureRequested = true;
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x00189D21 File Offset: 0x00187F21
		public override void Notify_SearchChanged()
		{
			this.captureRequested = true;
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00189D2C File Offset: 0x00187F2C
		public override void Refresh()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (!this.captureRequested && timestamp - this.lastCaptureTimestamp < ProfilerRamTable.CaptureIntervalTicks)
			{
				return;
			}
			this.lastCaptureTimestamp = timestamp;
			this.captureRequested = false;
			this.snapshot = ProcessMemoryReport.Capture();
			this.SyncRows();
			this.WriteFooter();
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00189D7C File Offset: 0x00187F7C
		private void SyncRows()
		{
			this.entries.Clear();
			if (this.snapshot == null)
			{
				return;
			}
			foreach (MemoryReportSection memoryReportSection in this.snapshot.Sections)
			{
				List<ProfilerRamTable.Entry> list = this.entries;
				ProfilerRamTable.Entry item = new ProfilerRamTable.Entry
				{
					section = memoryReportSection,
					item = -1
				};
				list.Add(item);
				if (this.expandedSections.Contains(memoryReportSection.Name))
				{
					for (int i = 0; i < memoryReportSection.Items.Count; i++)
					{
						List<ProfilerRamTable.Entry> list2 = this.entries;
						item = new ProfilerRamTable.Entry
						{
							section = memoryReportSection,
							item = i
						};
						list2.Add(item);
					}
				}
			}
			this.View.Resize(this.entries.Count);
			for (int j = 0; j < this.entries.Count; j++)
			{
				ProfilerRamTable.Entry item = this.entries[j];
				this.Shape(item, this.View.Rows[j]);
			}
			this.View.Notify_RowsChanged();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00189EB8 File Offset: 0x001880B8
		protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
		{
			if (index >= this.entries.Count)
			{
				return false;
			}
			ProfilerRamTable.Entry entry = this.entries[index];
			ProfilerRowData profilerRowData = new ProfilerRowData();
			this.Shape(entry, profilerRowData);
			into.Add(profilerRowData);
			if (entry.item >= 0)
			{
				return true;
			}
			for (int i = 0; i < entry.section.Items.Count; i++)
			{
				ProfilerRowData profilerRowData2 = new ProfilerRowData();
				ProfilerRamTable.Entry entry2 = default(ProfilerRamTable.Entry);
				entry2.section = entry.section;
				entry2.item = i;
				this.Shape(entry2, profilerRowData2);
				into.Add(profilerRowData2);
			}
			return true;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00189F54 File Offset: 0x00188154
		private void Shape(in ProfilerRamTable.Entry entry, ProfilerRowData data)
		{
			data.Reset();
			data.TotalIsBytes = true;
			data.CallsGrouped = true;
			long num = Math.Max(1L, this.snapshot.ResidentBytes);
			if (entry.item < 0)
			{
				MemoryReportSection section = entry.section;
				data.Depth = 0;
				data.TotalBytes = section.Bytes;
				data.Calls = section.Count;
				data.Label = section.Name;
				data.Detail = (section.Detail ?? "");
				data.HasChildren = (section.Items.Count > 0);
				data.Expanded = this.expandedSections.Contains(section.Name);
				data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(section.Name));
			}
			else
			{
				MemoryReportItem memoryReportItem = entry.section.Items[entry.item];
				data.Depth = 1;
				data.TotalBytes = memoryReportItem.Bytes;
				data.Calls = memoryReportItem.Count;
				data.Label = memoryReportItem.Name;
				data.Detail = (memoryReportItem.Detail ?? "");
				data.Dimmed = (this.session.Search.IsActive && !this.session.Search.Matches(memoryReportItem.Name));
			}
			data.BarFraction = Mathf.Clamp01((float)data.TotalBytes / (float)num);
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0018A0E4 File Offset: 0x001882E4
		protected override void ToggleExpansionAt(int index)
		{
			if (index < 0 || index >= this.entries.Count)
			{
				return;
			}
			ProfilerRamTable.Entry entry = this.entries[index];
			if (entry.item >= 0 || entry.section.Items.Count == 0)
			{
				return;
			}
			this.View.Select(index);
			if (!this.expandedSections.Add(entry.section.Name))
			{
				this.expandedSections.Remove(entry.section.Name);
			}
			this.SyncRows();
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0018A170 File Offset: 0x00188370
		public override void ExpandAll()
		{
			if (this.snapshot == null)
			{
				return;
			}
			foreach (MemoryReportSection memoryReportSection in this.snapshot.Sections)
			{
				this.expandedSections.Add(memoryReportSection.Name);
			}
			this.SyncRows();
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0018A1E4 File Offset: 0x001883E4
		public override void CollapseAll()
		{
			this.expandedSections.Clear();
			this.SyncRows();
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0018A1F8 File Offset: 0x001883F8
		private void WriteFooter()
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append("resident ").Append(VramProfiler.FormatBytes(this.snapshot.ResidentBytes));
			if (this.snapshot.HasMappingDetail)
			{
				scratch.Append(" / anonymous ").Append(VramProfiler.FormatBytes(this.snapshot.AnonymousBytes)).Append(" / file backed ").Append(VramProfiler.FormatBytes(this.snapshot.FileBackedBytes));
			}
			else
			{
				scratch.Append(" / private ").Append(VramProfiler.FormatBytes(this.snapshot.PrivateBytes));
			}
			this.footer = scratch.ToString();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0018A2AF File Offset: 0x001884AF
		protected override void AppendCopyContext(StringBuilder builder)
		{
			builder.AppendLine(this.footer);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0018A2C0 File Offset: 0x001884C0
		protected override void DrawFooter(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
			ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f, rect.height - 1f), this.footer, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0018A344 File Offset: 0x00188544
		public void Export()
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, "MemoryReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
			File.WriteAllText(text, ProcessMemoryReport.BuildReport(null));
			Messages.Message("Memory report written to " + text, MessageTypeDefOf.SilentInput, false);
		}

		// Token: 0x040028A7 RID: 10407
		private static readonly long CaptureIntervalTicks = Stopwatch.Frequency * 2L;

		// Token: 0x040028A8 RID: 10408
		private readonly HashSet<string> expandedSections = new HashSet<string>();

		// Token: 0x040028A9 RID: 10409
		private readonly List<ProfilerRamTable.Entry> entries = new List<ProfilerRamTable.Entry>();

		// Token: 0x040028AA RID: 10410
		private ProcessMemorySnapshot snapshot;

		// Token: 0x040028AB RID: 10411
		private long lastCaptureTimestamp;

		// Token: 0x040028AC RID: 10412
		private bool captureRequested = true;

		// Token: 0x040028AD RID: 10413
		private string footer = "";

		// Token: 0x02002AA0 RID: 10912
		private struct Entry
		{
			// Token: 0x0400AEA6 RID: 44710
			public MemoryReportSection section;

			// Token: 0x0400AEA7 RID: 44711
			public int item;
		}
	}
}
