using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000857 RID: 2135
	public class MentalBreakWorker_BingingDrug : MentalBreakWorker
	{
		// Token: 0x06004320 RID: 17184 RVA: 0x001A7F3C File Offset: 0x001A613C
		public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			float num = base.CommonalityFor(pawn, moodCaused);
			int num2 = this.BingeableAddictionsCount(pawn);
			if (num2 > 0)
			{
				num *= 1.4f * (float)num2;
			}
			if (moodCaused && pawn.story != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					if (trait.Degree == 1)
					{
						num *= 2.5f;
					}
					else if (trait.Degree == 2)
					{
						num *= 5f;
					}
				}
			}
			return num;
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x001A7FB4 File Offset: 0x001A61B4
		private int BingeableAddictionsCount(Pawn pawn)
		{
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					num++;
				}
			}
			return num;
		}
	}
}
