using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001DA8 RID: 7592
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x0600C55D RID: 50525 RVA: 0x004887FB File Offset: 0x004869FB
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x0600C55E RID: 50526 RVA: 0x0048880C File Offset: 0x00486A0C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.HasCheckbox(pawn))
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			bool value = this.GetValue(pawn);
			bool flag = value;
			Widgets.Checkbox(vector, ref value, 24f, false, this.def.paintable, null, null);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(pawn);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
			if (value != flag)
			{
				this.SetValue(pawn, value, table);
			}
		}

		// Token: 0x0600C55F RID: 50527 RVA: 0x004888D7 File Offset: 0x00486AD7
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x0600C560 RID: 50528 RVA: 0x004888E7 File Offset: 0x00486AE7
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C561 RID: 50529 RVA: 0x004888FC File Offset: 0x00486AFC
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x0600C562 RID: 50530 RVA: 0x0048890C File Offset: 0x00486B0C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x0048892F File Offset: 0x00486B2F
		private int GetValueToCompare(Pawn pawn)
		{
			if (!this.HasCheckbox(pawn))
			{
				return 0;
			}
			if (!this.GetValue(pawn))
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600C564 RID: 50532 RVA: 0x00488948 File Offset: 0x00486B48
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600C565 RID: 50533 RVA: 0x0048894B File Offset: 0x00486B4B
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600C566 RID: 50534
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x0600C567 RID: 50535
		protected abstract void SetValue(Pawn pawn, bool value, PawnTable table);

		// Token: 0x0600C568 RID: 50536 RVA: 0x00488950 File Offset: 0x00486B50
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (this.HasCheckbox(pawnsListForReading[i]))
					{
						if (Event.current.button == 0)
						{
							if (!this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], true, table);
							}
						}
						else if (Event.current.button == 1 && this.GetValue(pawnsListForReading[i]))
						{
							this.SetValue(pawnsListForReading[i], false, table);
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x0600C569 RID: 50537 RVA: 0x00488A24 File Offset: 0x00486C24
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = base.GetHeaderTip(table);
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "\n" + "CheckboxShiftClickTip".Translate();
			}
			return text;
		}

		// Token: 0x040070DB RID: 28891
		public const int HorizontalPadding = 2;
	}
}
