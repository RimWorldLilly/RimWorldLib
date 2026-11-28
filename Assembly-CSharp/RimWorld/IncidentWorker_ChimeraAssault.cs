using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011AE RID: 4526
	public class IncidentWorker_ChimeraAssault : IncidentWorker
	{
		// Token: 0x0600711A RID: 28954 RVA: 0x002791F4 File Offset: 0x002773F4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Chimeras, parms, false);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Chimeras, null);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ChimeraAssault(), parms.target as Map, list);
			base.SendStandardLetter("ChimeraAssaultLabel".Translate(), "ChimeraAssaultText".Translate(), LetterDefOf.ThreatSmall, parms, list, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
