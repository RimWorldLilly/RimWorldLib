using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001111 RID: 4369
	public class QuestPart_FactionGoodwillChange_ShuttleSentThings : QuestPartActivable
	{
		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06006D39 RID: 27961 RVA: 0x00263F4D File Offset: 0x0026214D
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_FactionGoodwillChange_ShuttleSentThings.<get_InvolvedFactions>d__10 <get_InvolvedFactions>d__ = new QuestPart_FactionGoodwillChange_ShuttleSentThings.<get_InvolvedFactions>d__10(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06006D3A RID: 27962 RVA: 0x00263F5D File Offset: 0x0026215D
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_FactionGoodwillChange_ShuttleSentThings.<get_QuestLookTargets>d__12 <get_QuestLookTargets>d__ = new QuestPart_FactionGoodwillChange_ShuttleSentThings.<get_QuestLookTargets>d__12(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x00263F70 File Offset: 0x00262170
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			List<Thing> list;
			if (this.inSignalsShuttleSent.Contains(signal.tag) && signal.args.TryGetArg<List<Thing>>("SENT", out list))
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					if (!list.Contains(this.things[i]))
					{
						num++;
					}
				}
				this.TryAffectGoodwill(num * this.changeNotOnShuttle);
				base.Complete();
			}
			if (signal.tag == this.inSignalShuttleDestroyed)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
				base.Complete();
			}
		}

		// Token: 0x06006D3C RID: 27964 RVA: 0x00264021 File Offset: 0x00262221
		public override void Cleanup()
		{
			if (base.State == QuestPartState.Enabled)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
			}
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x00264044 File Offset: 0x00262244
		private void TryAffectGoodwill(int goodwillChange)
		{
			if (goodwillChange != 0)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, this.historyEvent, null);
			}
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x00264084 File Offset: 0x00262284
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignalsShuttleSent, "inSignalsShuttleSent", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.changeNotOnShuttle, "changeNotOnShuttle", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalShuttleDestroyed, "inSignalShuttleDestroyed", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04004584 RID: 17796
		public List<string> inSignalsShuttleSent = new List<string>();

		// Token: 0x04004585 RID: 17797
		public string inSignalShuttleDestroyed;

		// Token: 0x04004586 RID: 17798
		public int changeNotOnShuttle;

		// Token: 0x04004587 RID: 17799
		public Faction faction;

		// Token: 0x04004588 RID: 17800
		public bool canSendMessage = true;

		// Token: 0x04004589 RID: 17801
		public bool canSendHostilityLetter = true;

		// Token: 0x0400458A RID: 17802
		public string reason;

		// Token: 0x0400458B RID: 17803
		public List<Thing> things = new List<Thing>();

		// Token: 0x0400458C RID: 17804
		public HistoryEventDef historyEvent;
	}
}
