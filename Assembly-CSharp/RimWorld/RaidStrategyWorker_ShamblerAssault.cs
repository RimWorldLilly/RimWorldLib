using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1A RID: 3354
	public class RaidStrategyWorker_ShamblerAssault : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x06005E23 RID: 24099 RVA: 0x0022727A File Offset: 0x0022547A
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_ShamblerAssault();
		}
	}
}
