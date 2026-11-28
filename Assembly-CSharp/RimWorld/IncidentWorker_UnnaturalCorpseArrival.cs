using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A3 RID: 4515
	public class IncidentWorker_UnnaturalCorpseArrival : IncidentWorker
	{
		// Token: 0x060070E6 RID: 28902 RVA: 0x002780F4 File Offset: 0x002762F4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map == null)
			{
				return false;
			}
			Pawn pawn;
			if (!QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				return false;
			}
			UnnaturalCorpse unnaturalCorpse = AnomalyUtility.MakeUnnaturalCorpse(pawn);
			Find.Anomaly.RegisterUnnaturalCorpse(pawn, unnaturalCorpse);
			IntVec3 loc;
			RCellFinder.TryFindRandomSpotJustOutsideColony(IntVec3.Invalid, map, out loc);
			GenSpawn.Spawn(unnaturalCorpse, loc, map, WipeMode.Vanish);
			EffecterDefOf.Skip_EntryNoDelay.Spawn(unnaturalCorpse, map, 1f).Cleanup();
			base.SendStandardLetter("UnnaturalCorpseArrivalLabel".Translate(), "UnnaturalCorpseArrivalText".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatSmall, parms, unnaturalCorpse, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x002781A2 File Offset: 0x002763A2
		protected bool ValidatePawn(Pawn pawn)
		{
			return (pawn.IsColonist || pawn.IsSlaveOfColony) && pawn.RaceProps.unnaturalCorpseDef != null && !Find.Anomaly.PawnHasUnnaturalCorpse(pawn);
		}
	}
}
