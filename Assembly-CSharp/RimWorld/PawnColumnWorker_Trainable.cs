using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001DD7 RID: 7639
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		// Token: 0x0600C68B RID: 50827 RVA: 0x0048BAB3 File Offset: 0x00489CB3
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x0600C68C RID: 50828 RVA: 0x0048BAC4 File Offset: 0x00489CC4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null)
			{
				return;
			}
			bool flag;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag || !canTrain.Accepted)
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			TrainingCardUtility.DoTrainableCheckbox(new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f), pawn, this.def.trainable, canTrain, false, true);
		}

		// Token: 0x0600C68D RID: 50829 RVA: 0x0048BB54 File Offset: 0x00489D54
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x0600C68E RID: 50830 RVA: 0x0048BB64 File Offset: 0x00489D64
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C68F RID: 50831 RVA: 0x0048BB79 File Offset: 0x00489D79
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x0600C690 RID: 50832 RVA: 0x0048BB8C File Offset: 0x00489D8C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C691 RID: 50833 RVA: 0x0048BBB0 File Offset: 0x00489DB0
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training == null)
			{
				return int.MinValue;
			}
			if (pawn.training.HasLearned(this.def.trainable))
			{
				return 4;
			}
			bool flag;
			AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag)
			{
				return 0;
			}
			if (!acceptanceReport.Accepted)
			{
				return 1;
			}
			if (!pawn.training.GetWanted(this.def.trainable))
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x0600C692 RID: 50834 RVA: 0x0048BC2C File Offset: 0x00489E2C
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].training != null && !pawnsListForReading[i].training.HasLearned(this.def.trainable))
					{
						bool flag;
						AcceptanceReport acceptanceReport = pawnsListForReading[i].training.CanAssignToTrain(this.def.trainable, out flag);
						if (flag && acceptanceReport.Accepted)
						{
							bool wanted = pawnsListForReading[i].training.GetWanted(this.def.trainable);
							if (Event.current.button == 0)
							{
								if (!wanted)
								{
									pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, true);
								}
							}
							else if (Event.current.button == 1 && wanted)
							{
								pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, false);
							}
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

		// Token: 0x0600C693 RID: 50835 RVA: 0x0048BD7C File Offset: 0x00489F7C
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = base.GetHeaderTip(table);
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "\n" + "CheckboxShiftClickTip".Translate();
			}
			return text;
		}
	}
}
