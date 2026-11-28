using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6F RID: 2927
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		// Token: 0x06005531 RID: 21809 RVA: 0x001FAC84 File Offset: 0x001F8E84
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle<IntVec3>();
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; j++)
				{
					IntVec3 intVec = JoyGiver_InteractBuildingSitAdjacent.tmpCells[j];
					if (!intVec.IsForbidden(pawn) && pawn.CanReserveSittableOrSpot(intVec, false))
					{
						if (i == 0)
						{
							Building edifice = intVec.GetEdifice(pawn.Map);
							if (edifice == null || !edifice.def.building.isSittable)
							{
								goto IL_95;
							}
						}
						return JobMaker.MakeJob(this.def.jobDef, t, intVec);
					}
					IL_95:;
				}
				if (this.def.requireChair)
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x040032C6 RID: 12998
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
