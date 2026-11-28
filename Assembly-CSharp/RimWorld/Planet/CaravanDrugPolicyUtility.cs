using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002383 RID: 9091
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x0600E20B RID: 57867 RVA: 0x0053D79D File Offset: 0x0053B99D
		public static void CheckTakeScheduledDrugs(Caravan caravan, int delta)
		{
			if (caravan.IsHashIntervalTick(120, delta))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		// Token: 0x0600E20C RID: 57868 RVA: 0x0053D7B0 File Offset: 0x0053B9B0
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x0600E20D RID: 57869 RVA: 0x0053D7E4 File Offset: 0x0053B9E4
		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs == null || pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			if (currentPolicy == null)
			{
				return;
			}
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				Thing drug;
				Pawn drugOwner;
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug) && CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
				{
					caravan.needs.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		// Token: 0x04009064 RID: 36964
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
