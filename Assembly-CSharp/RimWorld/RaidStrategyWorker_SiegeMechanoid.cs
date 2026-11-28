using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1D RID: 3357
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		// Token: 0x06005E2D RID: 24109 RVA: 0x002273EE File Offset: 0x002255EE
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && Faction.OfMechanoids != null && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x00227424 File Offset: 0x00225624
		public override void TryGenerateThreats(IncidentParms parms)
		{
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch(parms.points, parms.target as Map, true, false);
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x00227444 File Offset: 0x00225644
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00227474 File Offset: 0x00225674
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x00227477 File Offset: 0x00225677
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
