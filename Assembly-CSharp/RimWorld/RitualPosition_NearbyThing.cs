using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FC RID: 5884
	public abstract class RitualPosition_NearbyThing : RitualPosition
	{
		// Token: 0x060092FC RID: 37628
		public abstract IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual);

		// Token: 0x060092FD RID: 37629
		public abstract IntVec3 PositionForThing(Thing t);

		// Token: 0x060092FE RID: 37630 RVA: 0x0035B59C File Offset: 0x0035979C
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			foreach (Thing thing in this.CandidateThings(ritual))
			{
				if (this.IsUsableThing(thing, spot, ritual))
				{
					return new PawnStagePosition(this.PositionForThing(thing), thing, this.FacingDir(thing), this.highlight);
				}
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x060092FF RID: 37631 RVA: 0x0035B624 File Offset: 0x00359824
		public virtual bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (IntVec3 intVec in (ritualTarget.HasThing ? ritualTarget.Thing.OccupiedRect() : CellRect.CenteredOn(spot, 0)).Cells)
			{
				if (thing.Position.InHorDistOf(intVec, (float)this.maxDistanceToFocus))
				{
					flag = true;
				}
				if (GenSight.LineOfSight(thing.Position, intVec, ritualTarget.Map, true, null, 0, 0))
				{
					flag2 = true;
				}
			}
			return flag && flag2;
		}

		// Token: 0x06009300 RID: 37632 RVA: 0x0035B6CC File Offset: 0x003598CC
		public bool IsUsableThing(Thing thing, IntVec3 spot, LordJob_Ritual ritual)
		{
			return this.IsUsableThing(thing, spot, ritual.selectedTarget);
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x0035B6DC File Offset: 0x003598DC
		public override bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return this.GetCell(spot, p, ritual).cell.IsValid;
		}

		// Token: 0x06009302 RID: 37634 RVA: 0x0035B6F1 File Offset: 0x003598F1
		protected virtual Rot4 FacingDir(Thing t)
		{
			return Rot4.Invalid;
		}

		// Token: 0x0400590D RID: 22797
		public int maxDistanceToFocus = 1;
	}
}
