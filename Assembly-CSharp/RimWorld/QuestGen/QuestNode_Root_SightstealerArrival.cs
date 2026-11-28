using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002294 RID: 8852
	public class QuestNode_Root_SightstealerArrival : QuestNode
	{
		// Token: 0x17002623 RID: 9763
		// (get) Token: 0x0600DA98 RID: 55960 RVA: 0x00515B77 File Offset: 0x00513D77
		private string WaveTag
		{
			get
			{
				return QuestGenUtility.HardcodedTargetQuestTagWithQuestID("Sightstealer") + ".wave";
			}
		}

		// Token: 0x0600DA99 RID: 55961 RVA: 0x00515B90 File Offset: 0x00513D90
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null, false) != null;
		}

		// Token: 0x0600DA9A RID: 55962 RVA: 0x00515BB0 File Offset: 0x00513DB0
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Sightstealers, null);
			float num2 = slate.Get<float>("points", 0f, false);
			string firstDelayCompleteSignal = QuestGen.GenerateNewSignal("FirstDelayCompleted", true);
			string secondDelayCompleteSignal = QuestGen.GenerateNewSignal("SecondDelayCompleted", true);
			string text = QuestGenUtility.QuestTagSignal(this.WaveTag + "1", "AllEnemiesDefeated");
			string text2 = QuestGenUtility.QuestTagSignal(this.WaveTag + "2", "AllEnemiesDefeated");
			string inSignal = QuestGenUtility.QuestTagSignal(this.WaveTag + "3", "AllEnemiesDefeated");
			string firstHowlDelaySignal = QuestGen.GenerateNewSignal("FirstHowlDelayed", true);
			string secondHowlDelaySignal = QuestGen.GenerateNewSignal("SecondHowlDelayed", true);
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved");
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			List<Pawn> list;
			PawnsArrivalModeDef edgeWalkInDistributed;
			if (num2 < 400f)
			{
				float points = Mathf.Max(num2 * QuestNode_Root_SightstealerArrival.SecondAttackStrength.RandomInRange, num);
				list = this.GenerateSightstealers(map, points, false);
				Quest quest6 = quest;
				IEnumerable<Pawn> pawns = list;
				string inSignal3 = null;
				edgeWalkInDistributed = PawnsArrivalModeDefOf.EdgeWalkInDistributed;
				quest6.PawnsArrive(pawns, inSignal3, map.Parent, edgeWalkInDistributed, false, null, null, null, null, null, false, false, false);
				quest.AddPart(new QuestPart_SightstealerWave(null, map.Parent, list, this.WaveTag + "1"));
				quest.End(QuestEndOutcome.Success, 0, null, text, QuestPart.SignalListenMode.OngoingOnly, false, false);
				return;
			}
			list = this.GenerateSightstealers(map, num, false);
			quest.PawnsArrive(list, null, map.Parent, null, false, null, null, null, null, null, false, false, false);
			quest.AddPart(new QuestPart_SightstealerWave(null, map.Parent, list, this.WaveTag + "1"));
			quest.Delay(((float)QuestNode_Root_SightstealerArrival.HowlDelaySecondsRange.RandomInRange).SecondsToTicks(), delegate
			{
				quest.SignalPass(null, null, firstHowlDelaySignal);
			}, text, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Wave 2 howl delay";
			Quest quest2 = quest;
			LetterDef negativeEvent = LetterDefOf.NegativeEvent;
			string text3 = "LetterLabelSightstealerHowl".Translate();
			string text4 = "LetterSightstealerHowl".Translate();
			quest2.Letter(negativeEvent, firstHowlDelaySignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text4, null, text3, null, null);
			quest.PlayOneShotOnCamera(SoundDefOf.Sightstealer_DistantHowl, firstHowlDelaySignal);
			quest.Delay((int)(QuestNode_Root_SightstealerArrival.DaysBetweenAttacksDaysRange.RandomInRange * 60000f), delegate
			{
				quest.SignalPass(null, null, firstDelayCompleteSignal);
			}, firstHowlDelaySignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Wave 2 arrival delay";
			float points2 = Mathf.Max(num2 * QuestNode_Root_SightstealerArrival.SecondAttackStrength.RandomInRange, num);
			list = this.GenerateSightstealers(map, points2, true);
			Quest quest3 = quest;
			IEnumerable<Pawn> pawns2 = list;
			edgeWalkInDistributed = PawnsArrivalModeDefOf.EdgeWalkInDistributed;
			MapParent parent = map.Parent;
			quest3.PawnsArrive(pawns2, firstDelayCompleteSignal, parent, edgeWalkInDistributed, false, null, null, null, null, null, false, false, false);
			quest.AddPart(new QuestPart_SightstealerWave(firstDelayCompleteSignal, map.Parent, list, this.WaveTag + "2"));
			if (Rand.Bool)
			{
				quest.Delay(((float)QuestNode_Root_SightstealerArrival.HowlDelaySecondsRange.RandomInRange).SecondsToTicks(), delegate
				{
					quest.SignalPass(null, null, secondHowlDelaySignal);
				}, text2, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Wave 3 howl delay";
				Quest quest4 = quest;
				LetterDef negativeEvent2 = LetterDefOf.NegativeEvent;
				text4 = "LetterLabelSightstealerHowlBig".Translate();
				text3 = "LetterSightstealerHowlBig".Translate();
				quest4.Letter(negativeEvent2, secondHowlDelaySignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text3, null, text4, null, null);
				quest.PlayOneShotOnCamera(SoundDefOf.Sightstealer_DistantHowlLarge, secondHowlDelaySignal);
				quest.Delay((int)(QuestNode_Root_SightstealerArrival.DaysBetweenAttacksDaysRange.RandomInRange * 60000f), delegate
				{
					quest.SignalPass(null, null, secondDelayCompleteSignal);
				}, secondHowlDelaySignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Wave 3 arrival delay";
				float points3 = Mathf.Max(num2, num);
				list = this.GenerateSightstealers(map, points3, true);
				Quest quest5 = quest;
				IEnumerable<Pawn> pawns3 = list;
				edgeWalkInDistributed = PawnsArrivalModeDefOf.EdgeWalkInDistributed;
				parent = map.Parent;
				quest5.PawnsArrive(pawns3, secondDelayCompleteSignal, parent, edgeWalkInDistributed, false, null, null, null, null, null, false, false, false);
				quest.AddPart(new QuestPart_SightstealerWave(secondDelayCompleteSignal, map.Parent, list, this.WaveTag + "3"));
				quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
				return;
			}
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0600DA9B RID: 55963 RVA: 0x005160C8 File Offset: 0x005142C8
		private List<Pawn> GenerateSightstealers(Map map, float points, bool dropShard = false)
		{
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = Faction.OfEntities,
				groupKind = PawnGroupKindDefOf.Sightstealers,
				points = points,
				tile = map.Tile
			}, true).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			if (dropShard)
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			return list;
		}

		// Token: 0x04008BC1 RID: 35777
		private static readonly FloatRange DaysBetweenAttacksDaysRange = new FloatRange(0.2f, 1.5f);

		// Token: 0x04008BC2 RID: 35778
		private static readonly IntRange HowlDelaySecondsRange = new IntRange(4, 10);

		// Token: 0x04008BC3 RID: 35779
		private static readonly FloatRange SecondAttackStrength = new FloatRange(0.35f, 0.65f);

		// Token: 0x04008BC4 RID: 35780
		private const int WavesPointsThreshold = 400;

		// Token: 0x04008BC5 RID: 35781
		private const string QuestTag = "Sightstealer";
	}
}
