using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000789 RID: 1929
	public sealed class ProfilerCallTreeTable : ProfilerRowSection
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x001800C8 File Offset: 0x0017E2C8
		public ProfilerCallTreeTable(ProfilerSession session) : base(session)
		{
			ProfilerRowListView view = this.View;
			ProfilerColumn[] array = new ProfilerColumn[6];
			array[0] = new ProfilerColumn(ProfilerColumnKind.Percent, 44f, () => "%", null);
			array[1] = new ProfilerColumn(ProfilerColumnKind.Total, 90f, () => this.HeaderText("total", ProfilerTreeModel.SortColumn.Total), delegate()
			{
				this.SetSortColumn(ProfilerTreeModel.SortColumn.Total);
			});
			array[2] = new ProfilerColumn(ProfilerColumnKind.Self, 90f, () => this.HeaderText("self", ProfilerTreeModel.SortColumn.Self), delegate()
			{
				this.SetSortColumn(ProfilerTreeModel.SortColumn.Self);
			});
			array[3] = new ProfilerColumn(ProfilerColumnKind.Delta, 64f, () => "Δ median", null);
			array[4] = new ProfilerColumn(ProfilerColumnKind.Calls, 52f, () => this.HeaderText("calls", ProfilerTreeModel.SortColumn.Calls), delegate()
			{
				this.SetSortColumn(ProfilerTreeModel.SortColumn.Calls);
			});
			array[5] = new ProfilerColumn(ProfilerColumnKind.Alloc, 66f, () => this.HeaderText("alloc", ProfilerTreeModel.SortColumn.Alloc), delegate()
			{
				this.SetSortColumn(ProfilerTreeModel.SortColumn.Alloc);
			});
			view.Columns = array;
			this.View.LabelHeader = (() => this.HeaderText("scope", ProfilerTreeModel.SortColumn.Label));
			this.View.LabelSort = delegate()
			{
				this.SetSortColumn(ProfilerTreeModel.SortColumn.Label);
			};
			this.View.RootCrumb = new Func<string>(ProfilerCallTreeTable.RootCrumbLabel);
			this.View.SelectedByUser = new Action(this.OnSelectedByUser);
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x001802B6 File Offset: 0x0017E4B6
		private ProfilerTreeModel Tree
		{
			get
			{
				return this.session.Tree;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x001802C3 File Offset: 0x0017E4C3
		private List<ProfilerRowData> rows
		{
			get
			{
				return this.View.Rows;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x001802D0 File Offset: 0x0017E4D0
		private int selectedIndex
		{
			get
			{
				return this.View.SelectedIndex;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x001802DD File Offset: 0x0017E4DD
		protected override string CopyHeader
		{
			get
			{
				return "%\ttotal\tself\tcalls\talloc\tscope";
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06003CD6 RID: 15574 RVA: 0x001802E4 File Offset: 0x0017E4E4
		private ProfilerTreeModel.Row SelectedRow
		{
			get
			{
				if (this.selectedIndex < 0 || this.selectedIndex >= this.Tree.DisplayRows.Count)
				{
					return null;
				}
				return this.Tree.DisplayRows[this.selectedIndex];
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0018031F File Offset: 0x0017E51F
		public override void Invalidate()
		{
			this.refreshGate.Invalidate();
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0018032C File Offset: 0x0017E52C
		public void ResetSelection()
		{
			this.View.SelectQuietly(-1);
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0018033A File Offset: 0x0017E53A
		private void OnSelectedByUser()
		{
			this.revealInFlamegraph = !this.selectingFromFlamegraph;
			Action rowSelectedByUser = this.RowSelectedByUser;
			if (rowSelectedByUser == null)
			{
				return;
			}
			rowSelectedByUser();
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x0018035C File Offset: 0x0017E55C
		public override void Refresh()
		{
			bool flag = this.refreshGate.ShouldRefresh(this.session);
			bool flag2 = this.Tree.EmittedRequest != this.Tree.SubmittedRequest;
			if (!flag && !flag2)
			{
				return;
			}
			ProfilerTreeModel.Row selectedRow = this.SelectedRow;
			this.previousSelectionLabelId = ((selectedRow != null) ? new ProfilerLabelId?(selectedRow.labelId) : null);
			this.previousSelectionPathKey = ((selectedRow != null) ? selectedRow.pathKey : 0UL);
			if (this.FlamegraphMode)
			{
				this.Tree.BuildAllLevelRows();
				this.frameTotalMicroseconds = 0.0;
				List<ProfilerTreeModel.Row> displayRows = this.Tree.DisplayRows;
				for (int i = 0; i < displayRows.Count; i++)
				{
					if (displayRows[i].depth == 0)
					{
						this.frameTotalMicroseconds += (double)displayRows[i].totalMicroseconds;
					}
				}
			}
			else
			{
				this.session.CollectFrames(this.session.Span, this.frames);
				this.Tree.BuildTreeRows();
				float num = 0f;
				for (int j = 0; j < this.frames.Count; j++)
				{
					num += this.frames[j].totalMs;
				}
				this.frameTotalMicroseconds = (double)(num / (float)Mathf.Max(1, this.frames.Count) * 1000f);
			}
			if (this.frameTotalMicroseconds <= 0.0)
			{
				this.frameTotalMicroseconds = 1.0;
			}
			List<ProfilerTreeModel.Row> displayRows2 = this.Tree.DisplayRows;
			this.View.Resize(displayRows2.Count);
			for (int k = 0; k < displayRows2.Count; k++)
			{
				this.Shape(displayRows2[k], this.rows[k], this.frameTotalMicroseconds);
			}
			this.View.SelectQuietly(this.ResolveSelectionAfterRebuild());
			this.View.Notify_RowsChanged();
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0018055C File Offset: 0x0017E75C
		private void Shape(ProfilerTreeModel.Row row, ProfilerRowData data, double frameTotalMicroseconds)
		{
			float num = float.NaN;
			int deltaSeverity = 0;
			if (row.HasBaseline)
			{
				num = (row.totalMicroseconds - row.baselineTotalMicroseconds) / 1000f;
				deltaSeverity = ((num >= 0.05f) ? 1 : ((num <= -0.05f) ? -1 : 0));
			}
			data.Reset();
			data.BarFraction = Mathf.Clamp01((float)((double)row.totalMicroseconds / frameTotalMicroseconds));
			data.TotalMicroseconds = (double)row.totalMicroseconds;
			data.HasSelf = true;
			data.SelfMicroseconds = (double)row.selfMicroseconds;
			data.HasDelta = !this.FlamegraphMode;
			data.DeltaMs = num;
			data.DeltaSeverity = deltaSeverity;
			data.Calls = (long)row.calls;
			data.AllocBytes = row.allocBytes;
			data.Swatch = ProfilerPalette.ColorFor(row.labelId);
			data.Label = ProfilerTreeModel.LabelForRow(row);
			data.Depth = row.depth;
			data.HasChildren = (row.labelId.IsValid && row.hasChildren);
			data.Expanded = row.expanded;
			data.Dimmed = (this.session.Search.IsActive && !this.MatchesSearch(row));
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0018068D File Offset: 0x0017E88D
		private bool MatchesSearch(ProfilerTreeModel.Row row)
		{
			if (!row.labelId.IsValid)
			{
				return this.session.Search.Matches("Unprofiled");
			}
			return this.session.Search.Matches(row.labelId);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x001806C8 File Offset: 0x0017E8C8
		private int ResolveSelectionAfterRebuild()
		{
			List<ProfilerTreeModel.Row> displayRows = this.Tree.DisplayRows;
			if (this.pendingSelectKey != 0UL)
			{
				for (int i = 0; i < displayRows.Count; i++)
				{
					if (displayRows[i].pathKey == this.pendingSelectKey)
					{
						this.pendingSelectKey = 0UL;
						this.View.RequestReveal();
						this.revealInFlamegraph = true;
						return i;
					}
				}
				if (this.Tree.EmittedRequest == this.Tree.SubmittedRequest)
				{
					this.pendingSelectKey = 0UL;
				}
			}
			if (this.selectFirstMatchOnRebuild)
			{
				this.selectFirstMatchOnRebuild = false;
				if (this.session.Search.IsActive)
				{
					for (int j = 0; j < displayRows.Count; j++)
					{
						if (this.MatchesSearch(displayRows[j]))
						{
							this.View.RequestReveal();
							this.revealInFlamegraph = true;
							return j;
						}
					}
				}
			}
			ProfilerLabelId? profilerLabelId = this.previousSelectionLabelId;
			if (profilerLabelId != null)
			{
				ProfilerLabelId valueOrDefault = profilerLabelId.GetValueOrDefault();
				for (int k = 0; k < displayRows.Count; k++)
				{
					if (displayRows[k].labelId == valueOrDefault && displayRows[k].pathKey == this.previousSelectionPathKey)
					{
						return k;
					}
				}
			}
			return Mathf.Min(this.selectedIndex, displayRows.Count - 1);
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00180810 File Offset: 0x0017EA10
		public bool TryGetSelectedZones(List<int> pathIds, out int ordinal)
		{
			pathIds.Clear();
			ordinal = -1;
			ProfilerTreeModel.Row selectedRow = this.SelectedRow;
			if (!this.session.HasData || selectedRow == null || !selectedRow.labelId.IsValid || selectedRow.pathIds.Count == 0)
			{
				return false;
			}
			pathIds.AddRange(selectedRow.pathIds);
			ordinal = this.session.Ordinal;
			return true;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00180873 File Offset: 0x0017EA73
		public void ExpandTo(List<ProfilerLabelId> pathLabelIds)
		{
			this.pendingSelectKey = this.Tree.ExpandTo(pathLabelIds);
			this.Invalidate();
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00180890 File Offset: 0x0017EA90
		public override void Notify_SearchChanged()
		{
			this.View.SearchMatchesTruncated = false;
			ProfilerSearchModel search = this.session.Search;
			if (!this.Tree.Inverted && search.Query.Length >= 2 && this.session.HasData)
			{
				this.ExpandToMatches(search);
			}
			this.selectFirstMatchOnRebuild = true;
			this.Invalidate();
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x001808F4 File Offset: 0x0017EAF4
		private void ExpandToMatches(ProfilerSearchModel search)
		{
			this.Tree.CollectMatchingPaths(search, this.searchMatchScratch);
			this.searchMatchScratch.Sort(ProfilerCallTreeTable.MatchComparison);
			this.searchPathKeyScratch.Clear();
			for (int i = 0; i < this.searchMatchScratch.Count; i++)
			{
				this.Tree.AppendLabelChain(this.searchMatchScratch[i].Item2, this.searchPathScratch);
				ulong num = 14695981039346656037UL;
				for (int j = 0; j < this.searchPathScratch.Count; j++)
				{
					num = ProfilerTreeModel.CombineKey(num, this.searchPathScratch[j]);
				}
				if (this.searchPathKeyScratch.Add(num))
				{
					if (this.searchPathKeyScratch.Count > 24)
					{
						this.View.SearchMatchesTruncated = true;
						return;
					}
					this.Tree.ExpandTo(this.searchPathScratch);
				}
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x001809DB File Offset: 0x0017EBDB
		public override void CollapseSelected()
		{
			this.View.CollapseSelected(!this.FlamegraphMode);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001809F1 File Offset: 0x0017EBF1
		public override void ExpandAll()
		{
			this.Tree.ExpandAll();
			this.Invalidate();
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00180A04 File Offset: 0x0017EC04
		public override void CollapseAll()
		{
			this.Tree.CollapseAll();
			this.Invalidate();
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00180A18 File Offset: 0x0017EC18
		protected override void ToggleExpansionAt(int index)
		{
			if (index < 0 || index >= this.Tree.DisplayRows.Count)
			{
				return;
			}
			this.View.Select(index);
			if (this.FlamegraphMode)
			{
				return;
			}
			ProfilerTreeModel.Row row = this.Tree.DisplayRows[index];
			if (!row.labelId.IsValid || !row.hasChildren)
			{
				return;
			}
			this.Tree.SetExpanded(row.pathKey, !row.expanded);
			this.pendingSelectKey = row.pathKey;
			this.Invalidate();
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00180AA8 File Offset: 0x0017ECA8
		private void SetSortColumn(ProfilerTreeModel.SortColumn column)
		{
			if (this.Tree.ActiveSortColumn == column)
			{
				this.Tree.SortAscending = !this.Tree.SortAscending;
			}
			else
			{
				this.Tree.ActiveSortColumn = column;
				this.Tree.SortAscending = (column == ProfilerTreeModel.SortColumn.Label);
			}
			this.session.Invalidate();
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x00180B04 File Offset: 0x0017ED04
		private string HeaderText(string name, ProfilerTreeModel.SortColumn column)
		{
			if (this.Tree.ActiveSortColumn != column)
			{
				return name;
			}
			return name + (this.Tree.SortAscending ? " ↑" : " ↓");
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00180B38 File Offset: 0x0017ED38
		protected override void AppendCopyContext(StringBuilder builder)
		{
			if (this.FlamegraphMode)
			{
				builder.AppendLine(this.Tree.Inverted ? "inverted flamegraph" : "flamegraph");
				return;
			}
			if (this.Tree.Inverted)
			{
				builder.AppendLine("inverted call stack");
			}
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00180B88 File Offset: 0x0017ED88
		protected override bool BuildFullSubtree(int index, List<ProfilerRowData> into)
		{
			if (this.FlamegraphMode || index >= this.Tree.DisplayRows.Count)
			{
				return false;
			}
			ulong pathKey = this.Tree.DisplayRows[index].pathKey;
			this.Tree.BuildFullTreeRows(this.fullRowScratch);
			int num = 0;
			while (num < this.fullRowScratch.Count && this.fullRowScratch[num].pathKey != pathKey)
			{
				num++;
			}
			int num2 = num;
			while (num2 < this.fullRowScratch.Count && (num2 == num || this.fullRowScratch[num2].depth > this.fullRowScratch[num].depth))
			{
				ProfilerRowData profilerRowData = new ProfilerRowData();
				this.Shape(this.fullRowScratch[num2], profilerRowData, this.frameTotalMicroseconds);
				into.Add(profilerRowData);
				num2++;
			}
			this.Tree.ReleaseRows(this.fullRowScratch);
			return into.Count > 0;
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00180C84 File Offset: 0x0017EE84
		public static string RootCrumbLabel()
		{
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			int num;
			int index;
			ProfilerThreadRegistry.GetSelectionSummary(out num, out index);
			if (num == 0)
			{
				if (entries.Count != 0)
				{
					return "No threads";
				}
				return "Frame";
			}
			else if (num == 1)
			{
				if (!entries[index].isMain)
				{
					return FrameProfiler.GetLabel(entries[index].nameLabelId);
				}
				return "Frame";
			}
			else
			{
				if (num != entries.Count)
				{
					return num.ToString() + " threads";
				}
				return "All threads";
			}
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x00180D04 File Offset: 0x0017EF04
		public override void Draw(Rect rect)
		{
			if (!this.FlamegraphMode)
			{
				base.Draw(rect);
				return;
			}
			GUI.BeginGroup(rect);
			try
			{
				this.DrawFlamegraph(new Rect(0f, 0f, rect.width, rect.height - 24f));
				this.View.DrawBreadcrumbs(new Rect(0f, rect.height - 24f, rect.width, 24f));
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00180D98 File Offset: 0x0017EF98
		// (set) Token: 0x06003CED RID: 15597 RVA: 0x00180DA0 File Offset: 0x0017EFA0
		public bool FlamegraphMode { get; private set; }

		// Token: 0x06003CEE RID: 15598 RVA: 0x00180DA9 File Offset: 0x0017EFA9
		public void SetFlamegraphMode(bool value)
		{
			if (this.FlamegraphMode == value)
			{
				return;
			}
			this.FlamegraphMode = value;
			this.flameView.CancelAnimation();
			this.flameView.UserView = false;
			this.ResetSelection();
			this.Invalidate();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00180DE0 File Offset: 0x0017EFE0
		private void DrawFlamegraph(Rect rect)
		{
			ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
			GUI.BeginGroup(rect);
			try
			{
				this.HandleFlamegraphInput(rect.width, rect.height);
				if (ProfilerGui.Painting)
				{
					this.PaintFlamegraph(rect.width, rect.height);
				}
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00180E48 File Offset: 0x0017F048
		private void HandleFlamegraphInput(float w, float h)
		{
			Event current = Event.current;
			Vector2 mousePosition = current.mousePosition;
			bool flag = mousePosition.x >= 0f && mousePosition.y >= 0f && mousePosition.x < w && mousePosition.y < h;
			if (current.type == EventType.mouseDown && flag)
			{
				if (current.button == 0)
				{
					ProfilerCallTreeTable.Bar bar;
					bool flag2 = this.TryHitBar(mousePosition, out bar);
					if (current.clickCount >= 2)
					{
						if (flag2)
						{
							this.flameView.AnimateTo(bar.u0, bar.u1 - bar.u0, false);
						}
						else
						{
							this.flameView.TopLane = 0f;
							this.flameView.AnimateTo(0.0, this.flameView.Extent, true);
						}
					}
					else if (flag2)
					{
						this.selectingFromFlamegraph = true;
						this.View.Select(bar.rowIndex);
						this.selectingFromFlamegraph = false;
					}
				}
				else
				{
					this.flameView.BeginPan(mousePosition);
				}
				current.Use();
				return;
			}
			if (current.type == EventType.scrollWheel && flag)
			{
				this.flameView.HandleWheel(current.delta, mousePosition.x, 0f, w);
				current.Use();
				return;
			}
			if (current.type == EventType.repaint && this.flameView.Panning)
			{
				if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
				{
					this.flameView.EndPan();
					return;
				}
				this.flameView.TryPan(mousePosition, w, 19f);
			}
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00180FC8 File Offset: 0x0017F1C8
		private void PaintFlamegraph(float w, float h)
		{
			this.bars.Clear();
			if (this.rows.Count == 0 || w < 16f || h < 16f)
			{
				this.flameView.CancelAnimation();
				return;
			}
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < this.rows.Count; i++)
			{
				if (this.rows[i].Depth == 0)
				{
					num += this.rows[i].BarFraction;
				}
				num2 = Mathf.Max(num2, this.rows[i].Depth + 1);
			}
			this.flameView.Extent = ((num > 0f) ? ((double)num) : 1.0);
			this.flameView.Advance();
			if (!this.flameView.UserView)
			{
				this.flameView.TopLane = 0f;
				this.flameView.Start = 0.0;
				this.flameView.Span = this.flameView.Extent;
			}
			this.flameView.Clamp(Mathf.Max(1f, h / 19f), (float)num2);
			if (this.laneCursor.Length <= num2)
			{
				this.laneCursor = new double[Mathf.NextPowerOfTwo(num2 + 1)];
			}
			this.laneCursor[0] = 0.0;
			IGuiBackend guiBackend = GuiBackend.Current;
			double num3 = this.flameView.Start + this.flameView.Span;
			int num4 = 0;
			int num5 = -1;
			for (int j = 0; j < this.rows.Count; j++)
			{
				ProfilerRowData profilerRowData = this.rows[j];
				int depth = profilerRowData.Depth;
				double num6 = this.laneCursor[depth];
				double num7 = num6 + (double)profilerRowData.BarFraction;
				this.laneCursor[depth] = num7;
				this.laneCursor[depth + 1] = num6;
				if (num7 <= this.flameView.Start || num6 >= num3)
				{
					j = this.SkipSubtree(j, depth);
				}
				else
				{
					float num8 = this.flameView.ToX(num6, 0f, w);
					float num9 = this.flameView.ToX(num7, 0f, w);
					float num10 = ((float)depth - this.flameView.TopLane) * 19f;
					float num11 = num10 + 19f - 1f;
					if (num9 - num8 < 0.75f || num10 >= h)
					{
						j = this.SkipSubtree(j, depth);
					}
					else if (num11 > 0f)
					{
						Rect rect = Rect.MinMaxRect(Mathf.Max(0f, num8 + 0.5f), Mathf.Max(0f, num10), Mathf.Min(w, num9 - 0.5f), Mathf.Min(h, num11));
						if (rect.width > 0f && rect.height > 0f)
						{
							guiBackend.FillRect(rect, profilerRowData.Swatch);
							if (profilerRowData.Dimmed)
							{
								guiBackend.FillRect(rect, ProfilerCallTreeTable.DimColor);
							}
						}
						if (j == this.selectedIndex)
						{
							num5 = this.bars.Count;
						}
						List<ProfilerCallTreeTable.Bar> list = this.bars;
						ProfilerCallTreeTable.Bar item = new ProfilerCallTreeTable.Bar
						{
							x0 = num8,
							x1 = num9,
							y0 = num10,
							y1 = num11,
							u0 = num6,
							u1 = num7,
							rowIndex = j
						};
						list.Add(item);
						float num12 = Mathf.Max(0f, num8);
						float num13 = Mathf.Min(w, num9);
						if (num13 - num12 >= 40f && num4 < 240)
						{
							num4++;
							ProfilerGui.Label(new Rect(num12 + 3f, num10, num13 - num12 - 6f, 18f), profilerRowData.Label, ProfilerPalette.ZoneLabelColor, TextAnchor.MiddleLeft, GameFont.Tiny);
						}
					}
				}
			}
			if (num5 >= 0)
			{
				ProfilerCallTreeTable.Bar item = this.bars[num5];
				ProfilerGui.Outline(ProfilerCallTreeTable.BarRect(item, w), Color.white);
			}
			if (this.revealInFlamegraph)
			{
				this.revealInFlamegraph = false;
				this.RevealSelectedBar(num5 >= 0, h);
			}
			this.PaintFlamegraphHover(w, h);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x001813F7 File Offset: 0x0017F5F7
		private static Rect BarRect(in ProfilerCallTreeTable.Bar bar, float w)
		{
			return Rect.MinMaxRect(Mathf.Max(0f, bar.x0), bar.y0, Mathf.Min(w, bar.x1), bar.y1);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00181428 File Offset: 0x0017F628
		private void PaintFlamegraphHover(float w, float h)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			ProfilerCallTreeTable.Bar bar;
			if (mousePosition.x < 0f || mousePosition.y < 0f || mousePosition.x >= w || mousePosition.y >= h || this.flameView.Panning || !this.TryHitBar(mousePosition, out bar))
			{
				this.flameHoverRow = -1;
				return;
			}
			if (bar.rowIndex != this.flameHoverRow || this.flameTooltip == null)
			{
				this.flameHoverRow = bar.rowIndex;
				this.flameTooltip = ProfilerCallTreeTable.FlamegraphTooltip(this.rows[bar.rowIndex]);
			}
			ProfilerGui.Outline(ProfilerCallTreeTable.BarRect(bar, w), ProfilerCallTreeTable.FlameHoverStroke);
			ProfilerGui.SetTooltip(this.flameTooltip);
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x001814EC File Offset: 0x0017F6EC
		private static string FlamegraphTooltip(ProfilerRowData data)
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append(data.Label);
			scratch.Append("\ntotal ");
			ProfilerTreeModel.AppendMs(scratch, (float)(data.TotalMicroseconds / 1000.0), "0.000", " ");
			if (data.HasSelf)
			{
				scratch.Append(" / self ");
				ProfilerTreeModel.AppendMs(scratch, (float)(data.SelfMicroseconds / 1000.0), "0.000", " ");
			}
			scratch.Append(" / ").Append((data.BarFraction * 100f).ToString("0.0")).Append('%');
			if (data.Calls > 0L)
			{
				scratch.Append("\ncalls ").Append(data.Calls);
			}
			if (data.AllocBytes > 0L)
			{
				scratch.Append((data.Calls > 0L) ? " / alloc " : "\nalloc ");
				ProfilerTreeModel.AppendAllocBytes(scratch, data.AllocBytes, " ");
			}
			return scratch.ToString();
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00181608 File Offset: 0x0017F808
		private void RevealSelectedBar(bool alreadyVisible, float h)
		{
			if (alreadyVisible || this.selectedIndex < 0 || this.selectedIndex >= this.rows.Count)
			{
				return;
			}
			this.laneCursor[0] = 0.0;
			double num = 0.0;
			double num2 = 0.0;
			for (int i = 0; i <= this.selectedIndex; i++)
			{
				ProfilerRowData profilerRowData = this.rows[i];
				num = this.laneCursor[profilerRowData.Depth];
				num2 = num + (double)profilerRowData.BarFraction;
				this.laneCursor[profilerRowData.Depth] = num2;
				this.laneCursor[profilerRowData.Depth + 1] = num;
			}
			if (num2 <= num)
			{
				return;
			}
			int depth = this.rows[this.selectedIndex].Depth;
			float num3 = h / 19f;
			if ((float)depth < this.flameView.TopLane || (float)(depth + 1) > this.flameView.TopLane + num3)
			{
				this.flameView.TopLane = Mathf.Max(0f, (float)depth - num3 * 0.5f);
			}
			double num4 = num2 - num;
			this.flameView.AnimateTo(num - num4, num4 * 3.0, false);
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0018173E File Offset: 0x0017F93E
		private int SkipSubtree(int index, int depth)
		{
			while (index + 1 < this.rows.Count && this.rows[index + 1].Depth > depth)
			{
				index++;
			}
			return index;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00181770 File Offset: 0x0017F970
		private bool TryHitBar(Vector2 position, out ProfilerCallTreeTable.Bar hit)
		{
			for (int i = 0; i < this.bars.Count; i++)
			{
				ProfilerCallTreeTable.Bar bar = this.bars[i];
				if (position.x >= bar.x0 && position.x < bar.x1 && position.y >= bar.y0 && position.y < bar.y1)
				{
					hit = bar;
					return true;
				}
			}
			hit = default(ProfilerCallTreeTable.Bar);
			return false;
		}

		// Token: 0x0400278D RID: 10125
		private const int AutoExpandMatchLimit = 24;

		// Token: 0x0400278E RID: 10126
		private const int AutoExpandMinQueryLength = 2;

		// Token: 0x0400278F RID: 10127
		[TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})]
		private static readonly Comparison<ValueTuple<float, int>> MatchComparison = ([TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})] ValueTuple<float, int> lhs, [TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})] ValueTuple<float, int> rhs) => rhs.Item1.CompareTo(lhs.Item1);

		// Token: 0x04002790 RID: 10128
		private readonly List<FrameProfiler.FrameSnapshot> frames = new List<FrameProfiler.FrameSnapshot>();

		// Token: 0x04002791 RID: 10129
		[TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})]
		private readonly List<ValueTuple<float, int>> searchMatchScratch = new List<ValueTuple<float, int>>();

		// Token: 0x04002792 RID: 10130
		private readonly List<ProfilerLabelId> searchPathScratch = new List<ProfilerLabelId>();

		// Token: 0x04002793 RID: 10131
		private readonly HashSet<ulong> searchPathKeyScratch = new HashSet<ulong>();

		// Token: 0x04002794 RID: 10132
		private readonly List<ProfilerTreeModel.Row> fullRowScratch = new List<ProfilerTreeModel.Row>();

		// Token: 0x04002795 RID: 10133
		private ProfilerRefreshGate refreshGate;

		// Token: 0x04002796 RID: 10134
		private bool selectFirstMatchOnRebuild;

		// Token: 0x04002797 RID: 10135
		private ulong pendingSelectKey;

		// Token: 0x04002798 RID: 10136
		private ProfilerLabelId? previousSelectionLabelId;

		// Token: 0x04002799 RID: 10137
		private ulong previousSelectionPathKey;

		// Token: 0x0400279A RID: 10138
		private double frameTotalMicroseconds = 1.0;

		// Token: 0x0400279B RID: 10139
		public Action RowSelectedByUser;

		// Token: 0x0400279C RID: 10140
		private const float FlameLaneHeight = 19f;

		// Token: 0x0400279D RID: 10141
		private const float FlameLaneGap = 1f;

		// Token: 0x0400279E RID: 10142
		private const float MinBarWidth = 0.75f;

		// Token: 0x0400279F RID: 10143
		private const float MinLabelWidth = 40f;

		// Token: 0x040027A0 RID: 10144
		private const int MaxLabels = 240;

		// Token: 0x040027A1 RID: 10145
		private const double MinViewSpan = 1E-07;

		// Token: 0x040027A2 RID: 10146
		private static readonly Color DimColor = ProfilerGui.Hex(2566914048U);

		// Token: 0x040027A3 RID: 10147
		private static readonly Color FlameHoverStroke = ProfilerGui.Hex(2868903935U);

		// Token: 0x040027A4 RID: 10148
		private readonly ProfilerPanZoomView flameView = new ProfilerPanZoomView(1E-07);

		// Token: 0x040027A5 RID: 10149
		private readonly List<ProfilerCallTreeTable.Bar> bars = new List<ProfilerCallTreeTable.Bar>();

		// Token: 0x040027A6 RID: 10150
		private double[] laneCursor = new double[64];

		// Token: 0x040027A7 RID: 10151
		private bool revealInFlamegraph;

		// Token: 0x040027A8 RID: 10152
		private bool selectingFromFlamegraph;

		// Token: 0x040027A9 RID: 10153
		private int flameHoverRow = -1;

		// Token: 0x040027AA RID: 10154
		private string flameTooltip;

		// Token: 0x02002A96 RID: 10902
		private struct Bar
		{
			// Token: 0x0400AE71 RID: 44657
			public float x0;

			// Token: 0x0400AE72 RID: 44658
			public float x1;

			// Token: 0x0400AE73 RID: 44659
			public float y0;

			// Token: 0x0400AE74 RID: 44660
			public float y1;

			// Token: 0x0400AE75 RID: 44661
			public double u0;

			// Token: 0x0400AE76 RID: 44662
			public double u1;

			// Token: 0x0400AE77 RID: 44663
			public int rowIndex;
		}
	}
}
