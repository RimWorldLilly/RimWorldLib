using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DA RID: 5338
	public class PawnGroupMaker
	{
		// Token: 0x06008287 RID: 33415 RVA: 0x002ED6DF File Offset: 0x002EB8DF
		public float MinPointsToGenerateAnything(FactionDef faction, PawnGroupMakerParms parms = null)
		{
			return this.kindDef.Worker.MinPointsToGenerateAnything(this, faction, parms);
		}

		// Token: 0x06008288 RID: 33416 RVA: 0x002ED6F4 File Offset: 0x002EB8F4
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x06008289 RID: 33417 RVA: 0x002ED709 File Offset: 0x002EB909
		public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
		}

		// Token: 0x0600828A RID: 33418 RVA: 0x002ED720 File Offset: 0x002EB920
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			if (parms.points > this.maxTotalPoints)
			{
				return false;
			}
			if (this.disallowedStrategies != null && this.disallowedStrategies.Contains(parms.raidStrategy))
			{
				return false;
			}
			if (parms.points < this.MinPointsToGenerateAnything(parms.faction.def, parms))
			{
				return false;
			}
			if (parms.raidStrategy != null)
			{
				RaidStrategyWorker_WithRequiredPawnKinds raidStrategyWorker_WithRequiredPawnKinds = parms.raidStrategy.Worker as RaidStrategyWorker_WithRequiredPawnKinds;
				if (raidStrategyWorker_WithRequiredPawnKinds != null && !raidStrategyWorker_WithRequiredPawnKinds.CanUseWithGroupMaker(this))
				{
					return false;
				}
			}
			return this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x040050D3 RID: 20691
		public PawnGroupKindDef kindDef;

		// Token: 0x040050D4 RID: 20692
		public float commonality = 100f;

		// Token: 0x040050D5 RID: 20693
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x040050D6 RID: 20694
		public float maxTotalPoints = 9999999f;

		// Token: 0x040050D7 RID: 20695
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x040050D8 RID: 20696
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x040050D9 RID: 20697
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x040050DA RID: 20698
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
