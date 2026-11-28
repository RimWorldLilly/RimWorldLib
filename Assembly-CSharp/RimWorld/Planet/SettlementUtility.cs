using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023BA RID: 9146
	public class SettlementUtility
	{
		// Token: 0x0600E3EF RID: 58351 RVA: 0x0054B828 File Offset: 0x00549A28
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			if (faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Settlement settlement = maps[i].info.parent as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E3F0 RID: 58352 RVA: 0x0054B88C File Offset: 0x00549A8C
		public static void Attack(Caravan caravan, Settlement settlement)
		{
			if (!settlement.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					SettlementUtility.AttackNow(caravan, settlement);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			SettlementUtility.AttackNow(caravan, settlement);
		}

		// Token: 0x0600E3F1 RID: 58353 RVA: 0x0054B8E8 File Offset: 0x00549AE8
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterCaravanEnteredEnemyBase".Translate(caravan.Label, settlement.Label.ApplyTag(TagType.Settlement, settlement.Faction.GetUniqueLoadID())).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, caravan.PawnsListForReading, settlement.Faction, null, null, null, 0, true);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
			Find.GoodwillSituationManager.RecalculateAll(true);
		}

		// Token: 0x0600E3F2 RID: 58354 RVA: 0x0054B9DC File Offset: 0x00549BDC
		public static void AffectRelationsOnAttacked(MapParent mapParent, ref TaggedString letterText)
		{
			if (mapParent.Faction != null && mapParent.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = mapParent.Faction.PlayerRelationKind;
				Faction.OfPlayer.TryAffectGoodwillWith(mapParent.Faction, Faction.OfPlayer.GoodwillToMakeHostile(mapParent.Faction), false, false, HistoryEventDefOf.AttackedSettlement, null);
				mapParent.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, mapParent.Faction.PlayerRelationKind, null);
			}
		}
	}
}
