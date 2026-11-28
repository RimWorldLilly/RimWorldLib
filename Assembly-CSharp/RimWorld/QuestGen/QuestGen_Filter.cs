using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200215C RID: 8540
	public static class QuestGen_Filter
	{
		// Token: 0x0600D58F RID: 54671 RVA: 0x004F3B28 File Offset: 0x004F1D28
		public static QuestPart_Filter_AnyPawnAlive AnyPawnAlive(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive();
			questPart_Filter_AnyPawnAlive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnAlive.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnAlive.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnAlive.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AnyPawnAlive.outSignal = outSignal;
			questPart_Filter_AnyPawnAlive.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnAlive.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnAlive.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnAlive);
			return questPart_Filter_AnyPawnAlive;
		}

		// Token: 0x0600D590 RID: 54672 RVA: 0x004F3BCC File Offset: 0x004F1DCC
		public static QuestPart_Filter_AnyPawnInCombatShape AnyPawnInCombatShape(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnInCombatShape questPart_Filter_AnyPawnInCombatShape = new QuestPart_Filter_AnyPawnInCombatShape();
			questPart_Filter_AnyPawnInCombatShape.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnInCombatShape.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnInCombatShape.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnInCombatShape.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AnyPawnInCombatShape.outSignal = outSignal;
			questPart_Filter_AnyPawnInCombatShape.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnInCombatShape.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnInCombatShape.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnInCombatShape);
			return questPart_Filter_AnyPawnInCombatShape;
		}

		// Token: 0x0600D591 RID: 54673 RVA: 0x004F3C70 File Offset: 0x004F1E70
		public static QuestPart_Filter_AllPawnsDespawned AllPawnsDespawned(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDespawned questPart_Filter_AllPawnsDespawned = new QuestPart_Filter_AllPawnsDespawned();
			questPart_Filter_AllPawnsDespawned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDespawned.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDespawned.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDespawned.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDespawned.outSignal = outSignal;
			questPart_Filter_AllPawnsDespawned.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDespawned.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDespawned.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDespawned);
			return questPart_Filter_AllPawnsDespawned;
		}

		// Token: 0x0600D592 RID: 54674 RVA: 0x004F3D14 File Offset: 0x004F1F14
		public static QuestPart_Filter_AnyPawnUnhealthy AnyPawnUnhealthy(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnUnhealthy questPart_Filter_AnyPawnUnhealthy = new QuestPart_Filter_AnyPawnUnhealthy();
			questPart_Filter_AnyPawnUnhealthy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnUnhealthy.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnUnhealthy.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnUnhealthy.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AnyPawnUnhealthy.outSignal = outSignal;
			questPart_Filter_AnyPawnUnhealthy.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnUnhealthy.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnUnhealthy.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnUnhealthy);
			return questPart_Filter_AnyPawnUnhealthy;
		}

		// Token: 0x0600D593 RID: 54675 RVA: 0x004F3DB8 File Offset: 0x004F1FB8
		public static QuestPart_Filter_FactionHostileToOtherFaction FactionHostileToOtherFaction(this Quest quest, Faction faction, Faction other, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_FactionHostileToOtherFaction questPart_Filter_FactionHostileToOtherFaction = new QuestPart_Filter_FactionHostileToOtherFaction();
			questPart_Filter_FactionHostileToOtherFaction.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_FactionHostileToOtherFaction.signalListenMode = signalListenMode;
			questPart_Filter_FactionHostileToOtherFaction.faction = faction;
			questPart_Filter_FactionHostileToOtherFaction.other = other;
			questPart_Filter_FactionHostileToOtherFaction.outSignal = outSignal;
			questPart_Filter_FactionHostileToOtherFaction.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionHostileToOtherFaction.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionHostileToOtherFaction.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_FactionHostileToOtherFaction);
			return questPart_Filter_FactionHostileToOtherFaction;
		}

		// Token: 0x0600D594 RID: 54676 RVA: 0x004F3E58 File Offset: 0x004F2058
		public static QuestPart_Filter_AnyPawnPlayerControlled AnyPawnPlayerControlled(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnPlayerControlled questPart_Filter_AnyPawnPlayerControlled = new QuestPart_Filter_AnyPawnPlayerControlled();
			questPart_Filter_AnyPawnPlayerControlled.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnPlayerControlled.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnPlayerControlled.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnPlayerControlled.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AnyPawnPlayerControlled.outSignal = outSignal;
			questPart_Filter_AnyPawnPlayerControlled.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnPlayerControlled.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnPlayerControlled.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnPlayerControlled);
			return questPart_Filter_AnyPawnPlayerControlled;
		}

		// Token: 0x0600D595 RID: 54677 RVA: 0x004F3EFC File Offset: 0x004F20FC
		public static QuestPart_Filter_AllPawnsDestroyed AllPawnsDestroyed(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDestroyed questPart_Filter_AllPawnsDestroyed = new QuestPart_Filter_AllPawnsDestroyed();
			questPart_Filter_AllPawnsDestroyed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDestroyed.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDestroyed.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDestroyed.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDestroyed.outSignal = outSignal;
			questPart_Filter_AllPawnsDestroyed.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDestroyed.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDestroyed.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDestroyed);
			return questPart_Filter_AllPawnsDestroyed;
		}

		// Token: 0x0600D596 RID: 54678 RVA: 0x004F3FA0 File Offset: 0x004F21A0
		public static QuestPart_Filter_PawnDestroyed PawnDestroyed(this Quest quest, Pawn pawn, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_PawnDestroyed questPart_Filter_PawnDestroyed = new QuestPart_Filter_PawnDestroyed
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawn = pawn,
				outSignal = outSignal,
				outSignalElse = outSignalElse,
				signalListenMode = signalListenMode
			};
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_PawnDestroyed.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_PawnDestroyed.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_PawnDestroyed);
			return questPart_Filter_PawnDestroyed;
		}

		// Token: 0x0600D597 RID: 54679 RVA: 0x004F4038 File Offset: 0x004F2238
		public static QuestPart_Filter_FactionNonPlayer FactionNonPlayer(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_FactionNonPlayer questPart_Filter_FactionNonPlayer = new QuestPart_Filter_FactionNonPlayer();
			questPart_Filter_FactionNonPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_FactionNonPlayer.signalListenMode = signalListenMode;
			questPart_Filter_FactionNonPlayer.outSignal = outSignal;
			questPart_Filter_FactionNonPlayer.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionNonPlayer.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionNonPlayer.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_FactionNonPlayer);
			return questPart_Filter_FactionNonPlayer;
		}

		// Token: 0x0600D598 RID: 54680 RVA: 0x004F40C8 File Offset: 0x004F22C8
		public static QuestPart_Filter_AllPawnsDowned AllPawnsDowned(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDowned questPart_Filter_AllPawnsDowned = new QuestPart_Filter_AllPawnsDowned();
			questPart_Filter_AllPawnsDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDowned.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDowned.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDowned.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDowned.outSignal = outSignal;
			questPart_Filter_AllPawnsDowned.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDowned.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDowned.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDowned);
			return questPart_Filter_AllPawnsDowned;
		}

		// Token: 0x0600D599 RID: 54681 RVA: 0x004F416C File Offset: 0x004F236C
		public static QuestPart_Filter_AnyOnTransporter AnyOnTransporter(this Quest quest, IEnumerable<Pawn> pawns, Thing shuttle, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyOnTransporter questPart_Filter_AnyOnTransporter = new QuestPart_Filter_AnyOnTransporter();
			questPart_Filter_AnyOnTransporter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyOnTransporter.signalListenMode = signalListenMode;
			questPart_Filter_AnyOnTransporter.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyOnTransporter.outSignal = outSignal;
			questPart_Filter_AnyOnTransporter.outSignalElse = outSignalElse;
			questPart_Filter_AnyOnTransporter.transporter = shuttle;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyOnTransporter.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyOnTransporter.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyOnTransporter);
			return questPart_Filter_AnyOnTransporter;
		}

		// Token: 0x0600D59A RID: 54682 RVA: 0x004F4210 File Offset: 0x004F2410
		public static QuestPart_Filter_AcceptedAfterTicks AcceptedAfterTicks(this Quest quest, int ticks, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AcceptedAfterTicks questPart_Filter_AcceptedAfterTicks = new QuestPart_Filter_AcceptedAfterTicks();
			questPart_Filter_AcceptedAfterTicks.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AcceptedAfterTicks.signalListenMode = signalListenMode;
			questPart_Filter_AcceptedAfterTicks.timeTicks = ticks;
			questPart_Filter_AcceptedAfterTicks.outSignal = outSignal;
			questPart_Filter_AcceptedAfterTicks.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AcceptedAfterTicks.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AcceptedAfterTicks.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AcceptedAfterTicks);
			return questPart_Filter_AcceptedAfterTicks;
		}

		// Token: 0x0600D59B RID: 54683 RVA: 0x004F42A8 File Offset: 0x004F24A8
		public static QuestPart_Filter_AnyColonistWithCharityPrecept AnyColonistWithCharityPrecept(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyColonistWithCharityPrecept questPart_Filter_AnyColonistWithCharityPrecept = new QuestPart_Filter_AnyColonistWithCharityPrecept();
			questPart_Filter_AnyColonistWithCharityPrecept.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyColonistWithCharityPrecept.signalListenMode = signalListenMode;
			questPart_Filter_AnyColonistWithCharityPrecept.outSignal = outSignal;
			questPart_Filter_AnyColonistWithCharityPrecept.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyColonistWithCharityPrecept.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyColonistWithCharityPrecept.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyColonistWithCharityPrecept);
			return questPart_Filter_AnyColonistWithCharityPrecept;
		}

		// Token: 0x0600D59C RID: 54684 RVA: 0x004F4338 File Offset: 0x004F2538
		public static QuestPart_Filter_BuiltNearSettlement BuiltNearSettlement(this Quest quest, Faction settlementFaction, MapParent mapParent, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_BuiltNearSettlement questPart_Filter_BuiltNearSettlement = new QuestPart_Filter_BuiltNearSettlement();
			questPart_Filter_BuiltNearSettlement.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_BuiltNearSettlement.signalListenMode = signalListenMode;
			questPart_Filter_BuiltNearSettlement.settlementFaction = settlementFaction;
			questPart_Filter_BuiltNearSettlement.mapParent = mapParent;
			questPart_Filter_BuiltNearSettlement.outSignal = outSignal;
			questPart_Filter_BuiltNearSettlement.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_BuiltNearSettlement.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_BuiltNearSettlement.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_BuiltNearSettlement);
			return questPart_Filter_BuiltNearSettlement;
		}

		// Token: 0x0600D59D RID: 54685 RVA: 0x004F43D8 File Offset: 0x004F25D8
		public static QuestPart_Filter_AnyHostileThreatToPlayer AnyHostileThreatToPlayer(this Quest quest, MapParent mapParent, bool countDormantPawns = false, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyHostileThreatToPlayer questPart_Filter_AnyHostileThreatToPlayer = new QuestPart_Filter_AnyHostileThreatToPlayer();
			questPart_Filter_AnyHostileThreatToPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyHostileThreatToPlayer.signalListenMode = signalListenMode;
			questPart_Filter_AnyHostileThreatToPlayer.outSignal = outSignal;
			questPart_Filter_AnyHostileThreatToPlayer.outSignalElse = outSignalElse;
			questPart_Filter_AnyHostileThreatToPlayer.countDormantPawnsAsHostile = countDormantPawns;
			questPart_Filter_AnyHostileThreatToPlayer.mapParent = mapParent;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyHostileThreatToPlayer.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyHostileThreatToPlayer.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyHostileThreatToPlayer);
			return questPart_Filter_AnyHostileThreatToPlayer;
		}

		// Token: 0x0600D59E RID: 54686 RVA: 0x004F4478 File Offset: 0x004F2678
		public static QuestPart_Filter_CanAcceptQuest CanAcceptQuest(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_CanAcceptQuest questPart_Filter_CanAcceptQuest = new QuestPart_Filter_CanAcceptQuest();
			questPart_Filter_CanAcceptQuest.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_CanAcceptQuest.signalListenMode = signalListenMode;
			questPart_Filter_CanAcceptQuest.outSignal = outSignal;
			questPart_Filter_CanAcceptQuest.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_CanAcceptQuest.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_CanAcceptQuest.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_CanAcceptQuest);
			return questPart_Filter_CanAcceptQuest;
		}

		// Token: 0x0600D59F RID: 54687 RVA: 0x004F4508 File Offset: 0x004F2708
		public static QuestPart_Filter_ThingAnalyzed ThingAnalyzed(this Quest quest, ThingDef thingDef, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_ThingAnalyzed questPart_Filter_ThingAnalyzed = new QuestPart_Filter_ThingAnalyzed();
			questPart_Filter_ThingAnalyzed.thingDef = thingDef;
			questPart_Filter_ThingAnalyzed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_ThingAnalyzed.signalListenMode = signalListenMode;
			questPart_Filter_ThingAnalyzed.outSignal = outSignal;
			questPart_Filter_ThingAnalyzed.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_ThingAnalyzed.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_ThingAnalyzed.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_ThingAnalyzed);
			return questPart_Filter_ThingAnalyzed;
		}

		// Token: 0x0600D5A0 RID: 54688 RVA: 0x004F45A0 File Offset: 0x004F27A0
		public static QuestPart_Filter_AnyPawnHasHediff AnyPawnHasHediff(this Quest quest, IEnumerable<Pawn> pawns, HediffDef hediff, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnHasHediff questPart_Filter_AnyPawnHasHediff = new QuestPart_Filter_AnyPawnHasHediff();
			questPart_Filter_AnyPawnHasHediff.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnHasHediff.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnHasHediff.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnHasHediff.hediff = hediff;
			questPart_Filter_AnyPawnHasHediff.outSignal = outSignal;
			questPart_Filter_AnyPawnHasHediff.outSignalElse = outSignalElse;
			questPart_Filter_AnyPawnHasHediff.inSignalRemovePawn = inSignalRemovePawn;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnHasHediff.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnHasHediff.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnHasHediff);
			return questPart_Filter_AnyPawnHasHediff;
		}
	}
}
