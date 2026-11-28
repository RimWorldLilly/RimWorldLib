using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011B1 RID: 4529
	public class IncidentWorker_GorehulkAssault : IncidentWorker
	{
		// Token: 0x06007120 RID: 28960 RVA: 0x00279530 File Offset: 0x00277730
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Gorehulks, parms, false);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Gorehulks, null);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), parms.target as Map, list);
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
