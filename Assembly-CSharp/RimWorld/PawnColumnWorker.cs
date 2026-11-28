using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000FB7 RID: 4023
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600662C RID: 26156 RVA: 0x002450F3 File Offset: 0x002432F3
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600662D RID: 26157 RVA: 0x002450FA File Offset: 0x002432FA
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x0600662E RID: 26158 RVA: 0x002450FD File Offset: 0x002432FD
		protected virtual TextAnchor DefaultHeaderAlignment
		{
			get
			{
				return TextAnchor.LowerCenter;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x0600662F RID: 26159 RVA: 0x00245100 File Offset: 0x00243300
		public virtual bool VisibleCurrently
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x00245104 File Offset: 0x00243304
		public virtual void DoHeader(Rect rect, PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				GUI.color = this.DefaultHeaderColor;
				Text.Anchor = this.DefaultHeaderAlignment;
				Rect rect2 = rect;
				rect2.xMin += this.GetHeaderOffsetX(rect);
				Widgets.Label(rect2, this.def.LabelCap.Resolve().Truncate(rect.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			else if (this.def.HeaderIcon != null)
			{
				Vector2 headerIconSize = this.def.HeaderIconSize;
				int num = (int)((rect.width - headerIconSize.x) / 2f);
				GUI.DrawTexture(new Rect(rect.x + (float)num, rect.yMax - headerIconSize.y, headerIconSize.x, headerIconSize.y).ContractedBy(2f), this.def.HeaderIcon);
			}
			if (table.SortingBy == this.def)
			{
				Texture2D texture2D = table.SortingDescending ? PawnColumnWorker.SortingDescendingIcon : PawnColumnWorker.SortingIcon;
				GUI.DrawTexture(new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height), texture2D);
			}
			if (this.def.HeaderInteractable)
			{
				Rect interactableHeaderRect = this.GetInteractableHeaderRect(rect, table);
				if (Mouse.IsOver(interactableHeaderRect))
				{
					Widgets.DrawHighlight(interactableHeaderRect);
					string headerTip = this.GetHeaderTip(table);
					if (!headerTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
					}
				}
				if (Widgets.ButtonInvisible(interactableHeaderRect, true))
				{
					this.HeaderClicked(rect, table);
				}
			}
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x002452D6 File Offset: 0x002434D6
		protected virtual float GetHeaderOffsetX(Rect rect)
		{
			return 0f;
		}

		// Token: 0x06006632 RID: 26162
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x06006633 RID: 26163 RVA: 0x002452DD File Offset: 0x002434DD
		public virtual bool CanGroupWith(Pawn pawn, Pawn other)
		{
			return false;
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x002452E0 File Offset: 0x002434E0
		public virtual void Recache()
		{
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x002452E4 File Offset: 0x002434E4
		public virtual int GetMinWidth(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).x);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.x);
			}
			return 1;
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x0024535E File Offset: 0x0024355E
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x00245365 File Offset: 0x00243565
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x0024536E File Offset: 0x0024356E
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x00245374 File Offset: 0x00243574
		public virtual int GetMinHeaderHeight(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).y);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.y);
			}
			return 0;
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x002453EE File Offset: 0x002435EE
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x002453F4 File Offset: 0x002435F4
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x00245430 File Offset: 0x00243630
		protected virtual void HeaderClicked(Rect headerRect, PawnTable table)
		{
			if (this.def.sortable && !Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(null, false);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					return;
				}
				else if (Event.current.button == 1)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(this.def, true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x0024552C File Offset: 0x0024372C
		protected virtual string GetHeaderTip(PawnTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.headerTip.NullOrEmpty())
			{
				stringBuilder.Append(this.def.headerTip);
			}
			if (this.def.sortable)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ClickToSortByThisColumn".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003E97 RID: 16023
		public PawnColumnDef def;

		// Token: 0x04003E98 RID: 16024
		protected const int DefaultCellHeight = 30;

		// Token: 0x04003E99 RID: 16025
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x04003E9A RID: 16026
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x04003E9B RID: 16027
		private const int IconMargin = 2;
	}
}
