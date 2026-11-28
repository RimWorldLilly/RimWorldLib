using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149B RID: 5275
	public class Recipe_AddHediff : Recipe_Surgery
	{
		// Token: 0x0600814F RID: 33103 RVA: 0x002E267A File Offset: 0x002E087A
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.IsValidNow(thing, part, false);
		}

		// Token: 0x06008150 RID: 33104 RVA: 0x002E2688 File Offset: 0x002E0888
		public bool IsValidNow(Thing thing, BodyPartRecord part = null, bool ignoreBills = false)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn != null && (part == null || (!pawn.health.WouldDieAfterAddingHediff(this.recipe.addsHediff, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(this.recipe.addsHediff, part, 1f))) && !pawn.health.hediffSet.HasHediff(this.recipe.addsHediff, false) && (ignoreBills || !pawn.BillStack.Bills.Any((Bill b) => b.recipe == this.recipe)) && !pawn.health.hediffSet.hediffs.Any((Hediff x) => !this.recipe.CompatibleWithHediff(x.def));
		}

		// Token: 0x06008151 RID: 33105 RVA: 0x002E2758 File Offset: 0x002E0958
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
			this.OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayerSilentFail))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
		}
	}
}
