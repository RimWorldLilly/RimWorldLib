using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171C RID: 5916
	public class RitualStageAction_DropThing : RitualStageAction
	{
		// Token: 0x060093B2 RID: 37810 RVA: 0x0035E368 File Offset: 0x0035C568
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (Pawn pawn in ritual.assignments.Participants)
			{
				this.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x060093B3 RID: 37811 RVA: 0x0035E3C4 File Offset: 0x0035C5C4
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			Thing thing;
			if (carriedThing != null && carriedThing.def == this.def && carriedThing.stackCount >= this.count && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing, null))
			{
				return;
			}
			int num = Math.Min(pawn.inventory.Count(this.def), this.count);
			pawn.inventory.DropCount(this.def, num, false, false);
		}

		// Token: 0x060093B4 RID: 37812 RVA: 0x0035E445 File Offset: 0x0035C645
		public override void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04005947 RID: 22855
		public ThingDef def;

		// Token: 0x04005948 RID: 22856
		public int count = 1;
	}
}
