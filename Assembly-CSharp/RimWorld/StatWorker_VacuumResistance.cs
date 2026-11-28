using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBE RID: 8126
	public class StatWorker_VacuumResistance : StatWorker
	{
		// Token: 0x0600CE69 RID: 52841 RVA: 0x004BF7F0 File Offset: 0x004BD9F0
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.RaceProps.IsFlesh;
		}

		// Token: 0x0600CE6A RID: 52842 RVA: 0x004BF828 File Offset: 0x004BDA28
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn.IsMutant && !pawn.mutant.Def.breathesAir)
			{
				return 1f;
			}
			return base.GetValueUnfinalized(req, applyPostProcess);
		}
	}
}
