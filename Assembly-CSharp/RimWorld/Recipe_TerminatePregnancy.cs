using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B1 RID: 5297
	public class Recipe_TerminatePregnancy : Recipe_Surgery
	{
		// Token: 0x060081AE RID: 33198 RVA: 0x002E459C File Offset: 0x002E279C
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn != null && PregnancyUtility.GetPregnancyHediff(pawn) != null;
		}

		// Token: 0x060081AF RID: 33199 RVA: 0x002E45CC File Offset: 0x002E27CC
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (PregnancyUtility.GetPregnancyHediff(pawn) == null)
			{
				return;
			}
			if (billDoer != null && base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			if (PregnancyUtility.TryTerminatePregnancy(pawn) && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (billDoer != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
					{
						billDoer,
						pawn
					});
				}
				Messages.Message("MessagePregnancyTerminated".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayerSilentFail))
				{
					base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
				}
			}
		}
	}
}
