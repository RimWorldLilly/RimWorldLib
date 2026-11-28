using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBA RID: 7610
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5D1 RID: 50641 RVA: 0x00489762 File Offset: 0x00487962
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x0600C5D2 RID: 50642 RVA: 0x00489778 File Offset: 0x00487978
		protected override string GetIconTip(Pawn pawn)
		{
			int num = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat);
			int num2 = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat * 60f);
			num2 -= num * 60;
			string text = "";
			if (num > 0)
			{
				if (num == 1)
				{
					text += "Period1Year".Translate();
				}
				else
				{
					text += "PeriodYears".Translate(num);
				}
				text += ", ";
			}
			if (num2 > 0)
			{
				if (num2 == 1)
				{
					text += "Period1Day".Translate();
				}
				else
				{
					text += "PeriodDays".Translate(num2);
				}
			}
			else if (num <= 0)
			{
				text += "PeriodHours".Translate(((float)pawn.ageTracker.AgeBiologicalTicks / 2500f).ToString("0.0"));
			}
			return pawn.ageTracker.CurLifeStage.LabelCap + " (" + text.TrimEnd(new char[]
			{
				',',
				' '
			}) + ")";
		}

		// Token: 0x0600C5D3 RID: 50643 RVA: 0x004898C4 File Offset: 0x00487AC4
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalTicks.CompareTo(b.ageTracker.AgeBiologicalTicks);
		}
	}
}
