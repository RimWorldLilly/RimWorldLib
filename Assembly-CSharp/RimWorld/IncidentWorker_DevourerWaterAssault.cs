using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011B0 RID: 4528
	public class IncidentWorker_DevourerWaterAssault : IncidentWorker
	{
		// Token: 0x0600711E RID: 28958 RVA: 0x0027940C File Offset: 0x0027760C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EmergeFromWater;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Devourers, parms, false);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Devourers, null);
			defaultPawnGroupMakerParms.points = parms.points * 0.7f;
			if (defaultPawnGroupMakerParms.points < num)
			{
				defaultPawnGroupMakerParms.points = num * 2f;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_DevourerAssault(), parms.target as Map, null);
			parms.lord = lord;
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			if (AnomalyIncidentUtility.IncidentShardChance(defaultPawnGroupMakerParms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x040047C3 RID: 18371
		private const float PointsMultiplier = 0.7f;
	}
}
