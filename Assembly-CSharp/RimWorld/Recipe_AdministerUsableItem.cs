using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149D RID: 5277
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x0600815B RID: 33115 RVA: 0x002E2BF8 File Offset: 0x002E0DF8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			CompUsable compUsable;
			if (!ingredients[0].TryGetComp(out compUsable))
			{
				return;
			}
			AcceptanceReport acceptanceReport = compUsable.CanBeUsedBy(pawn, false, true);
			if (acceptanceReport.Accepted)
			{
				compUsable.UsedBy(pawn);
				return;
			}
			if (!string.IsNullOrEmpty(acceptanceReport.Reason))
			{
				compUsable.SendCannotUseMessage(pawn, acceptanceReport.Reason);
			}
		}

		// Token: 0x0600815C RID: 33116 RVA: 0x002E2C50 File Offset: 0x002E0E50
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			Hediff hediff;
			BodyPartRecord bodyPartRecord;
			return pawn != null && HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, Array.Empty<HediffDef>());
		}

		// Token: 0x0600815D RID: 33117 RVA: 0x002E2C84 File Offset: 0x002E0E84
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
