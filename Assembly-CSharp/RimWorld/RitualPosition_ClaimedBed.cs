using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F7 RID: 5879
	public class RitualPosition_ClaimedBed : RitualPosition
	{
		// Token: 0x060092EA RID: 37610 RVA: 0x0035B0DC File Offset: 0x003592DC
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Pawn pawn = (this.ofPawn == null) ? p : ritual.assignments.FirstAssignedPawn(this.ofPawn);
			if (pawn.ownership != null && pawn.ownership.OwnedBed != null)
			{
				return new PawnStagePosition(pawn.ownership.OwnedBed.Position, null, Rot4.Invalid, this.highlight);
			}
			if (this.tryClaimIfNoneClaimed)
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(pawn);
				if (building_Bed != null)
				{
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					return new PawnStagePosition(building_Bed.Position, null, Rot4.Invalid, this.highlight);
				}
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x060092EB RID: 37611 RVA: 0x0035B18A File Offset: 0x0035938A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.ofPawn, "ofPawn", null, false);
			Scribe_Values.Look<bool>(ref this.tryClaimIfNoneClaimed, "tryClaimIfNoneClaimed", false, false);
		}

		// Token: 0x04005903 RID: 22787
		[NoTranslate]
		public string ofPawn;

		// Token: 0x04005904 RID: 22788
		public bool tryClaimIfNoneClaimed;
	}
}
