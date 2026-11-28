using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6A RID: 3690
	public class ThoughtWorker_RotStink : ThoughtWorker
	{
		// Token: 0x060061F5 RID: 25077 RVA: 0x002346A8 File Offset: 0x002328A8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			byte b = p.Position.GasDensity(p.Map, GasType.RotStink);
			if (b == 0)
			{
				return ThoughtState.Inactive;
			}
			if (!GasUtility.IsAffectedByExposure(p))
			{
				return ThoughtState.Inactive;
			}
			if (b <= 127)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}

		// Token: 0x0400360D RID: 13837
		private const byte MinorDensity = 127;
	}
}
