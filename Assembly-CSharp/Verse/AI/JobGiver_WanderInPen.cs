using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008FB RID: 2299
	public class JobGiver_WanderInPen : JobGiver_Wander
	{
		// Token: 0x06004648 RID: 17992 RVA: 0x001B8F41 File Offset: 0x001B7141
		public JobGiver_WanderInPen()
		{
			this.wanderRadius = 10f;
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x001B8F54 File Offset: 0x001B7154
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				CompAnimalPenMarker compAnimalPenMarker = AnimalPenUtility.ClosestSuitablePen(pawn, true);
				if (compAnimalPenMarker != null)
				{
					return compAnimalPenMarker.parent.Position;
				}
			}
			return pawn.Position;
		}
	}
}
