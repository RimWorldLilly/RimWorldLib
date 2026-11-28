using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBD RID: 7613
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5DD RID: 50653 RVA: 0x004899B7 File Offset: 0x00487BB7
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (PawnColumnWorker_Pregnant.GetPregnantHediff(pawn) == null)
			{
				return null;
			}
			return PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x0600C5DE RID: 50654 RVA: 0x004899C8 File Offset: 0x00487BC8
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x0600C5DF RID: 50655 RVA: 0x004899D0 File Offset: 0x00487BD0
		public static string GetTooltipText(Pawn pawn)
		{
			float gestationProgress = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn).GestationProgress;
			int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
			int numTicks = (int)(gestationProgress * (float)num);
			return "PregnantIconDesc".Translate(numTicks.ToStringTicksToDays("F0"), num.ToStringTicksToDays("F0"));
		}

		// Token: 0x0600C5E0 RID: 50656 RVA: 0x00489A2F File Offset: 0x00487C2F
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x040070E5 RID: 28901
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
