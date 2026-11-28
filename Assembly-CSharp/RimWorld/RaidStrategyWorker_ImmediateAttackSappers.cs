using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D1F RID: 3359
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06005E3C RID: 24124 RVA: 0x002276DE File Offset: 0x002258DE
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.canBeSapper;
		}

		// Token: 0x06005E3D RID: 24125 RVA: 0x002276E6 File Offset: 0x002258E6
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
		{
			return 1;
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x002276E9 File Offset: 0x002258E9
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return (otherPawns.Count != 0 || SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p)) && (!p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p));
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x00227728 File Offset: 0x00225928
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			return new LordJob_AssaultColony(faction, parms.canKidnap, canTimeoutOrFlee, true, true, parms.canSteal, false, false);
		}
	}
}
