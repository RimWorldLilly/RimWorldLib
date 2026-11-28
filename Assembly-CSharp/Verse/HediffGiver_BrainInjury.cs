using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003FE RID: 1022
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B7944 File Offset: 0x000B5B44
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			if (!(hediff is Hediff_Injury))
			{
				return false;
			}
			if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				return false;
			}
			if (pawn.IsSubhuman)
			{
				return false;
			}
			float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
			if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
			{
				if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040017D0 RID: 6096
		public float chancePerDamagePct;

		// Token: 0x040017D1 RID: 6097
		public string letterLabel;

		// Token: 0x040017D2 RID: 6098
		public string letter;
	}
}
