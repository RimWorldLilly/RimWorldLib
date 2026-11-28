using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A2 RID: 5282
	public class Recipe_GhoulInfusion : Recipe_Surgery
	{
		// Token: 0x06008175 RID: 33141 RVA: 0x002E33DC File Offset: 0x002E15DC
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.IsMutant && pawn.ageTracker.Adult && base.AvailableOnNow(thing, part);
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x002E3416 File Offset: 0x002E1616
		public override TaggedString GetConfirmation(Pawn pawn)
		{
			if (pawn.IsColonist)
			{
				return "GhoulConfirmation".Translate(pawn.Named("PAWN"));
			}
			return base.GetConfirmation(pawn);
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x002E3440 File Offset: 0x002E1640
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			bool flag = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			Faction homeFaction = pawn.HomeFaction;
			MutantUtility.SetPawnAsMutantInstantly(pawn, MutantDefOf.Ghoul, RotStage.Fresh);
			if (pawn.Faction != Faction.OfPlayer)
			{
				pawn.SetFaction(Faction.OfPlayer, null);
			}
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			if (flag)
			{
				base.ReportViolation(pawn, billDoer, homeFaction, -50, HistoryEventDefOf.GhoulInfusionPerformed);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ColonyGhouls, OpportunityType.GoodToKnow);
		}
	}
}
