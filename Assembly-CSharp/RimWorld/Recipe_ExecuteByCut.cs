using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A8 RID: 5288
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x0600818B RID: 33163 RVA: 0x002E3B33 File Offset: 0x002E1D33
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -100, null);
			}
			ExecutionUtility.DoExecutionByCut(billDoer, pawn, 8, true);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericHumane);
		}
	}
}
