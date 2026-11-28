using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001701 RID: 5889
	public class RitualPosition_Copilot : RitualPosition_Cells
	{
		// Token: 0x06009312 RID: 37650 RVA: 0x0035B8AC File Offset: 0x00359AAC
		public override void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation)
		{
			CompPilotConsole compPilotConsole;
			thing.TryGetComp(out compPilotConsole);
			IntVec3 interactionCell = thing.InteractionCell;
			foreach (IntVec3 b in GenAdj.AdjacentCells)
			{
				IntVec3 intVec = interactionCell + b;
				if (ReachabilityImmediate.CanReachImmediate(intVec, thing, thing.Map, PathEndMode.Touch, null) && compPilotConsole.engine.ValidSubstructureAt(intVec))
				{
					cells.Add(intVec);
				}
			}
		}

		// Token: 0x06009313 RID: 37651 RVA: 0x0035B920 File Offset: 0x00359B20
		protected override IntVec3 GetFallbackSpot(CellRect rect, IntVec3 spot, Pawn p, LordJob_Ritual ritual, Func<IntVec3, bool> Validator)
		{
			CompPilotConsole pilotConsole = null;
			Thing thing = ritual.selectedTarget.Thing;
			if (thing == null || !thing.TryGetComp(out pilotConsole))
			{
				return IntVec3.Invalid;
			}
			IntVec3 result;
			if (!CellFinder.TryFindRandomCellNear(spot, p.Map, 20, (IntVec3 c) => Validator(c) && pilotConsole.engine.ValidSubstructureAt(c), out result, -1))
			{
				return IntVec3.Invalid;
			}
			return result;
		}
	}
}
