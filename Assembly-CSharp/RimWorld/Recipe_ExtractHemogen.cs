using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A0 RID: 5280
	public class Recipe_ExtractHemogen : Recipe_Surgery
	{
		// Token: 0x06008169 RID: 33129 RVA: 0x002E2FF4 File Offset: 0x002E11F4
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return (((pawn != null) ? pawn.genes : null) == null || !pawn.genes.HasActiveGene(GeneDefOf.Hemogenic)) && (pawn == null || pawn.health.CanBleed) && base.AvailableOnNow(thing, part);
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x002E3044 File Offset: 0x002E1244
		public override AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.DevelopmentalStage.Baby())
			{
				return "TooSmall".Translate();
			}
			return base.AvailableReport(thing, part);
		}

		// Token: 0x0600816B RID: 33131 RVA: 0x002E3080 File Offset: 0x002E1280
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.CompletableEver(surgeryTarget) && this.PawnHasEnoughBloodForExtraction(surgeryTarget);
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x002E3094 File Offset: 0x002E1294
		public override void CheckForWarnings(Pawn medPawn)
		{
			base.CheckForWarnings(medPawn);
			if (!this.PawnHasEnoughBloodForExtraction(medPawn))
			{
				Messages.Message("MessageCannotStartHemogenExtraction".Translate(medPawn.Named("PAWN")), medPawn, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x002E30D4 File Offset: 0x002E12D4
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModLister.CheckBiotech("Hemogen extraction"))
			{
				return;
			}
			if (!this.PawnHasEnoughBloodForExtraction(pawn))
			{
				Messages.Message("MessagePawnHadNotEnoughBloodToProduceHemogenPack".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn, null);
			hediff.Severity = 0.45f;
			pawn.health.AddHediff(hediff, null, null, null);
			this.OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -1, HistoryEventDefOf.ExtractedHemogenPack);
			}
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x002E3184 File Offset: 0x002E1384
		protected override void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.HemogenPack, null), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, null, null, null, 1))
			{
				Log.Error("Could not drop hemogen pack near " + pawn.PositionHeld.ToString());
			}
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x002E31E0 File Offset: 0x002E13E0
		private bool PawnHasEnoughBloodForExtraction(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return firstHediffOfDef == null || firstHediffOfDef.Severity < 0.45f;
		}

		// Token: 0x04005067 RID: 20583
		private const float BloodlossSeverity = 0.45f;

		// Token: 0x04005068 RID: 20584
		private const float MinBloodlossForFailure = 0.45f;
	}
}
