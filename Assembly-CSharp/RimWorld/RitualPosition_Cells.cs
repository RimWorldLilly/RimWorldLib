using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F9 RID: 5881
	public abstract class RitualPosition_Cells : RitualPosition
	{
		// Token: 0x060092F1 RID: 37617
		public abstract void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation);

		// Token: 0x060092F2 RID: 37618 RVA: 0x0035B354 File Offset: 0x00359554
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			RitualPosition_Cells.tmpPotentialCells.Clear();
			Thing thing = spot.GetThingList(ritual.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			Map mapHeld = p.MapHeld;
			CellRect rect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(spot, 0);
			this.FindCells(RitualPosition_Cells.tmpPotentialCells, thing, rect, spot, (thing != null) ? thing.Rotation : Rot4.South);
			CommonRitualCellPredicates.RemoveLeastDesirableRitualCells(RitualPosition_Cells.tmpPotentialCells, spot, mapHeld, p, rect);
			Func<IntVec3, bool> validator = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, rect);
			IntVec3 intVec;
			if (RitualPosition_Cells.tmpPotentialCells.Count != 0)
			{
				intVec = RitualPosition_Cells.tmpPotentialCells[0];
			}
			else
			{
				intVec = this.GetFallbackSpot(rect, spot, p, ritual, validator);
			}
			if (!intVec.IsValid)
			{
				return null;
			}
			Rot4 orientation;
			if (this.faceThing)
			{
				if (this.facing != Rot4.Invalid)
				{
					Log.Error("Only one of faceThing and facing should be specified.");
				}
				orientation = Rot4.FromAngleFlat((thing.Position - intVec).AngleFlat);
			}
			else
			{
				orientation = this.facing;
			}
			return new PawnStagePosition(intVec, thing, orientation, this.highlight);
		}

		// Token: 0x060092F3 RID: 37619 RVA: 0x0035B484 File Offset: 0x00359684
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.facing, "facing", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.faceThing, "faceThing", false, false);
		}

		// Token: 0x04005908 RID: 22792
		public Rot4 facing = Rot4.Invalid;

		// Token: 0x04005909 RID: 22793
		public bool faceThing;

		// Token: 0x0400590A RID: 22794
		private static List<IntVec3> tmpPotentialCells = new List<IntVec3>(8);
	}
}
