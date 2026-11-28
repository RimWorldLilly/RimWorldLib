using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7F RID: 2943
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x06005577 RID: 21879 RVA: 0x001FC70C File Offset: 0x001FA90C
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001FC73C File Offset: 0x001FA93C
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null) || pawn.Map.weatherManager.curWeather.rainRate > 0.1f || pawn.Map.weatherManager.curWeather.preventSkygaze || (ModsConfig.AnomalyActive && pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness)))
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
