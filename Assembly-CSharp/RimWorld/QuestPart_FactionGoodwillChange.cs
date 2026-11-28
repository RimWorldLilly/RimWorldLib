using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001110 RID: 4368
	[StaticConstructorOnStartup]
	public class QuestPart_FactionGoodwillChange : QuestPart
	{
		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x00263CAE File Offset: 0x00261EAE
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_FactionGoodwillChange.<get_QuestLookTargets>d__10 <get_QuestLookTargets>d__ = new QuestPart_FactionGoodwillChange.<get_QuestLookTargets>d__10(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06006D32 RID: 27954 RVA: 0x00263CBE File Offset: 0x00261EBE
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_FactionGoodwillChange.<get_InvolvedFactions>d__12 <get_InvolvedFactions>d__ = new QuestPart_FactionGoodwillChange.<get_InvolvedFactions>d__12(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x00263CD0 File Offset: 0x00261ED0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				if (this.lookTarget.IsValid)
				{
					GlobalTargetInfo globalTargetInfo = this.lookTarget;
				}
				else if (this.getLookTargetFromSignal)
				{
					LookTargets lookTargets;
					if (SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
					{
						lookTargets.TryGetPrimaryTarget();
					}
					else
					{
						GlobalTargetInfo invalid = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					GlobalTargetInfo invalid2 = GlobalTargetInfo.Invalid;
				}
				FactionRelationKind playerRelationKind = this.faction.PlayerRelationKind;
				int num = 0;
				if (!signal.args.TryGetArg<int>("GOODWILL", out num))
				{
					num = this.change;
				}
				if (this.ensureMakesHostile)
				{
					num = Mathf.Min(num, Faction.OfPlayer.GoodwillToMakeHostile(this.faction));
				}
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, num, this.canSendMessage, this.canSendHostilityLetter, (num >= 0) ? (this.historyEvent ?? HistoryEventDefOf.QuestGoodwillReward) : this.historyEvent, null);
				TaggedString t = "";
				this.faction.TryAppendRelationKindChangedInfo(ref t, playerRelationKind, this.faction.PlayerRelationKind, null);
				if (!t.NullOrEmpty())
				{
					t = "\n\n" + t;
				}
			}
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x00263E24 File Offset: 0x00262024
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.change, "change", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetFromSignal, "getLookTargetFromSignal", true, false);
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
			Scribe_Values.Look<bool>(ref this.ensureMakesHostile, "ensureMakesHostile", false, false);
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x00263ED4 File Offset: 0x002620D4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.change = -15;
			this.faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
		}

		// Token: 0x0400457B RID: 17787
		public HistoryEventDef historyEvent;

		// Token: 0x0400457C RID: 17788
		public string inSignal;

		// Token: 0x0400457D RID: 17789
		public int change;

		// Token: 0x0400457E RID: 17790
		public Faction faction;

		// Token: 0x0400457F RID: 17791
		public bool canSendMessage = true;

		// Token: 0x04004580 RID: 17792
		public bool canSendHostilityLetter = true;

		// Token: 0x04004581 RID: 17793
		public bool getLookTargetFromSignal = true;

		// Token: 0x04004582 RID: 17794
		public GlobalTargetInfo lookTarget;

		// Token: 0x04004583 RID: 17795
		public bool ensureMakesHostile;
	}
}
