using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AD RID: 8877
	public abstract class QuestNode_Root_WandererJoin : QuestNode
	{
		// Token: 0x17002630 RID: 9776
		// (get) Token: 0x0600DB29 RID: 56105 RVA: 0x00519EE8 File Offset: 0x005180E8
		protected virtual int AllowKilledBeforeTicks
		{
			get
			{
				return 15000;
			}
		}

		// Token: 0x17002631 RID: 9777
		// (get) Token: 0x0600DB2A RID: 56106 RVA: 0x00519EEF File Offset: 0x005180EF
		protected virtual bool CanBeSpace
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DB2B RID: 56107
		public abstract Pawn GeneratePawn();

		// Token: 0x0600DB2C RID: 56108 RVA: 0x00519EF2 File Offset: 0x005180F2
		public virtual Pawn GeneratePawn_NewTemp(Map map)
		{
			return null;
		}

		// Token: 0x0600DB2D RID: 56109
		[Obsolete]
		public abstract void SendLetter(Quest quest, Pawn pawn);

		// Token: 0x0600DB2E RID: 56110 RVA: 0x00519EF5 File Offset: 0x005180F5
		public virtual void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
		{
			this.SendLetter(quest, pawn);
		}

		// Token: 0x0600DB2F RID: 56111 RVA: 0x00519F00 File Offset: 0x00518100
		protected virtual void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			quest.DropPods(map.Parent, Gen.YieldSingle<Pawn>(pawn), null, null, null, null, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, false, null);
		}

		// Token: 0x0600DB30 RID: 56112 RVA: 0x00519F40 File Offset: 0x00518140
		protected virtual void AddLeftMapQuestParts(Quest quest, Pawn pawn)
		{
			Action <>9__2;
			quest.AnyPawnUnhealthy(Gen.YieldSingle<Pawn>(pawn), delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, delegate
			{
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate()
					{
						quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererLeftHealthy".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
					});
				}
				quest2.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600DB31 RID: 56113 RVA: 0x00519F98 File Offset: 0x00518198
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map;
			if (!slate.TryGet<Map>("map", out map, false))
			{
				bool mustBeInfestable = false;
				bool canBeSpace = this.CanBeSpace;
				map = QuestGen_Get.GetMap(mustBeInfestable, null, canBeSpace);
			}
			if (!this.CanBeSpace)
			{
				quest.AcceptanceRequirementNotSpace(map.Parent);
			}
			Pawn pawn = this.GeneratePawn_NewTemp(map);
			this.AddSpawnPawnQuestParts(quest, map, pawn);
			slate.Set<Pawn>("pawn", pawn, false);
			this.SendLetter_NewTemp(quest, pawn, map);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftBehind");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.PlayerTended");
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
			string inSignal5 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");
			quest.End(QuestEndOutcome.Success, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			Action <>9__6;
			Action <>9__4;
			Action <>9__5;
			quest.Signal(inSignal, delegate
			{
				Quest quest = quest;
				int allowKilledBeforeTicks = this.AllowKilledBeforeTicks;
				Action action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate()
					{
						Quest quest2 = quest;
						Action action2;
						if ((action2 = <>9__6) == null)
						{
							action2 = (<>9__6 = delegate()
							{
								quest.Message("MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftToDie".Translate(pawn), MessageTypeDefOf.NegativeEvent, false, null, pawn, null);
							});
						}
						quest2.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				Action elseAction;
				if ((elseAction = <>9__5) == null)
				{
					elseAction = (<>9__5 = delegate()
					{
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest.AcceptedAfterTicks(allowKilledBeforeTicks, action, elseAction, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			Action <>9__7;
			quest.Signal(inSignal2, delegate
			{
				Quest quest = quest;
				Action action;
				if ((action = <>9__7) == null)
				{
					action = (<>9__7 = delegate()
					{
						quest.Message("MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftBehind".Translate(pawn), MessageTypeDefOf.NegativeEvent, false, null, pawn, null);
					});
				}
				quest.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererRecruited".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
			}, null, inSignal5, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal5, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Signal(inSignal4, delegate
			{
				this.AddLeftMapQuestParts(quest, pawn);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600DB32 RID: 56114 RVA: 0x0051A11C File Offset: 0x0051831C
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (!slate.TryGet<Map>("map", out map, false))
			{
				bool mustBeInfestable = false;
				bool canBeSpace = this.CanBeSpace;
				return QuestGen_Get.GetMap(mustBeInfestable, null, canBeSpace) != null;
			}
			if (!this.CanBeSpace)
			{
				return Find.AnyPlayerHomeMap != null;
			}
			return Find.RandomSurfacePlayerHomeMap != null;
		}
	}
}
