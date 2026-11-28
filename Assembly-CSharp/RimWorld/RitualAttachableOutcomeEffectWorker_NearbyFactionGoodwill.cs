using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165C RID: 5724
	public class RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FF1 RID: 36849 RVA: 0x0034E5EC File Offset: 0x0034C7EC
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			List<RitualOutcomePossibility> outcomeChances = jobRitual.Ritual.outcomeEffect.def.outcomeChances;
			int positivityIndex = outcomeChances.MaxBy((RitualOutcomePossibility c) => c.positivityIndex).positivityIndex;
			int positivityIndex2 = (from c in outcomeChances
			where c.positivityIndex >= 0
			select c).MinBy((RitualOutcomePossibility c) => c.positivityIndex).positivityIndex;
			int num = RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill.GoodwillRange.Lerped((float)(outcome.positivityIndex - positivityIndex2) / (float)(positivityIndex - positivityIndex2));
			PlanetTile tile = jobRitual.Map.Tile;
			float num2 = float.PositiveInfinity;
			Settlement settlement = null;
			foreach (WorldObject worldObject in Find.WorldObjects.AllSettlementsOnLayer(tile.Layer))
			{
				Settlement settlement2 = worldObject as Settlement;
				if (settlement2 != null && worldObject.Faction.CanChangeGoodwillFor(Faction.OfPlayer, num))
				{
					float num3 = Find.WorldGrid.ApproxDistanceInTiles(tile, worldObject.Tile);
					if (num3 < num2)
					{
						num2 = num3;
						settlement = settlement2;
					}
				}
			}
			if (settlement != null && settlement.Faction.TryAffectGoodwillWith(Faction.OfPlayer, num, true, true, HistoryEventDefOf.RitualDone, null))
			{
				letterLookTargets = new LookTargets((letterLookTargets.targets ?? new List<GlobalTargetInfo>()).Concat(Gen.YieldSingle<GlobalTargetInfo>(settlement)));
				extraOutcomeDesc = this.def.letterInfoText.Formatted(num.Named("AMOUNT"), settlement.Faction.Named("FACTION"));
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x040057EA RID: 22506
		public static readonly IntRange GoodwillRange = new IntRange(10, 20);
	}
}
