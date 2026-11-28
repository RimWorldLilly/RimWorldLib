using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B29 RID: 6953
	public class Medicine : ThingWithComps
	{
		// Token: 0x0600B233 RID: 45619 RVA: 0x003EBDC8 File Offset: 0x003E9FC8
		public static int GetMedicineCountToFullyHeal(Pawn pawn)
		{
			int num = 0;
			int num2 = pawn.health.hediffSet.hediffs.Count + 1;
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].TendableNow(false))
				{
					Medicine.tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
				}
			}
			TendUtility.SortByTendPriority(Medicine.tendableHediffsInTendPriorityOrder);
			int num3 = 0;
			for (;;)
			{
				num++;
				if (num > num2)
				{
					break;
				}
				TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, true, Medicine.tmpHediffs, Medicine.tendableHediffsInTendPriorityOrder);
				if (!Medicine.tmpHediffs.Any<Hediff>())
				{
					goto IL_DF;
				}
				num3++;
				for (int j = 0; j < Medicine.tmpHediffs.Count; j++)
				{
					Medicine.tendableHediffsInTendPriorityOrder.Remove(Medicine.tmpHediffs[j]);
				}
			}
			Log.Error("Too many iterations.");
			IL_DF:
			Medicine.tmpHediffs.Clear();
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			return num3;
		}

		// Token: 0x040066CF RID: 26319
		private static List<Hediff> tendableHediffsInTendPriorityOrder = new List<Hediff>();

		// Token: 0x040066D0 RID: 26320
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
