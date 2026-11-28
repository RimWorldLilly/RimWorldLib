using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001700 RID: 5888
	public class RitualPosition_AnimaLinkingSpot : RitualPosition
	{
		// Token: 0x06009310 RID: 37648 RVA: 0x0035B7E4 File Offset: 0x003599E4
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			IntVec3 intVec;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, CellRect.CenteredOn(spot, 0), 2f, 3f, p.Map, out intVec, null, null))
			{
				return new PawnStagePosition(intVec, null, Rot4.FromAngleFlat((spot - intVec).AngleFlat), this.highlight);
			}
			Thing thing = ritual.selectedTarget.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			LocalTargetInfo localTargetInfo;
			if (compPsylinkable != null && compPsylinkable.TryFindLinkSpot(p, out localTargetInfo))
			{
				Rot4 orientation = Rot4.FromAngleFlat((spot - localTargetInfo.Cell).AngleFlat);
				return new PawnStagePosition(localTargetInfo.Cell, null, orientation, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}
	}
}
