using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB7 RID: 7607
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x170024B6 RID: 9398
		// (get) Token: 0x0600C5B8 RID: 50616 RVA: 0x004894B7 File Offset: 0x004876B7
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x170024B7 RID: 9399
		// (get) Token: 0x0600C5B9 RID: 50617 RVA: 0x004894BB File Offset: 0x004876BB
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600C5BA RID: 50618 RVA: 0x004894C0 File Offset: 0x004876C0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (iconFor != null)
			{
				Vector2 iconSize = this.GetIconSize(pawn);
				int num = (int)((rect.width - iconSize.x) / 2f);
				int num2 = Mathf.Max((int)((30f - iconSize.y) / 2f), 0);
				Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, iconSize.x, iconSize.y);
				GUI.color = this.GetIconColor(pawn);
				GUI.DrawTexture(rect2.ContractedBy((float)this.Padding), iconFor);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					string iconTip = this.GetIconTip(pawn);
					if (!iconTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, iconTip);
					}
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					this.ClickedIcon(pawn);
				}
				if (Mouse.IsOver(rect2) && Input.GetMouseButton(0))
				{
					this.PaintedIcon(pawn);
				}
			}
		}

		// Token: 0x0600C5BB RID: 50619 RVA: 0x004895BC File Offset: 0x004877BC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x0600C5BC RID: 50620 RVA: 0x004895D0 File Offset: 0x004877D0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C5BD RID: 50621 RVA: 0x004895E5 File Offset: 0x004877E5
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x0600C5BE RID: 50622 RVA: 0x00489604 File Offset: 0x00487804
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C5BF RID: 50623 RVA: 0x00489628 File Offset: 0x00487828
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		// Token: 0x0600C5C0 RID: 50624
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x0600C5C1 RID: 50625 RVA: 0x00489652 File Offset: 0x00487852
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x00489655 File Offset: 0x00487855
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x0600C5C3 RID: 50627 RVA: 0x0048965C File Offset: 0x0048785C
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x0600C5C4 RID: 50628 RVA: 0x0048965E File Offset: 0x0048785E
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x0600C5C5 RID: 50629 RVA: 0x00489660 File Offset: 0x00487860
		protected virtual Vector2 GetIconSize(Pawn pawn)
		{
			if (this.GetIconFor(pawn) == null)
			{
				return Vector2.zero;
			}
			return new Vector2((float)this.Width, (float)this.Width);
		}
	}
}
