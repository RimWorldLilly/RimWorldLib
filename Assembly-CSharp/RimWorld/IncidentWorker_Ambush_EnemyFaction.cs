using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200115F RID: 4447
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		// Token: 0x06006FBA RID: 28602 RVA: 0x00270E5C File Offset: 0x0026F05C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return base.CanFireNowSub(parms) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x00270E88 File Offset: 0x0026F088
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				string str = "Could not find any valid faction for ";
				IncidentDef def = this.def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " incident.");
				return new List<Pawn>();
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
			return PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
		}

		// Token: 0x06006FBC RID: 28604 RVA: 0x00270EFE File Offset: 0x0026F0FE
		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true, false, false);
		}

		// Token: 0x06006FBD RID: 28605 RVA: 0x00270F14 File Offset: 0x0026F114
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return this.def.letterText.Formatted((caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), parms.faction.def.pawnsPlural, parms.faction.NameColored).Resolve().CapitalizeFirst();
		}
	}
}
