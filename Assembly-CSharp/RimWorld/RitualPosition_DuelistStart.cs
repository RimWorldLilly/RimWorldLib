using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F8 RID: 5880
	public class RitualPosition_DuelistStart : RitualPosition
	{
		// Token: 0x060092ED RID: 37613 RVA: 0x0035B1C0 File Offset: 0x003593C0
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			int num = 0;
			for (int i = 0; i < RitualPosition_DuelistStart.Rotations.Length; i++)
			{
				Rot4 rot = RitualPosition_DuelistStart.Rotations[i];
				IntVec3 b = rot.FacingCell * this.distFromTarget;
				IntVec3 intVec = spot + b;
				if (intVec.InBounds(p.Map) && intVec.Standable(p.Map))
				{
					if (num == this.duelistIndex)
					{
						return new PawnStagePosition(intVec, null, Rot4.FromIntVec3(rot.FacingCell), this.highlight);
					}
					num++;
				}
			}
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect spectateRect = (thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(spot);
			IntVec3 cell;
			if (SpectatorCellFinder.TryFindCircleSpectatorCellFor(p, spectateRect, 1f, (float)(this.distFromTarget * 2), p.Map, out cell, null, null))
			{
				return new PawnStagePosition(cell, null, Rot4.Invalid, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x060092EE RID: 37614 RVA: 0x0035B2E2 File Offset: 0x003594E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.distFromTarget, "distFromTarget", 0, false);
			Scribe_Values.Look<int>(ref this.duelistIndex, "duelistIndex", 0, false);
		}

		// Token: 0x04005905 RID: 22789
		public int distFromTarget;

		// Token: 0x04005906 RID: 22790
		public int duelistIndex;

		// Token: 0x04005907 RID: 22791
		private static readonly Rot4[] Rotations = new Rot4[]
		{
			Rot4.West,
			Rot4.East,
			Rot4.North,
			Rot4.South
		};
	}
}
