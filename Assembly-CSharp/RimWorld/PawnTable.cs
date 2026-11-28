using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDD RID: 7645
	public class PawnTable
	{
		// Token: 0x170024C1 RID: 9409
		// (get) Token: 0x0600C6C2 RID: 50882 RVA: 0x0048CA00 File Offset: 0x0048AC00
		public List<PawnColumnDef> Columns
		{
			get
			{
				this.columns.Clear();
				foreach (PawnColumnDef pawnColumnDef in this.def.columns)
				{
					if (pawnColumnDef.Worker.VisibleCurrently)
					{
						this.columns.Add(pawnColumnDef);
					}
				}
				return this.columns;
			}
		}

		// Token: 0x170024C2 RID: 9410
		// (get) Token: 0x0600C6C3 RID: 50883 RVA: 0x0048CA7C File Offset: 0x0048AC7C
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x170024C3 RID: 9411
		// (get) Token: 0x0600C6C4 RID: 50884 RVA: 0x0048CA84 File Offset: 0x0048AC84
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x170024C4 RID: 9412
		// (get) Token: 0x0600C6C5 RID: 50885 RVA: 0x0048CA96 File Offset: 0x0048AC96
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x170024C5 RID: 9413
		// (get) Token: 0x0600C6C6 RID: 50886 RVA: 0x0048CAA4 File Offset: 0x0048ACA4
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x170024C6 RID: 9414
		// (get) Token: 0x0600C6C7 RID: 50887 RVA: 0x0048CAB2 File Offset: 0x0048ACB2
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x170024C7 RID: 9415
		// (get) Token: 0x0600C6C8 RID: 50888 RVA: 0x0048CAC0 File Offset: 0x0048ACC0
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x0600C6C9 RID: 50889 RVA: 0x0048CAD0 File Offset: 0x0048ACD0
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x0600C6CA RID: 50890 RVA: 0x0048CB54 File Offset: 0x0048AD54
		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type == EventType.layout)
			{
				return;
			}
			this.RecacheIfDirty();
			float num = this.cachedSize.x - 16f;
			List<PawnColumnDef> list = this.Columns;
			int num2 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				int num3;
				if (i == list.Count - 1)
				{
					num3 = (int)(num - (float)num2);
				}
				else
				{
					num3 = (int)this.cachedColumnWidths[i];
				}
				Rect rect = new Rect((float)((int)position.x + num2), (float)((int)position.y), (float)num3, (float)((int)this.cachedHeaderHeight));
				list[i].Worker.DoHeader(rect, this);
				num2 += num3;
			}
			Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			num2 = 0;
			int num4;
			for (int j = 0; j < list.Count; j++)
			{
				num4 = 0;
				PawnColumnDef pawnColumnDef = list[j];
				int num5;
				if (j == list.Count - 1)
				{
					num5 = (int)(num - (float)num2);
				}
				else
				{
					num5 = (int)this.cachedColumnWidths[j];
				}
				for (int k = 0; k < this.cachedPawns.Count; k++)
				{
					GUI.color = PawnTable.BorderColor;
					Widgets.DrawLineHorizontal((float)num2, (float)num4, (float)num5);
					GUI.color = Color.white;
					Rect rect2 = new Rect((float)num2, (float)num4, (float)num5, (float)((int)this.cachedRowHeights[k]));
					Pawn pawn = this.cachedPawns[k];
					bool flag = false;
					if (pawnColumnDef.groupable)
					{
						int num6 = k;
						int num7 = k + 1;
						while (num7 < this.cachedPawns.Count && list[j].Worker.CanGroupWith(this.cachedPawns[k], this.cachedPawns[num7]))
						{
							rect2.yMax += (float)((int)this.cachedRowHeights[num7]);
							num6 = num7;
							flag = true;
							num7++;
						}
						k = num6;
					}
					if ((float)num4 - this.scrollPosition.y + (float)((int)this.cachedRowHeights[k]) >= 0f && (float)num4 - this.scrollPosition.y <= outRect.height)
					{
						list[j].Worker.DoCell(rect2, pawn, this);
						if (pawnColumnDef.groupable && flag)
						{
							GUI.color = PawnTable.BorderColor;
							Widgets.DrawLineVertical(rect2.xMin, rect2.yMin, rect2.height);
							Widgets.DrawLineVertical(rect2.xMax, rect2.yMin, rect2.height);
							GUI.color = Color.white;
						}
					}
					GUI.color = Color.white;
					num4 += (int)rect2.height;
				}
				num2 += num5;
			}
			num4 = 0;
			for (int l = 0; l < this.cachedPawns.Count; l++)
			{
				Rect rect3 = new Rect(0f, (float)num4, viewRect.width, (float)((int)this.cachedRowHeights[l]));
				if (Find.Selector.IsSelected(this.cachedPawns[l]))
				{
					Widgets.DrawHighlight(rect3, 0.6f);
				}
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					this.cachedLookTargets[l].Highlight(true, this.cachedPawns[l].IsColonist, false);
				}
				if (this.cachedPawns[l].Downed)
				{
					GUI.color = new Color(1f, 0f, 0f, 0.5f);
					Widgets.DrawLineHorizontal(0f, rect3.center.y, viewRect.width);
					GUI.color = Color.white;
				}
				num4 += (int)this.cachedRowHeights[l];
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600C6CB RID: 50891 RVA: 0x0048CFA7 File Offset: 0x0048B1A7
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600C6CC RID: 50892 RVA: 0x0048CFB0 File Offset: 0x0048B1B0
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x0600C6CD RID: 50893 RVA: 0x0048CFDC File Offset: 0x0048B1DC
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x0600C6CE RID: 50894 RVA: 0x0048CFF2 File Offset: 0x0048B1F2
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x0600C6CF RID: 50895 RVA: 0x0048D008 File Offset: 0x0048B208
		private void RecacheIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.RecacheColumns();
			this.RecachePawns();
			this.RecacheRowHeights();
			this.cachedHeaderHeight = this.CalculateHeaderHeight();
			this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
			this.RecacheSize();
			this.RecacheColumnWidths();
			this.RecacheLookTargets();
		}

		// Token: 0x0600C6D0 RID: 50896 RVA: 0x0048D064 File Offset: 0x0048B264
		private void RecacheColumns()
		{
			foreach (PawnColumnDef pawnColumnDef in this.def.columns)
			{
				pawnColumnDef.Worker.Recache();
			}
		}

		// Token: 0x0600C6D1 RID: 50897 RVA: 0x0048D0C0 File Offset: 0x0048B2C0
		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			this.cachedPawns = this.LabelSortFunction(this.cachedPawns).ToList<Pawn>();
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.SortStable(new Func<Pawn, Pawn, int>(this.sortByColumn.Worker.Compare));
				}
				else
				{
					this.cachedPawns.SortStable((Pawn a, Pawn b) => this.sortByColumn.Worker.Compare(b, a));
				}
			}
			this.cachedPawns = this.PrimarySortFunction(this.cachedPawns).ToList<Pawn>();
		}

		// Token: 0x0600C6D2 RID: 50898 RVA: 0x0048D167 File Offset: 0x0048B367
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x0600C6D3 RID: 50899 RVA: 0x0048D18E File Offset: 0x0048B38E
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x0600C6D4 RID: 50900 RVA: 0x0048D194 File Offset: 0x0048B394
		private void RecacheColumnWidths()
		{
			float num = this.cachedSize.x - 16f;
			float num2 = 0f;
			this.RecacheColumnWidths_StartWithMinWidths(out num2);
			if (num2 == num)
			{
				return;
			}
			if (num2 > num)
			{
				this.SubtractProportionally(num2 - num, num2);
				return;
			}
			bool flag;
			this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
			if (flag)
			{
				return;
			}
			this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
		}

		// Token: 0x0600C6D5 RID: 50901 RVA: 0x0048D1F0 File Offset: 0x0048B3F0
		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				float minWidth = this.GetMinWidth(list[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x0048D248 File Offset: 0x0048B448
		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(list[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = float.MinValue;
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.columnAtOptimalWidth[j])
					{
						num2 = Mathf.Max(num2, (float)list[j].widthPriority);
					}
				}
				float num3 = 0f;
				for (int k = 0; k < this.cachedColumnWidths.Count; k++)
				{
					if (!this.columnAtOptimalWidth[k] && (float)list[k].widthPriority == num2)
					{
						num3 += this.GetOptimalWidth(list[k]);
					}
				}
				float num4 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				bool flag2 = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtOptimalWidth[l])
					{
						if ((float)list[l].widthPriority != num2)
						{
							flag = true;
						}
						else
						{
							float num5 = num4 * this.GetOptimalWidth(list[l]) / num3;
							float num6 = this.GetOptimalWidth(list[l]) - this.cachedColumnWidths[l];
							if (num5 >= num6)
							{
								num5 = num6;
								this.columnAtOptimalWidth[l] = true;
								flag2 = true;
							}
							else
							{
								flag = true;
							}
							if (num5 > 0f)
							{
								List<float> list2 = this.cachedColumnWidths;
								int index = l;
								list2[index] += num5;
								usedWidth += num5;
							}
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_13;
				}
				if (!flag || !flag2)
				{
					goto IL_1F3;
				}
			}
			Log.Error("Too many iterations.");
			goto IL_1F3;
			Block_13:
			noMoreFreeSpace = true;
			IL_1F3:
			noMoreFreeSpace = false;
		}

		// Token: 0x0600C6D7 RID: 50903 RVA: 0x0048D44C File Offset: 0x0048B64C
		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(list[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = 0f;
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.columnAtMaxWidth[j])
					{
						num2 += Mathf.Max(this.GetOptimalWidth(list[j]), 1f);
					}
				}
				float num3 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (!this.columnAtMaxWidth[k])
					{
						float num4 = num3 * Mathf.Max(this.GetOptimalWidth(list[k]), 1f) / num2;
						float num5 = this.GetMaxWidth(list[k]) - this.cachedColumnWidths[k];
						if (num4 >= num5)
						{
							num4 = num5;
							this.columnAtMaxWidth[k] = true;
						}
						else
						{
							flag = true;
						}
						if (num4 > 0f)
						{
							List<float> list2 = this.cachedColumnWidths;
							int index = k;
							list2[index] += num4;
							usedWidth += num4;
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					return;
				}
				if (!flag)
				{
					goto Block_10;
				}
			}
			Log.Error("Too many iterations.");
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x0600C6D8 RID: 50904 RVA: 0x0048D5E0 File Offset: 0x0048B7E0
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x0600C6D9 RID: 50905 RVA: 0x0048D62C File Offset: 0x0048B82C
		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
				return;
			}
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].ignoreWhenCalculatingOptimalTableSize)
				{
					num += this.GetOptimalWidth(list[i]);
				}
			}
			float num2 = Mathf.Clamp(num + 16f, (float)this.minTableWidth, (float)this.maxTableWidth);
			float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
			num2 = Mathf.Min(num2, (float)UI.screenWidth);
			num3 = Mathf.Min(num3, (float)UI.screenHeight);
			this.cachedSize = new Vector2(num2, num3);
		}

		// Token: 0x0600C6DA RID: 50906 RVA: 0x0048D6EC File Offset: 0x0048B8EC
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		// Token: 0x0600C6DB RID: 50907 RVA: 0x0048D73C File Offset: 0x0048B93C
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x0600C6DC RID: 50908 RVA: 0x0048D788 File Offset: 0x0048B988
		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(list[i]), 1f);
			}
			for (int j = 0; j < list.Count; j++)
			{
				List<float> list2 = this.cachedColumnWidths;
				int index = j;
				list2[index] += toDistribute * Mathf.Max(this.GetOptimalWidth(list[j]), 1f) / num;
			}
		}

		// Token: 0x0600C6DD RID: 50909 RVA: 0x0048D817 File Offset: 0x0048BA17
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x0600C6DE RID: 50910 RVA: 0x0048D830 File Offset: 0x0048BA30
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x0600C6DF RID: 50911 RVA: 0x0048D849 File Offset: 0x0048BA49
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x0600C6E0 RID: 50912 RVA: 0x0048D864 File Offset: 0x0048BA64
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num = Mathf.Max(num, (float)list[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x0600C6E1 RID: 50913 RVA: 0x0048D8AC File Offset: 0x0048BAAC
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			List<PawnColumnDef> list = this.Columns;
			for (int i = 0; i < list.Count; i++)
			{
				num = Mathf.Max(num, (float)list[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x0600C6E2 RID: 50914 RVA: 0x0048D8F4 File Offset: 0x0048BAF4
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x04007105 RID: 28933
		private static readonly Color BorderColor = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04007106 RID: 28934
		private PawnTableDef def;

		// Token: 0x04007107 RID: 28935
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x04007108 RID: 28936
		private int minTableWidth;

		// Token: 0x04007109 RID: 28937
		private int maxTableWidth;

		// Token: 0x0400710A RID: 28938
		private int minTableHeight;

		// Token: 0x0400710B RID: 28939
		private int maxTableHeight;

		// Token: 0x0400710C RID: 28940
		private Vector2 fixedSize;

		// Token: 0x0400710D RID: 28941
		private bool hasFixedSize;

		// Token: 0x0400710E RID: 28942
		private bool dirty;

		// Token: 0x0400710F RID: 28943
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x04007110 RID: 28944
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x04007111 RID: 28945
		private Vector2 scrollPosition;

		// Token: 0x04007112 RID: 28946
		private PawnColumnDef sortByColumn;

		// Token: 0x04007113 RID: 28947
		private bool sortDescending;

		// Token: 0x04007114 RID: 28948
		private Vector2 cachedSize;

		// Token: 0x04007115 RID: 28949
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x04007116 RID: 28950
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x04007117 RID: 28951
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x04007118 RID: 28952
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		// Token: 0x04007119 RID: 28953
		private List<PawnColumnDef> columns = new List<PawnColumnDef>();

		// Token: 0x0400711A RID: 28954
		private float cachedHeaderHeight;

		// Token: 0x0400711B RID: 28955
		private float cachedHeightNoScrollbar;
	}
}
