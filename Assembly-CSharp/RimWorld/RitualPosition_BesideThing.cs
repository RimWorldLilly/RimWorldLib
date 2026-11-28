using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F2 RID: 5874
	public class RitualPosition_BesideThing : RitualPosition
	{
		// Token: 0x060092DD RID: 37597 RVA: 0x0035ABFC File Offset: 0x00358DFC
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect cellRect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(spot, 0);
			Map mapHeld = p.MapHeld;
			IntVec3 intVec = (thing != null) ? IntVec3.West.RotatedBy(thing.Rotation) : IntVec3.West;
			CellRect cellRect2 = new CellRect(cellRect.minX + intVec.x, cellRect.minZ + intVec.z, 1, cellRect.Height);
			CellRect cellRect3 = new CellRect(cellRect.maxX - intVec.z, cellRect.minZ - intVec.z, 1, cellRect.Height);
			Func<IntVec3, bool> func = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, cellRect);
			IntVec3 cell = IntVec3.Invalid;
			for (int i = 0; i < 16; i++)
			{
				IntVec3 randomCell;
				if (Rand.Chance(0.5f))
				{
					randomCell = cellRect2.RandomCell;
				}
				else
				{
					randomCell = cellRect3.RandomCell;
				}
				if (func(randomCell))
				{
					cell = randomCell;
					break;
				}
			}
			if (!cell.IsValid)
			{
				cell = this.GetFallbackSpot(cellRect, spot, p, ritual, func);
			}
			return new PawnStagePosition(cell, thing, this.faceThing ? Rot4.FromIntVec3(intVec.Inverse()) : Rot4.Invalid, this.highlight);
		}

		// Token: 0x060092DE RID: 37598 RVA: 0x0035AD59 File Offset: 0x00358F59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.faceThing, "faceThing", false, false);
		}

		// Token: 0x040058FF RID: 22783
		public bool faceThing;
	}
}
