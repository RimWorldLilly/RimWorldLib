using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6C RID: 3692
	public class ThoughtWorker_DeadlifeDust : ThoughtWorker
	{
		// Token: 0x060061F9 RID: 25081 RVA: 0x00234780 File Offset: 0x00232980
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			byte b = p.Position.GasDensity(p.Map, GasType.DeadlifeDust);
			if (b == 0)
			{
				return ThoughtState.Inactive;
			}
			if (b <= 51)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (b <= 127)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(2);
		}

		// Token: 0x0400360F RID: 13839
		private const byte MinorDensity = 51;

		// Token: 0x04003610 RID: 13840
		private const byte MediumDensity = 127;
	}
}
