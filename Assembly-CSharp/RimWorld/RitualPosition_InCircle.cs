using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016F6 RID: 5878
	public class RitualPosition_InCircle : RitualPosition
	{
		// Token: 0x060092E7 RID: 37607 RVA: 0x0035AF20 File Offset: 0x00359120
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			if (this.preferredRotation != null)
			{
				for (int i = this.distRange.max; i >= this.distRange.min; i--)
				{
					IntVec3 intVec = spot + this.preferredRotation.Value.FacingCell * i;
					if (intVec.InBounds(p.Map) && intVec.Standable(p.Map) && GenSight.LineOfSight(spot, intVec, ritual.Map, true, null, 0, 0) && WanderUtility.InSameRoom(intVec, spot, ritual.Map))
					{
						return new PawnStagePosition(intVec, null, this.preferredRotation.Value.Opposite, this.highlight);
					}
				}
			}
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect spectateRect = (thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(spot);
			IntVec3 cell;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, spectateRect, (float)this.distRange.min, (float)this.distRange.max, p.Map, out cell, null, null))
			{
				return new PawnStagePosition(cell, null, Rot4.Invalid, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x060092E8 RID: 37608 RVA: 0x0035B08C File Offset: 0x0035928C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.distRange, "distRange", default(IntRange), false);
			Scribe_Values.Look<Rot4?>(ref this.preferredRotation, "preferredRotation", null, false);
		}

		// Token: 0x04005901 RID: 22785
		public Rot4? preferredRotation;

		// Token: 0x04005902 RID: 22786
		public IntRange distRange;
	}
}
