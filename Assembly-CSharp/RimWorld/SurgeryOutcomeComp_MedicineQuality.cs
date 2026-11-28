using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201A RID: 8218
	public class SurgeryOutcomeComp_MedicineQuality : SurgeryOutcomeComp_Curve
	{
		// Token: 0x0600D001 RID: 53249 RVA: 0x004C793C File Offset: 0x004C5B3C
		protected override float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			int num = 0;
			float num2 = 0f;
			Bill_Medical bill_Medical = bill as Bill_Medical;
			if (bill_Medical != null && bill_Medical.consumedMedicine != null)
			{
				foreach (KeyValuePair<ThingDef, int> keyValuePair in bill_Medical.consumedMedicine)
				{
					num += keyValuePair.Value;
					num2 += keyValuePair.Key.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * (float)keyValuePair.Value;
				}
			}
			if (num == 0)
			{
				return 1f;
			}
			return num2 / (float)num;
		}
	}
}
