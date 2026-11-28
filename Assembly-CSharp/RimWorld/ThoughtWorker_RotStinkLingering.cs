using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6B RID: 3691
	public class ThoughtWorker_RotStinkLingering : ThoughtWorker
	{
		// Token: 0x060061F7 RID: 25079 RVA: 0x00234708 File Offset: 0x00232908
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned || p.mindState == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.Map.gasGrid.DensityAt(p.Position, GasType.RotStink) > 0)
			{
				return ThoughtState.Inactive;
			}
			if (Find.TickManager.TicksGame > p.mindState.lastRotStinkTick + 1800)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x0400360E RID: 13838
		private const int LingerDuration = 1800;
	}
}
