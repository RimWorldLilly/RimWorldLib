using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E67 RID: 3687
	public class ThoughtWorker_InSunlight : ThoughtWorker
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x00234445 File Offset: 0x00232645
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Spawned && p.Position.InSunlight(p.Map))
			{
				return true;
			}
			return ThoughtState.Inactive;
		}
	}
}
