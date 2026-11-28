using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B9 RID: 4537
	public class IncidentWorker_CaravanArrivalTributeCollector : IncidentWorker_TraderCaravanArrival
	{
		// Token: 0x0600715D RID: 29021 RVA: 0x0027B594 File Offset: 0x00279794
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			if (Faction.OfEmpire == null)
			{
				return false;
			}
			Map map = (Map)parms.target;
			parms.faction = Faction.OfEmpire;
			parms.traderKind = (from t in DefDatabase<TraderKindDef>.AllDefsListForReading
			where t.category == "TributeCollector"
			select t).RandomElementByWeight((TraderKindDef t) => this.TraderKindCommonality(t, map, parms.faction));
			return true;
		}

		// Token: 0x0600715E RID: 29022 RVA: 0x0027B63A File Offset: 0x0027983A
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Faction.OfEmpire != null && this.FactionCanBeGroupSource(Faction.OfEmpire, parms, false);
		}

		// Token: 0x0600715F RID: 29023 RVA: 0x0027B65B File Offset: 0x0027985B
		protected override float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x06007160 RID: 29024 RVA: 0x0027B664 File Offset: 0x00279864
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTributeCollectorArrival".Translate().CapitalizeFirst();
			TaggedString taggedString = "LetterTributeCollectorArrival".Translate(parms.faction.Named("FACTION")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x040047CC RID: 18380
		public const string TributeCollectorTraderKindCategory = "TributeCollector";
	}
}
