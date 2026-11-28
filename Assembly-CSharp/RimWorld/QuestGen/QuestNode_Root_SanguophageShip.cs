using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002292 RID: 8850
	public class QuestNode_Root_SanguophageShip : QuestNode
	{
		// Token: 0x0600DA8B RID: 55947 RVA: 0x00514BB8 File Offset: 0x00512DB8
		protected override void RunInt()
		{
			if (!ModLister.CheckBiotech("Sanguophage ship"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float x = slate.Get<float>("points", 0f, false);
			int endTicks = 5240;
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("SanguophageShip");
			string attackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.TookDamageFromPlayer");
			string defendTimeoutSignal = QuestGen.GenerateNewSignal("DefendTimeout", true);
			string beginAssaultSignal = QuestGen.GenerateNewSignal("BeginAssault", true);
			string assaultBeganSignal = QuestGen.GenerateNewSignal("AssaultBegan", true);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved");
			slate.Set<Map>("map", map, false);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
			{
				list.Add(new FactionRelation(other, FactionRelationKind.Hostile));
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Sanguophages, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			quest.ReserveFaction(faction);
			List<Pawn> shuttlePawns = new List<Pawn>();
			Pawn sanguophage = quest.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Sanguophage, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), true, false, false, true, true, 1f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false), false);
			sanguophage.health.forceDowned = true;
			shuttlePawns.Add(sanguophage);
			slate.Set<Pawn>("sanguophage", sanguophage, false);
			int num = Mathf.RoundToInt(QuestNode_Root_SanguophageShip.PointsToThrallCountCurve.Evaluate(x));
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SanguophageThrall, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), true, false, false, true, true, 1f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			for (int i = 0; i < num; i++)
			{
				Pawn item = quest.GeneratePawn(request, false);
				shuttlePawns.Add(item);
			}
			slate.Set<int>("thrallCount", num, false);
			slate.Set<List<Pawn>>("shuttlePawns", shuttlePawns, false);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed_Exitable, null);
			quest.SetFaction(Gen.YieldSingle<Thing>(thing), faction, null);
			IntVec3 shuttleCrashPosition;
			this.TryFindShuttleCrashPosition(map, thing.def.size, out shuttleCrashPosition);
			TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_ShuttleCrashing, shuttlePawns, thing, null).transportShip;
			quest.AddShipJob_WaitTime(transportShip, 60, false, null, null).showGizmos = false;
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
			QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			int reinforcementsCount = Mathf.RoundToInt(QuestNode_Root_SanguophageShip.PointsToReinforcementsCountCurve.Evaluate(x));
			if (reinforcementsCount > 0)
			{
				endTicks = 10060;
			}
			List<Pawn> reinforcements = null;
			if (reinforcementsCount > 0)
			{
				reinforcements = new List<Pawn>();
				for (int j = 0; j < reinforcementsCount; j++)
				{
					Pawn item2 = quest.GeneratePawn(request, false);
					reinforcements.Add(item2);
				}
				quest.BiocodeWeapons(reinforcements, null);
			}
			Action <>9__1;
			Action <>9__5;
			Action <>9__2;
			Action <>9__3;
			Action <>9__4;
			quest.Delay(180, delegate
			{
				Quest quest;
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, shuttlePawns, false, "[sanguophageShuttleCrashedLetterText]", null, "[sanguophageShuttleCrashedLetterLabel]", null, null);
				quest.AddShipJob_Arrive(transportShip, map.Parent, sanguophage, new IntVec3?(shuttleCrashPosition), ShipJobStartMode.Force_DelayCurrent, faction, null);
				quest.DefendPoint(map.Parent, sanguophage, shuttleCrashPosition, shuttlePawns, faction, null, null, new float?((float)5), false, true);
				quest = quest;
				int delayTicks = 5000;
				Action inner;
				if ((inner = <>9__1) == null)
				{
					inner = (<>9__1 = delegate()
					{
						quest.SignalPass(null, null, attackedSignal);
					});
				}
				quest.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Assault delay";
				quest.AnySignal(new string[]
				{
					attackedSignal,
					defendTimeoutSignal
				}, null, Gen.YieldSingle<string>(beginAssaultSignal), QuestPart.SignalListenMode.OngoingOnly);
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate()
					{
						Quest quest5 = quest;
						IEnumerable<Pawn> shuttlePawns = shuttlePawns;
						Action action2;
						if ((action2 = <>9__5) == null)
						{
							action2 = (<>9__5 = delegate()
							{
								QuestPart_AssaultColony questPart_AssaultColony = quest.AssaultColony(faction, map.Parent, shuttlePawns, null, null);
								questPart_AssaultColony.canKidnap = false;
								questPart_AssaultColony.canSteal = false;
								questPart_AssaultColony.canTimeoutOrFlee = false;
								quest.Letter(LetterDefOf.ThreatSmall, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, shuttlePawns, false, "[assaultBeginLetterText]", null, "[assaultBeginLetterLabel]", null, null);
							});
						}
						quest5.AnyPawnInCombatShape(shuttlePawns, action2, null, null, assaultBeganSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest2.SignalPassActivable(action, null, beginAssaultSignal, null, null, assaultBeganSignal, false);
				if (reinforcementsCount > 0)
				{
					Quest quest3 = quest;
					int delayTicks2 = 10000;
					Action inner2;
					if ((inner2 = <>9__3) == null)
					{
						inner2 = (<>9__3 = delegate()
						{
							quest.Letter(LetterDefOf.ThreatBig, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, reinforcements, false, "[reinforcementsArrivedLetterText]", null, "[reinforcementsArrivedLetterLabel]", null, null);
							IntVec3 value;
							DropCellFinder.TryFindRaidDropCenterClose(out value, map, true, true, true, -1);
							quest.DropPods(map.Parent, reinforcements, null, null, null, null, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, new IntVec3?(value), true, false, false, false, null);
							QuestPart_AssaultColony questPart_AssaultColony = quest.AssaultColony(faction, map.Parent, reinforcements, null, null);
							questPart_AssaultColony.canSteal = false;
							questPart_AssaultColony.canTimeoutOrFlee = false;
						});
					}
					quest3.Delay(delayTicks2, inner2, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Reinforcements delay";
				}
				Quest quest4 = quest;
				int endTicks = endTicks;
				Action inner3;
				if ((inner3 = <>9__4) == null)
				{
					inner3 = (<>9__4 = delegate()
					{
						quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest4.Delay(endTicks, inner3, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "End delay";
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, true).debugLabel = "Arrival delay";
		}

		// Token: 0x0600DA8C RID: 55948 RVA: 0x00515098 File Offset: 0x00513298
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			IntVec3 intVec;
			return map != null && this.TryFindShuttleCrashPosition(map, ThingDefOf.ShuttleCrashed.size, out intVec);
		}

		// Token: 0x0600DA8D RID: 55949 RVA: 0x005150D3 File Offset: 0x005132D3
		private bool TryFindShuttleCrashPosition(Map map, IntVec2 size, out IntVec3 spot)
		{
			return DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, new IntVec2?(size), new IntVec3?(ThingDefOf.ShuttleCrashed.interactionCellOffset));
		}

		// Token: 0x04008BAD RID: 35757
		private const string QuestTag = "SanguophageShip";

		// Token: 0x04008BAE RID: 35758
		private const int TicksToShuttleArrival = 180;

		// Token: 0x04008BAF RID: 35759
		private const int TicksToReinforcements = 10000;

		// Token: 0x04008BB0 RID: 35760
		private const int TicksToBeginAssault = 5000;

		// Token: 0x04008BB1 RID: 35761
		private static readonly SimpleCurve PointsToThrallCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(2000f, 5f),
				true
			}
		};

		// Token: 0x04008BB2 RID: 35762
		private static readonly SimpleCurve PointsToReinforcementsCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(2000f, 0f),
				true
			},
			{
				new CurvePoint(2500f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			},
			{
				new CurvePoint(8000f, 6f),
				true
			}
		};
	}
}
