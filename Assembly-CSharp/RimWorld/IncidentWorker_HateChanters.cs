using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011B2 RID: 4530
	public class IncidentWorker_HateChanters : IncidentWorker
	{
		// Token: 0x06007122 RID: 28962 RVA: 0x00279644 File Offset: 0x00277844
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfHoraxCult;
			parms.sendLetter = false;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkInHateChanters;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms.pawnGroupKind ?? PawnGroupKindDefOf.PsychicRitualSiege, parms, false);
			float num = Faction.OfHoraxCult.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			LordJob_HateChant lordJob_HateChant = new LordJob_HateChant();
			lordJob_HateChant.smallGroup = (list.Count < 20);
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			Lord lord = LordMaker.MakeNewLord(Faction.OfHoraxCult, lordJob_HateChant, (Map)parms.target, list);
			TaggedString text = "HateChantersText".Translate();
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			TaggedString label = "HateChantersLabel".Translate();
			TaggedString taggedString = "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, taggedString, true, true);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, lord.ownedPawns, null, null, null, null, 0, true);
			return true;
		}

		// Token: 0x040047C4 RID: 18372
		public const int SmallGroupSize = 20;
	}
}
