using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200088C RID: 2188
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
		// Token: 0x0600441D RID: 17437 RVA: 0x001AB79C File Offset: 0x001A999C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], this.def.drugCategory))
				{
					return true;
				}
				if (this.def.drugCategory == DrugCategory.Hard && AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], DrugCategory.Social))
				{
					return true;
				}
			}
			return false;
		}
	}
}
