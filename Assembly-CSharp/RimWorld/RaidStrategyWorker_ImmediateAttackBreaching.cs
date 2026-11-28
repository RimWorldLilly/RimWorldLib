using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D18 RID: 3352
	public class RaidStrategyWorker_ImmediateAttackBreaching : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06005E1D RID: 24093 RVA: 0x00227193 File Offset: 0x00225393
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.isGoodBreacher;
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x0022719B File Offset: 0x0022539B
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction)
		{
			if (faction == null || faction != Faction.OfMechanoids)
			{
				return Mathf.RoundToInt(RaidStrategyWorker_ImmediateAttackBreaching.MinGoodBreachersFromPointCurve.Evaluate(pointsTotal));
			}
			return 1;
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x002271BC File Offset: 0x002253BC
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			bool canKidnap = parms.canKidnap;
			bool canTimeoutOrFlee2 = canTimeoutOrFlee;
			bool sappers = false;
			bool canSteal = parms.canSteal;
			return new LordJob_AssaultColony(faction, canKidnap, canTimeoutOrFlee2, sappers, this.useAvoidGridSmart, canSteal, true, false);
		}

		// Token: 0x04003567 RID: 13671
		private static SimpleCurve MinGoodBreachersFromPointCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(200f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 3f),
				true
			},
			{
				new CurvePoint(4000f, 4f),
				true
			}
		};

		// Token: 0x04003568 RID: 13672
		private const int MaxGoodBreachesForMechanoids = 1;

		// Token: 0x04003569 RID: 13673
		protected bool useAvoidGridSmart;
	}
}
