using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079B RID: 1947
	public sealed class ProfilerRowListView
	{
		// Token: 0x06003DF9 RID: 15865 RVA: 0x0018A888 File Offset: 0x00188A88
		public ProfilerRowListView(ProfilerSession session)
		{
			this.session = session;
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x0018A8D5 File Offset: 0x00188AD5
		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0018A8DD File Offset: 0x00188ADD
		// (set) Token: 0x06003DFC RID: 15868 RVA: 0x0018A8E5 File Offset: 0x00188AE5
		public string SearchStatus { get; private set; } = "";

		// Token: 0x06003DFD RID: 15869 RVA: 0x0018A8EE File Offset: 0x00188AEE
		public void RequestReveal()
		{
			this.revealPending = true;
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0018A8F8 File Offset: 0x00188AF8
		public void Resize(int count)
		{
			while (this.Rows.Count > count)
			{
				this.rowPool.Add(this.Rows[this.Rows.Count - 1]);
				this.Rows.RemoveAt(this.Rows.Count - 1);
			}
			while (this.Rows.Count < count)
			{
				if (this.rowPool.Count > 0)
				{
					this.Rows.Add(this.rowPool[this.rowPool.Count - 1]);
					this.rowPool.RemoveAt(this.rowPool.Count - 1);
				}
				else
				{
					this.Rows.Add(new ProfilerRowData());
				}
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0018A9BB File Offset: 0x00188BBB
		public void Notify_RowsChanged()
		{
			if (this.selectedIndex >= this.Rows.Count)
			{
				this.selectedIndex = this.Rows.Count - 1;
			}
			this.UpdateSearchStatus();
			this.RefreshBreadcrumbs();
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0018A9EF File Offset: 0x00188BEF
		public void SelectQuietly(int index)
		{
			if (this.selectedIndex == index)
			{
				return;
			}
			this.selectedIndex = index;
			this.UpdateSearchStatus();
			this.RefreshBreadcrumbs();
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0018AA0E File Offset: 0x00188C0E
		public void Select(int index)
		{
			if (this.selectedIndex == index)
			{
				return;
			}
			this.SelectQuietly(index);
			this.revealPending = true;
			Action selectedByUser = this.SelectedByUser;
			if (selectedByUser == null)
			{
				return;
			}
			selectedByUser();
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0018AA38 File Offset: 0x00188C38
		public void UpdateSearchStatus()
		{
			if (!this.session.Search.IsActive)
			{
				this.SearchStatus = "";
				return;
			}
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < this.Rows.Count; i++)
			{
				if (!this.Rows[i].Dimmed)
				{
					num++;
					if (i == this.selectedIndex)
					{
						num2 = num;
					}
				}
			}
			string text = this.SearchMatchesTruncated ? (num.ToString() + "+") : num.ToString();
			string text2 = (num == 1 && !this.SearchMatchesTruncated) ? "match" : "matches";
			this.SearchStatus = ((num2 > 0) ? string.Concat(new string[]
			{
				num2.ToString(),
				"/",
				text,
				" ",
				text2
			}) : (text + " " + text2));
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0018AB28 File Offset: 0x00188D28
		public void SelectNextMatch(int direction)
		{
			if (!this.session.Search.IsActive || this.Rows.Count == 0)
			{
				return;
			}
			int num = this.selectedIndex;
			if (num < 0 && direction < 0)
			{
				num = 0;
			}
			for (int i = 1; i <= this.Rows.Count; i++)
			{
				int index = ((num + i * direction) % this.Rows.Count + this.Rows.Count) % this.Rows.Count;
				if (!this.Rows[index].Dimmed)
				{
					this.Select(index);
					return;
				}
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0018ABC4 File Offset: 0x00188DC4
		public void SelectFirstMatch()
		{
			for (int i = 0; i < this.Rows.Count; i++)
			{
				if (!this.Rows[i].Dimmed)
				{
					this.SelectQuietly(i);
					this.revealPending = true;
					return;
				}
			}
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0018AC09 File Offset: 0x00188E09
		public void MoveSelection(int direction)
		{
			if (this.Rows.Count == 0)
			{
				return;
			}
			this.Select(Mathf.Clamp(this.selectedIndex + direction, 0, this.Rows.Count - 1));
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0018AC3C File Offset: 0x00188E3C
		public void ActivateSelected(bool expandOnly)
		{
			int num = this.selectedIndex;
			if (num < 0 || num >= this.Rows.Count)
			{
				return;
			}
			if (!this.Rows[num].HasChildren)
			{
				if (expandOnly)
				{
					if (num + 1 < this.Rows.Count)
					{
						this.Select(num + 1);
					}
					return;
				}
				Action<int> activateLeaf = this.ActivateLeaf;
				if (activateLeaf == null)
				{
					return;
				}
				activateLeaf(num);
				return;
			}
			else
			{
				if (expandOnly && this.Rows[num].Expanded)
				{
					if (num + 1 < this.Rows.Count && this.Rows[num + 1].Depth > this.Rows[num].Depth)
					{
						this.Select(num + 1);
					}
					return;
				}
				Action<int> toggleExpansion = this.ToggleExpansion;
				if (toggleExpansion == null)
				{
					return;
				}
				toggleExpansion(num);
				return;
			}
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0018AD0C File Offset: 0x00188F0C
		public void CollapseSelected(bool collapseExpanded = true)
		{
			int num = this.selectedIndex;
			if (num < 0 || num >= this.Rows.Count)
			{
				return;
			}
			if (!collapseExpanded || !this.Rows[num].Expanded)
			{
				int depth = this.Rows[num].Depth;
				for (int i = num - 1; i >= 0; i--)
				{
					if (this.Rows[i].Depth < depth)
					{
						this.Select(i);
						return;
					}
				}
				if (num > 0)
				{
					this.Select(num - 1);
				}
				return;
			}
			Action<int> toggleExpansion = this.ToggleExpansion;
			if (toggleExpansion == null)
			{
				return;
			}
			toggleExpansion(num);
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0018ADA4 File Offset: 0x00188FA4
		public bool CopySelectedRow()
		{
			StringBuilder builder = new StringBuilder();
			if (!ProfilerClipboard.AppendRow(builder, this.Rows, this.selectedIndex))
			{
				return false;
			}
			ProfilerClipboard.Put(builder);
			return true;
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0018ADD4 File Offset: 0x00188FD4
		private void RefreshBreadcrumbs()
		{
			this.crumbs.Clear();
			if (this.selectedIndex < 0 || this.selectedIndex >= this.Rows.Count)
			{
				return;
			}
			int num = int.MaxValue;
			int num2 = this.selectedIndex;
			while (num2 >= 0 && num > 0)
			{
				if (this.Rows[num2].Depth < num)
				{
					num = this.Rows[num2].Depth;
					this.crumbs.Add(new ValueTuple<string, int>(this.Rows[num2].Label, num2));
				}
				num2--;
			}
			this.crumbs.Reverse();
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003E0A RID: 15882 RVA: 0x0018AE78 File Offset: 0x00189078
		private float ColumnsWidth
		{
			get
			{
				float num = 72f;
				for (int i = 0; i < this.Columns.Length; i++)
				{
					num += this.Columns[i].width;
				}
				return num;
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0018AEB0 File Offset: 0x001890B0
		private void LabelAndDetailLayout(float w, out float labelX, out float detailX, out float detailWidth)
		{
			labelX = this.ColumnsWidth + 10f;
			float num = w - labelX - 6f - 8f;
			detailWidth = ((this.DetailHeader != null) ? Mathf.Floor(num * 0.4f) : 0f);
			detailX = w - 6f - 8f - detailWidth;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0018AF10 File Offset: 0x00189110
		public void DrawHeader(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
			float num = rect.x + 12f + 60f;
			for (int i = 0; i < this.Columns.Length; i++)
			{
				ProfilerColumn profilerColumn = this.Columns[i];
				Rect rect2 = new Rect(num, rect.y, profilerColumn.width, rect.height - 1f);
				num += profilerColumn.width;
				bool flag = profilerColumn.sort != null && ProfilerGui.Hovered(rect2);
				ProfilerGui.Label(rect2, profilerColumn.header(), flag ? ProfilerGui.TextDim : ProfilerGui.TextFaint, TextAnchor.MiddleRight, GameFont.Tiny);
				if (profilerColumn.sort != null && ProfilerGui.Pressed(rect2, null))
				{
					profilerColumn.sort();
				}
			}
			float num2;
			float num3;
			float width;
			this.LabelAndDetailLayout(rect.width, out num2, out num3, out width);
			Rect rect3 = new Rect(rect.x + num2, rect.y, 120f, rect.height - 1f);
			bool flag2 = this.LabelSort != null && ProfilerGui.Hovered(rect3);
			ProfilerGui.Label(rect3, this.LabelHeader(), flag2 ? ProfilerGui.TextDim : ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			if (this.LabelSort != null && ProfilerGui.Pressed(rect3, null))
			{
				this.LabelSort();
			}
			if (this.DetailHeader != null)
			{
				ProfilerGui.Label(new Rect(rect.x + num3, rect.y, width, rect.height - 1f), this.DetailHeader, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			}
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0018B0DC File Offset: 0x001892DC
		public void DrawRows(Rect rect)
		{
			float height = rect.height;
			float num = (float)this.Rows.Count * 20f;
			float num2 = Mathf.Max(0f, num - height);
			if (this.revealPending && ProfilerGui.Painting)
			{
				this.revealPending = false;
				if (this.selectedIndex >= 0)
				{
					float num3 = (float)this.selectedIndex * 20f;
					if (num3 < this.scrollY)
					{
						this.scrollY = num3;
					}
					else if (num3 + 20f > this.scrollY + height)
					{
						this.scrollY = num3 + 20f - height;
					}
				}
			}
			this.scrollY = Mathf.Clamp(this.scrollY, 0f, num2);
			GUI.BeginGroup(rect);
			try
			{
				Rect local = new Rect(0f, 0f, rect.width, rect.height);
				Event current = Event.current;
				Vector2 mousePosition = current.mousePosition;
				bool flag = num2 > 0f && mousePosition.x >= rect.width - 6f - 4f;
				this.HandleScrollBar(local, num, num2);
				if (current.type == EventType.scrollWheel && local.Contains(mousePosition))
				{
					this.scrollY = Mathf.Clamp(this.scrollY + current.delta.y / 3f * 3f * 20f, 0f, num2);
					current.Use();
				}
				else if (current.type == EventType.mouseDown && local.Contains(mousePosition) && !flag)
				{
					int num4 = Mathf.FloorToInt((mousePosition.y + this.scrollY) / 20f);
					if (num4 >= 0 && num4 < this.Rows.Count)
					{
						this.OnRowMouseDown(num4, mousePosition.x, current);
					}
					current.Use();
				}
				if (ProfilerGui.Painting)
				{
					int num5 = Mathf.Max(0, Mathf.FloorToInt(this.scrollY / 20f));
					int num6 = Mathf.Min(this.Rows.Count - 1, Mathf.FloorToInt((this.scrollY + height) / 20f));
					int num7 = (local.Contains(mousePosition) && !flag && !this.draggingThumb) ? Mathf.FloorToInt((mousePosition.y + this.scrollY) / 20f) : -1;
					for (int i = num5; i <= num6; i++)
					{
						this.DrawRow(i, new Rect(0f, (float)i * 20f - this.scrollY, rect.width, 20f), i == num7);
					}
					if (num2 > 0f)
					{
						Rect rect2 = this.ThumbRect(local, num, num2);
						ProfilerGui.Fill(rect2, (this.draggingThumb || rect2.Contains(mousePosition)) ? ProfilerGui.ScrollThumbHover : ProfilerGui.ScrollThumb);
					}
				}
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0018B3C8 File Offset: 0x001895C8
		private Rect ThumbRect(Rect local, float contentHeight, float maxScroll)
		{
			float num = Mathf.Max(24f, local.height * local.height / contentHeight);
			float y = (local.height - num) * (this.scrollY / maxScroll);
			return new Rect(local.width - 6f - 2f, y, 6f, num);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0018B424 File Offset: 0x00189624
		private void HandleScrollBar(Rect local, float contentHeight, float maxScroll)
		{
			if (maxScroll <= 0f)
			{
				this.draggingThumb = false;
				return;
			}
			Event current = Event.current;
			Rect rect = this.ThumbRect(local, contentHeight, maxScroll);
			if (current.type == EventType.mouseDown && current.button == 0 && current.mousePosition.x >= local.width - 6f - 4f && local.Contains(current.mousePosition))
			{
				this.draggingThumb = true;
				this.thumbGrabOffset = (rect.Contains(current.mousePosition) ? (current.mousePosition.y - rect.y) : (rect.height * 0.5f));
				current.Use();
			}
			if (!this.draggingThumb || current.type != EventType.repaint)
			{
				return;
			}
			if (!Input.GetMouseButton(0))
			{
				this.draggingThumb = false;
				return;
			}
			float num = local.height - rect.height;
			if (num > 0f)
			{
				this.scrollY = Mathf.Clamp((current.mousePosition.y - this.thumbGrabOffset) / num, 0f, 1f) * maxScroll;
			}
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0018B53C File Offset: 0x0018973C
		private void OnRowMouseDown(int index, float mouseX, Event e)
		{
			if (e.button == 1)
			{
				this.Select(index);
				Action contextMenu = this.ContextMenu;
				if (contextMenu == null)
				{
					return;
				}
				contextMenu();
				return;
			}
			else
			{
				if (e.button != 0)
				{
					return;
				}
				ProfilerRowData profilerRowData = this.Rows[index];
				float num = this.ColumnsWidth + 10f + (float)profilerRowData.Depth * 14f;
				if (!profilerRowData.HasChildren || mouseX < num || mouseX >= num + 14f)
				{
					this.Select(index);
					if (e.clickCount >= 2)
					{
						this.ActivateSelected(false);
					}
					return;
				}
				Action<int> toggleExpansion = this.ToggleExpansion;
				if (toggleExpansion == null)
				{
					return;
				}
				toggleExpansion(index);
				return;
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0018B5DC File Offset: 0x001897DC
		private void DrawRow(int index, Rect rect, bool hovered)
		{
			ProfilerRowData profilerRowData = this.Rows[index];
			if (index == this.selectedIndex)
			{
				ProfilerGui.Fill(rect, ProfilerGui.RowSelected);
			}
			else if (hovered)
			{
				ProfilerGui.Fill(rect, ProfilerGui.FieldBackground);
			}
			ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerRowListView.RowBorder);
			float alpha = profilerRowData.Dimmed ? 0.35f : 1f;
			float num = 12f;
			Rect rect2 = new Rect(num, rect.y + 6.5f, 56f, 7f);
			ProfilerGui.Fill(rect2, ProfilerRowListView.Fade(ProfilerGui.BarTrack, alpha));
			ProfilerGui.Fill(new Rect(rect2.x, rect2.y, rect2.width * profilerRowData.BarFraction, rect2.height), ProfilerRowListView.Fade(ProfilerGui.BarFill, alpha));
			num += 60f;
			profilerRowData.EnsureDisplayText();
			for (int i = 0; i < this.Columns.Length; i++)
			{
				ProfilerColumn profilerColumn = this.Columns[i];
				ProfilerGui.Label(new Rect(num, rect.y, profilerColumn.width, rect.height), ProfilerRowListView.CellText(profilerRowData, profilerColumn.kind), ProfilerRowListView.Fade(ProfilerRowListView.CellColor(profilerRowData, profilerColumn.kind), alpha), TextAnchor.MiddleRight, GameFont.Tiny);
				num += profilerColumn.width;
			}
			float num2;
			float num3;
			float width;
			this.LabelAndDetailLayout(rect.width, out num2, out num3, out width);
			num = num2 + (float)profilerRowData.Depth * 14f;
			Rect cell = new Rect(num, rect.y, 14f, rect.height);
			if (profilerRowData.HasChildren)
			{
				ProfilerGui.Arrow(cell, profilerRowData.Expanded, ProfilerRowListView.Fade(ProfilerGui.TextDim, alpha));
			}
			num += 16f;
			float num4 = ((this.DetailHeader != null) ? (num3 - 8f) : (rect.width - 6f - 8f)) - num;
			if (num4 > 20f)
			{
				ProfilerGui.Label(new Rect(num, rect.y, num4, rect.height), profilerRowData.FittedLabel(num4), ProfilerRowListView.Fade(ProfilerGui.TextMain, alpha), TextAnchor.MiddleLeft, GameFont.Tiny);
			}
			if (this.DetailHeader != null && !string.IsNullOrEmpty(profilerRowData.Detail))
			{
				ProfilerGui.Label(new Rect(num3, rect.y, width, rect.height), profilerRowData.Detail, ProfilerRowListView.Fade(ProfilerGui.TextFaint, alpha), TextAnchor.MiddleLeft, GameFont.Tiny);
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0018B858 File Offset: 0x00189A58
		private static string CellText(ProfilerRowData row, ProfilerColumnKind kind)
		{
			switch (kind)
			{
			case ProfilerColumnKind.Percent:
				return row.PercentText;
			case ProfilerColumnKind.Total:
				return row.TotalText;
			case ProfilerColumnKind.Self:
				return row.SelfText;
			case ProfilerColumnKind.Delta:
				return row.DeltaText;
			case ProfilerColumnKind.Calls:
				return row.CallsText;
			default:
				return row.AllocText;
			}
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0018B8AA File Offset: 0x00189AAA
		private static Color CellColor(ProfilerRowData row, ProfilerColumnKind kind)
		{
			if (kind == ProfilerColumnKind.Total)
			{
				return ProfilerGui.TextMain;
			}
			if (kind != ProfilerColumnKind.Delta)
			{
				return ProfilerGui.TextDim;
			}
			if (row.DeltaSeverity > 0)
			{
				return ProfilerGui.Regression;
			}
			if (row.DeltaSeverity >= 0)
			{
				return ProfilerGui.TextFaint;
			}
			return ProfilerGui.Improvement;
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0018B8E5 File Offset: 0x00189AE5
		private static Color Fade(Color color, float alpha)
		{
			color.a *= alpha;
			return color;
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0018B8F4 File Offset: 0x00189AF4
		public void DrawBreadcrumbs(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
			float num = rect.x + 12f;
			float y = rect.y + 3f;
			float num2 = rect.height - 5f;
			this.Crumb(ref num, y, num2, this.RootCrumb(), (this.crumbs.Count > 0) ? this.crumbs[0].Item2 : -1, this.crumbs.Count == 0);
			int num3 = 0;
			float num4 = rect.width - 12f - (num - rect.x);
			float num5 = 0f;
			for (int i = this.crumbs.Count - 1; i >= 0; i--)
			{
				num5 += Mathf.Min(180f, ProfilerGui.Width(this.crumbs[i].Item1, GameFont.Tiny)) + 24f;
				if (num5 > num4 && i < this.crumbs.Count - 1)
				{
					num3 = i + 1;
					break;
				}
			}
			if (num3 > 0)
			{
				num = ProfilerGui.Run(num, y, num2, " ›  …", ProfilerGui.TextFaint);
			}
			for (int j = num3; j < this.crumbs.Count; j++)
			{
				num = ProfilerGui.Run(num, y, num2, " › ", ProfilerGui.TextFaint);
				this.Crumb(ref num, y, num2, this.crumbs[j].Item1, this.crumbs[j].Item2, j == this.crumbs.Count - 1);
			}
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0018BAA8 File Offset: 0x00189CA8
		private void Crumb(ref float x, float y, float h, string label, int rowIndex, bool last)
		{
			string text = ProfilerGui.Ellipsize(label, 180f);
			float num = ProfilerGui.Width(text, GameFont.Tiny) + 10f;
			Rect rect = new Rect(x, y, num, h);
			if (rowIndex >= 0 && ProfilerGui.Hovered(rect))
			{
				ProfilerGui.Fill(rect, ProfilerRowListView.CrumbHover);
			}
			ProfilerGui.Label(rect, text, last ? ProfilerGui.TextMain : ProfilerGui.TextDim, TextAnchor.MiddleCenter, GameFont.Tiny);
			if (rowIndex >= 0 && rowIndex < this.Rows.Count && ProfilerGui.Pressed(rect, null))
			{
				this.Select(rowIndex);
			}
			x += num;
		}

		// Token: 0x040028D7 RID: 10455
		public const float HeaderHeight = 22f;

		// Token: 0x040028D8 RID: 10456
		public const float CrumbBarHeight = 24f;

		// Token: 0x040028D9 RID: 10457
		private const float RowHeight = 20f;

		// Token: 0x040028DA RID: 10458
		private const float IndentPerDepth = 14f;

		// Token: 0x040028DB RID: 10459
		private const float LeftPad = 12f;

		// Token: 0x040028DC RID: 10460
		private const float BarColumnWidth = 60f;

		// Token: 0x040028DD RID: 10461
		private const float ScrollBarWidth = 6f;

		// Token: 0x040028DE RID: 10462
		private const float WheelRows = 3f;

		// Token: 0x040028DF RID: 10463
		private const float ScrollUnitsPerNotch = 3f;

		// Token: 0x040028E0 RID: 10464
		private const float DetailWidthShare = 0.4f;

		// Token: 0x040028E1 RID: 10465
		private static readonly Color RowBorder = ProfilerGui.Hex(4279835940U);

		// Token: 0x040028E2 RID: 10466
		private static readonly Color CrumbHover = ProfilerGui.Hex(352321535U);

		// Token: 0x040028E3 RID: 10467
		private readonly ProfilerSession session;

		// Token: 0x040028E4 RID: 10468
		private readonly List<ProfilerRowData> rowPool = new List<ProfilerRowData>();

		// Token: 0x040028E5 RID: 10469
		[TupleElementNames(new string[]
		{
			"label",
			"rowIndex"
		})]
		private readonly List<ValueTuple<string, int>> crumbs = new List<ValueTuple<string, int>>();

		// Token: 0x040028E6 RID: 10470
		private int selectedIndex = -1;

		// Token: 0x040028E7 RID: 10471
		private bool revealPending;

		// Token: 0x040028E8 RID: 10472
		private float scrollY;

		// Token: 0x040028E9 RID: 10473
		private bool draggingThumb;

		// Token: 0x040028EA RID: 10474
		private float thumbGrabOffset;

		// Token: 0x040028EB RID: 10475
		public readonly List<ProfilerRowData> Rows = new List<ProfilerRowData>();

		// Token: 0x040028EC RID: 10476
		public ProfilerColumn[] Columns;

		// Token: 0x040028ED RID: 10477
		public Func<string> LabelHeader;

		// Token: 0x040028EE RID: 10478
		public Action LabelSort;

		// Token: 0x040028EF RID: 10479
		public string DetailHeader;

		// Token: 0x040028F0 RID: 10480
		public Func<string> RootCrumb;

		// Token: 0x040028F1 RID: 10481
		public bool SearchMatchesTruncated;

		// Token: 0x040028F2 RID: 10482
		public Action SelectedByUser;

		// Token: 0x040028F3 RID: 10483
		public Action<int> ToggleExpansion;

		// Token: 0x040028F4 RID: 10484
		public Action<int> ActivateLeaf;

		// Token: 0x040028F5 RID: 10485
		public Action ContextMenu;
	}
}
