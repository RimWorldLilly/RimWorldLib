using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LudeonTK
{
	// Token: 0x0200004D RID: 77
	public class Window_DebugTable : Window_Dev
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000EF91 File Offset: 0x0000D191
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public Window_DebugTable(string[,] tables)
		{
			this.tableRaw = tables;
			this.colVisible = new bool[this.tableRaw.GetLength(0)];
			this.rowsVisible = new bool[this.tableRaw.GetLength(1)];
			for (int i = 0; i < this.colVisible.Length; i++)
			{
				this.colVisible[i] = true;
			}
			for (int j = 0; j < this.rowsVisible.Length; j++)
			{
				this.rowsVisible[j] = true;
			}
			this.doCloseButton = true;
			this.doCloseX = true;
			this.focusFilter = true;
			Text.Font = GameFont.Tiny;
			this.BuildTableSorted();
			this.visibleWidth = this.colWidths.Sum();
			this.visibleHeight = this.rowHeights.Sum() - this.rowHeights[0];
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		private void BuildTableSorted()
		{
			if (this.sortMode == Window_DebugTable.SortMode.Off)
			{
				this.tableSorted = this.tableRaw;
			}
			else
			{
				List<List<string>> list = new List<List<string>>();
				for (int i = 1; i < this.tableRaw.GetLength(1); i++)
				{
					list.Add(new List<string>());
					for (int j = 0; j < this.tableRaw.GetLength(0); j++)
					{
						list[i - 1].Add(this.tableRaw[j, i]);
					}
				}
				NumericStringComparer comparer = new NumericStringComparer();
				switch (this.sortMode)
				{
				case Window_DebugTable.SortMode.Off:
					throw new Exception();
				case Window_DebugTable.SortMode.Ascending:
					list = list.OrderBy((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				case Window_DebugTable.SortMode.Descending:
					list = list.OrderByDescending((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				}
				this.tableSorted = new string[this.tableRaw.GetLength(0), this.tableRaw.GetLength(1)];
				for (int k = 0; k < this.tableRaw.GetLength(1); k++)
				{
					for (int l = 0; l < this.tableRaw.GetLength(0); l++)
					{
						if (k == 0)
						{
							this.tableSorted[l, k] = this.tableRaw[l, k];
						}
						else
						{
							this.tableSorted[l, k] = list[k - 1][l];
						}
					}
				}
			}
			this.colWidths.Clear();
			for (int m = 0; m < this.tableRaw.GetLength(0); m++)
			{
				float item;
				if (this.colVisible[m])
				{
					float num = 0f;
					for (int n = 0; n < this.tableRaw.GetLength(1); n++)
					{
						float x2 = Text.CalcSize(this.tableRaw[m, n]).x;
						if (x2 > num)
						{
							num = x2;
						}
					}
					item = num + 4f;
				}
				else
				{
					item = 10f;
				}
				this.colWidths.Add(item);
			}
			this.rowHeights.Clear();
			for (int num2 = 0; num2 < this.tableSorted.GetLength(1); num2++)
			{
				float num3 = 0f;
				for (int num4 = 0; num4 < this.tableSorted.GetLength(0); num4++)
				{
					float y = Text.CalcSize(this.tableSorted[num4, num2]).y;
					if (y > num3)
					{
						num3 = y;
					}
				}
				this.rowHeights.Add(num3 + 2f);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000F340 File Offset: 0x0000D540
		private void RefreshSearchFilter()
		{
			this.visibleHeight = 0f;
			for (int i = 1; i < this.tableSorted.GetLength(1); i++)
			{
				bool flag = false;
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (this.quickSearchWidget.filter.Matches(this.tableSorted[j, i]))
					{
						flag = true;
						break;
					}
				}
				if (this.rowsVisible[i] != flag)
				{
					this.scrollPosition = Vector2.zero;
				}
				if (flag)
				{
					this.visibleHeight += this.rowHeights[i];
				}
				this.rowsVisible[i] = flag;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000F3EC File Offset: 0x0000D5EC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			Rect outRect = inRect;
			float num = this.rowHeights[0] + 2f;
			outRect.y = num;
			outRect.yMax -= 40f + num;
			Rect viewRect = new Rect(0f, 0f, this.colWidths.Sum(), this.visibleHeight);
			float num2 = 0f;
			for (int i = 0; i < this.tableSorted.GetLength(0); i++)
			{
				Rect rect = new Rect(num2 - this.scrollPosition.x, 0f, this.colWidths[i], this.rowHeights[0]);
				num2 += this.colWidths[i];
				MouseoverSounds.DoRegion(rect);
				if (!Mouse.IsOver(rect))
				{
					DevGUI.Label(rect, this.tableSorted[i, 0]);
				}
				else
				{
					DevGUI.DrawHighlight(rect);
					if (Event.current.type == EventType.mouseDown)
					{
						if (Event.current.button == 0)
						{
							if (i != this.sortColumn)
							{
								this.sortMode = Window_DebugTable.SortMode.Off;
							}
							switch (this.sortMode)
							{
							case Window_DebugTable.SortMode.Off:
								this.sortMode = Window_DebugTable.SortMode.Descending;
								this.sortColumn = i;
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								break;
							case Window_DebugTable.SortMode.Ascending:
								this.sortMode = Window_DebugTable.SortMode.Off;
								this.sortColumn = -1;
								SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
								break;
							case Window_DebugTable.SortMode.Descending:
								this.sortMode = Window_DebugTable.SortMode.Ascending;
								this.sortColumn = i;
								SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
								break;
							}
							this.BuildTableSorted();
						}
						else if (Event.current.button == 1)
						{
							this.colVisible[i] = !this.colVisible[i];
							SoundDefOf.Crunch.PlayOneShotOnCamera(null);
							this.BuildTableSorted();
						}
						Event.current.Use();
					}
					DevGUI.Label(rect, this.tableSorted[i, 0]);
				}
			}
			Rect rect2 = new Rect
			{
				x = 0f,
				width = 300f,
				y = inRect.yMax - 24f - 4f,
				height = 24f
			};
			this.quickSearchWidget.OnGUI(rect2, new Action(this.RefreshSearchFilter), null);
			if (this.focusFilter)
			{
				this.quickSearchWidget.Focus();
				this.focusFilter = false;
			}
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num3 = 0f;
			for (int j = 1; j < this.tableSorted.GetLength(1); j++)
			{
				if (this.rowsVisible[j])
				{
					if (num3 + this.rowHeights[j] < viewRect.yMin + this.scrollPosition.y || num3 > viewRect.yMax + this.scrollPosition.y)
					{
						num3 += this.rowHeights[j];
					}
					else
					{
						float num4 = 0f;
						bool flag = Mouse.IsOver(new Rect(num4, num3, this.visibleWidth, this.rowHeights[j]));
						for (int k = 0; k < this.tableSorted.GetLength(0); k++)
						{
							Rect rect3 = new Rect(num4, num3, this.colWidths[k], this.rowHeights[j]);
							if (flag)
							{
								DevGUI.DrawHighlight(rect3);
							}
							else if (k % 2 == 0)
							{
								DevGUI.DrawLightHighlight(rect3);
							}
							if (this.colVisible[k])
							{
								DevGUI.Label(rect3, this.tableSorted[k, j]);
							}
							num4 += this.colWidths[k];
						}
						num3 += this.rowHeights[j];
					}
				}
			}
			DevGUI.EndScrollView();
			Text.Font = GameFont.Small;
			if (DevGUI.ButtonText(new Rect(inRect.xMax - 120f, inRect.yMax - 30f, 120f, 30f), "Copy CSV", null))
			{
				this.CopyCSVToClipboard();
				Messages.Message("Copied table data to clipboard in CSV format.", MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000F82C File Offset: 0x0000DA2C
		private void MouseOverHeaderRowCell(Rect cell, int col)
		{
			MouseoverSounds.DoRegion(cell);
			if (Mouse.IsOver(cell) && Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0)
				{
					if (col != this.sortColumn)
					{
						this.sortMode = Window_DebugTable.SortMode.Off;
					}
					switch (this.sortMode)
					{
					case Window_DebugTable.SortMode.Off:
						this.sortMode = Window_DebugTable.SortMode.Descending;
						this.sortColumn = col;
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Ascending:
						this.sortMode = Window_DebugTable.SortMode.Off;
						this.sortColumn = -1;
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Descending:
						this.sortMode = Window_DebugTable.SortMode.Ascending;
						this.sortColumn = col;
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						break;
					}
					this.BuildTableSorted();
				}
				else if (Event.current.button == 1)
				{
					this.colVisible[col] = !this.colVisible[col];
					SoundDefOf.Crunch.PlayOneShotOnCamera(null);
					this.BuildTableSorted();
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000F928 File Offset: 0x0000DB28
		private static void MouseCenterButtonDrag(ref Vector2 scroll)
		{
			if (Event.current.type == EventType.mouseDrag && Event.current.button == 2)
			{
				Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
				Event.current.Use();
				if (currentEventDelta != Vector2.zero)
				{
					scroll += -1f * currentEventDelta;
				}
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000F988 File Offset: 0x0000DB88
		private static bool IsMouseOverRow(Rect cell)
		{
			cell.xMin -= 9999999f;
			cell.xMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000F9B6 File Offset: 0x0000DBB6
		private static bool IsMouseOverCol(Rect cell)
		{
			cell.yMin -= 9999999f;
			cell.yMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		private static void DrawOpaqueLabel(Rect r, string label, bool highlighted = true, bool selectedRow = false, bool selectedCol = false, bool offsetLabel = false)
		{
			if (offsetLabel)
			{
				r.height += 2f;
			}
			Color color = GUI.color;
			GUI.color = DevGUI.WindowBGFillColor;
			GUI.DrawTexture(r, BaseContent.WhiteTex);
			GUI.color = color;
			if (selectedRow)
			{
				DevGUI.DrawHighlightSelected(r);
			}
			if (selectedCol)
			{
				DevGUI.DrawHighlightSelected(r);
			}
			if (highlighted)
			{
				DevGUI.DrawHighlight(r);
			}
			else
			{
				DevGUI.DrawLightHighlight(r);
			}
			if (offsetLabel)
			{
				r.y += 2f;
			}
			DevGUI.Label(r, label);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		private void CopyCSVToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.tableSorted.GetLength(1); i++)
			{
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(",");
					}
					string text = this.tableSorted[j, i] ?? "";
					stringBuilder.Append("\"" + text.Replace("\n", " ") + "\"");
				}
				stringBuilder.Append("\n");
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04000115 RID: 277
		private string[,] tableRaw;

		// Token: 0x04000116 RID: 278
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04000117 RID: 279
		private string[,] tableSorted;

		// Token: 0x04000118 RID: 280
		private List<float> colWidths = new List<float>();

		// Token: 0x04000119 RID: 281
		private List<float> rowHeights = new List<float>();

		// Token: 0x0400011A RID: 282
		private int sortColumn = -1;

		// Token: 0x0400011B RID: 283
		private Window_DebugTable.SortMode sortMode;

		// Token: 0x0400011C RID: 284
		private bool[] colVisible;

		// Token: 0x0400011D RID: 285
		private bool[] rowsVisible;

		// Token: 0x0400011E RID: 286
		private float visibleWidth;

		// Token: 0x0400011F RID: 287
		private float visibleHeight;

		// Token: 0x04000120 RID: 288
		private bool focusFilter;

		// Token: 0x04000121 RID: 289
		private readonly QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04000122 RID: 290
		private const float ColExtraWidth = 4f;

		// Token: 0x04000123 RID: 291
		private const float RowExtraHeight = 2f;

		// Token: 0x04000124 RID: 292
		private const float HiddenColumnWidth = 10f;

		// Token: 0x04000125 RID: 293
		private const float MouseoverOffset = 2f;

		// Token: 0x02002473 RID: 9331
		private enum SortMode
		{
			// Token: 0x040094F2 RID: 38130
			Off,
			// Token: 0x040094F3 RID: 38131
			Ascending,
			// Token: 0x040094F4 RID: 38132
			Descending
		}
	}
}
