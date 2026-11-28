using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011AF RID: 4527
	public class IncidentWorker_DevourerAssault : IncidentWorker
	{
		// Token: 0x0600711C RID: 28956 RVA: 0x002792F8 File Offset: 0x002774F8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Devourers, parms, false);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Devourers, null);
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
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_DevourerAssault(), parms.target as Map, list);
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
