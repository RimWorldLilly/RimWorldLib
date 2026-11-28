using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022DE RID: 8926
	public static class SettlementProximityGoodwillUtility
	{
		// Token: 0x1700264A RID: 9802
		// (get) Token: 0x0600DC2F RID: 56367 RVA: 0x00520740 File Offset: 0x0051E940
		public static int MaxDist
		{
			get
			{
				return Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Last<CurvePoint>().x);
			}
		}

		// Token: 0x0600DC30 RID: 56368 RVA: 0x00520764 File Offset: 0x0051E964
		public static void CheckSettlementProximityGoodwillChange()
		{
			if (Find.TickManager.TicksGame == 0 || Find.TickManager.TicksGame % 900000 != 0)
			{
				return;
			}
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction == Faction.OfPlayer)
				{
					SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(settlement.Tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, true, false);
				}
			}
			if (!SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Any<Pair<Settlement, int>>())
			{
				return;
			}
			SettlementProximityGoodwillUtility.SortProximityGoodwillOffsets(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			bool flag = false;
			TaggedString text = "LetterFactionBaseProximity".Translate() + "\n\n" + SettlementProximityGoodwillUtility.ProximityGoodwillOffsetsToString(SettlementProximityGoodwillUtility.tmpGoodwillOffsets).ToLineList(" - ", false);
			for (int j = 0; j < allFactionsListForReading.Count; j++)
			{
				Faction faction = allFactionsListForReading[j];
				if (faction != Faction.OfPlayer)
				{
					int num = 0;
					for (int k = 0; k < SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Count; k++)
					{
						if (SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].First.Faction == faction)
						{
							num += SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].Second;
						}
					}
					if (num != 0)
					{
						FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
						Faction.OfPlayer.TryAffectGoodwillWith(faction, num, false, false, HistoryEventDefOf.SettlementProximity, null);
						flag = true;
						faction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, faction.PlayerRelationKind, null);
					}
				}
			}
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseProximity".Translate(), text, LetterDefOf.NegativeEvent, null, 0, true);
			}
		}

		// Token: 0x0600DC31 RID: 56369 RVA: 0x00520920 File Offset: 0x0051EB20
		public static void AppendProximityGoodwillOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
		{
			int maxDist = SettlementProximityGoodwillUtility.MaxDist;
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction != null && settlement.Faction != Faction.OfPlayer && (!ignorePermanentlyHostile || !settlement.Faction.def.permanentEnemy) && (!ignoreIfAlreadyMinGoodwill || settlement.Faction.PlayerGoodwill != -100) && !(tile == settlement.Tile))
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(tile, settlement.Tile, false, maxDist, false);
					if (num != 2147483647)
					{
						float x = (float)num * settlement.Tile.LayerDef.rangeDistanceFactor;
						int num2 = Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Evaluate(x));
						if (num2 != 0)
						{
							outOffsets.Add(new Pair<Settlement, int>(settlement, num2));
						}
					}
				}
			}
		}

		// Token: 0x0600DC32 RID: 56370 RVA: 0x00520A0C File Offset: 0x0051EC0C
		private static void AppendGravshipAttackOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
		{
			Settlement settlement = Find.WorldObjects.SettlementAt(tile);
			if (settlement == null || settlement.Faction.IsPlayer)
			{
				return;
			}
			if (ignorePermanentlyHostile && settlement.Faction.def.permanentEnemy)
			{
				return;
			}
			if (ignoreIfAlreadyMinGoodwill && settlement.Faction.PlayerGoodwill == -100)
			{
				return;
			}
			outOffsets.Add(new Pair<Settlement, int>(settlement, Faction.OfPlayer.GoodwillToMakeHostile(settlement.Faction)));
		}

		// Token: 0x0600DC33 RID: 56371 RVA: 0x00520A7C File Offset: 0x0051EC7C
		public static void SortProximityGoodwillOffsets(List<Pair<Settlement, int>> offsets)
		{
			offsets.SortBy((Pair<Settlement, int> x) => x.First.Faction.loadID, (Pair<Settlement, int> x) => -Mathf.Abs(x.Second));
		}

		// Token: 0x0600DC34 RID: 56372 RVA: 0x00520ACD File Offset: 0x0051ECCD
		public static IEnumerable<string> ProximityGoodwillOffsetsToString(List<Pair<Settlement, int>> offsets)
		{
			SettlementProximityGoodwillUtility.<ProximityGoodwillOffsetsToString>d__7 <ProximityGoodwillOffsetsToString>d__ = new SettlementProximityGoodwillUtility.<ProximityGoodwillOffsetsToString>d__7(-2);
			<ProximityGoodwillOffsetsToString>d__.<>3__offsets = offsets;
			return <ProximityGoodwillOffsetsToString>d__;
		}

		// Token: 0x0600DC35 RID: 56373 RVA: 0x00520ADD File Offset: 0x0051ECDD
		private static IEnumerable<TaggedString> GetConfirmationDescriptions(PlanetTile tile, Building_GravEngine gravEngine = null)
		{
			SettlementProximityGoodwillUtility.<GetConfirmationDescriptions>d__8 <GetConfirmationDescriptions>d__ = new SettlementProximityGoodwillUtility.<GetConfirmationDescriptions>d__8(-2);
			<GetConfirmationDescriptions>d__.<>3__tile = tile;
			<GetConfirmationDescriptions>d__.<>3__gravEngine = gravEngine;
			return <GetConfirmationDescriptions>d__;
		}

		// Token: 0x0600DC36 RID: 56374 RVA: 0x00520AF4 File Offset: 0x0051ECF4
		private static IEnumerable<string> GetConfirmationEffects(PlanetTile tile, bool gravship)
		{
			SettlementProximityGoodwillUtility.<GetConfirmationEffects>d__9 <GetConfirmationEffects>d__ = new SettlementProximityGoodwillUtility.<GetConfirmationEffects>d__9(-2);
			<GetConfirmationEffects>d__.<>3__tile = tile;
			<GetConfirmationEffects>d__.<>3__gravship = gravship;
			return <GetConfirmationEffects>d__;
		}

		// Token: 0x0600DC37 RID: 56375 RVA: 0x00520B0C File Offset: 0x0051ED0C
		public static void CheckConfirmSettle(PlanetTile tile, Action settleAction, Action cancelAction = null, Building_GravEngine gravEngine = null)
		{
			IEnumerable<TaggedString> confirmationDescriptions = SettlementProximityGoodwillUtility.GetConfirmationDescriptions(tile, gravEngine);
			if (confirmationDescriptions.Any<TaggedString>())
			{
				TaggedString taggedString = new TaggedString("");
				foreach (TaggedString t in confirmationDescriptions)
				{
					if (!taggedString.NullOrEmpty())
					{
						taggedString += "\n\n";
					}
					taggedString += t;
				}
				if (gravEngine != null)
				{
					taggedString += "\n\n" + "ConfirmLand".Translate();
				}
				else
				{
					taggedString += "\n\n" + "ConfirmSettle".Translate();
				}
				IEnumerable<string> confirmationEffects = SettlementProximityGoodwillUtility.GetConfirmationEffects(tile, gravEngine != null);
				if (confirmationEffects.Any<string>())
				{
					taggedString += "\n\n" + confirmationEffects.ToLineList(" - ", false);
				}
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, settleAction, cancelAction, false, null, WindowLayer.Dialog));
				return;
			}
			settleAction();
		}

		// Token: 0x04008D36 RID: 36150
		private static readonly List<Pair<Settlement, int>> tmpGoodwillOffsets = new List<Pair<Settlement, int>>();
	}
}
