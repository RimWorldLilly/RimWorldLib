using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001612 RID: 5650
	public static class NamePlayerFactionAndSettlementUtility
	{
		// Token: 0x06008CC5 RID: 36037 RVA: 0x00332669 File Offset: 0x00330869
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06008CC6 RID: 36038 RVA: 0x0033267A File Offset: 0x0033087A
		public static bool CanNameSettlementNow(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06008CC7 RID: 36039 RVA: 0x00332693 File Offset: 0x00330893
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06008CC8 RID: 36040 RVA: 0x003326AA File Offset: 0x003308AA
		public static bool CanNameSettlementSoon(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x003326C9 File Offset: 0x003308C9
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 4.3f && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x003326F0 File Offset: 0x003308F0
		private static bool CanNameSettlement(Settlement factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 4.3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x00332758 File Offset: 0x00330958
		private static bool CanNameAnythingNow()
		{
			if (Find.AnyPlayerHomeMap == null || Find.CurrentMap == null || !Find.CurrentMap.IsPlayerHome || Find.GameEnder.gameEnding)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					if (maps[i].mapPawns.FreeColonistsSpawnedCount >= 2)
					{
						flag = true;
					}
					if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x, false)))
					{
						flag2 = true;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x04005641 RID: 22081
		private const float MinDaysPassedToNameFaction = 4.3f;

		// Token: 0x04005642 RID: 22082
		private const float MinDaysPassedToNameSettlement = 4.3f;

		// Token: 0x04005643 RID: 22083
		private const int SoonTicks = 30000;
	}
}
