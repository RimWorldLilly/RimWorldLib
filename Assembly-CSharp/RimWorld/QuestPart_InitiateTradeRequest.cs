using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C7 RID: 4295
	public class QuestPart_InitiateTradeRequest : QuestPart
	{
		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06006B59 RID: 27481 RVA: 0x0025A720 File Offset: 0x00258920
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_InitiateTradeRequest.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_InitiateTradeRequest.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06006B5A RID: 27482 RVA: 0x0025A730 File Offset: 0x00258930
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_InitiateTradeRequest.<get_InvolvedFactions>d__9 <get_InvolvedFactions>d__ = new QuestPart_InitiateTradeRequest.<get_InvolvedFactions>d__9(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06006B5B RID: 27483 RVA: 0x0025A740 File Offset: 0x00258940
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_InitiateTradeRequest.<get_Hyperlinks>d__11 <get_Hyperlinks>d__ = new QuestPart_InitiateTradeRequest.<get_Hyperlinks>d__11(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x06006B5C RID: 27484 RVA: 0x0025A750 File Offset: 0x00258950
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null)
				{
					if (component.ActiveRequest)
					{
						Log.Error("Settlement " + this.settlement.Label + " already has an active trade request.");
						return;
					}
					component.requestThingDef = this.requestedThingDef;
					component.requestCount = this.requestedCount;
					component.expiration = Find.TickManager.TicksGame + this.requestDuration;
				}
			}
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x0025A7E0 File Offset: 0x002589E0
		public override void Cleanup()
		{
			base.Cleanup();
			if (!this.keepAfterQuestEnds)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x0025A818 File Offset: 0x00258A18
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<ThingDef>(ref this.requestedThingDef, "requestedThingDef");
			Scribe_Values.Look<int>(ref this.requestedCount, "requestedCount", 0, false);
			Scribe_Values.Look<int>(ref this.requestDuration, "requestDuration", 0, false);
			Scribe_Values.Look<bool>(ref this.keepAfterQuestEnds, "keepAfterQuestEnds", false, false);
		}

		// Token: 0x06006B5F RID: 27487 RVA: 0x0025A894 File Offset: 0x00258A94
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && !component.ActiveRequest && x.Faction != Faction.OfPlayer;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
			this.requestedThingDef = ThingDefOf.Silver;
			this.requestedCount = 100;
			this.requestDuration = 60000;
		}

		// Token: 0x0400443C RID: 17468
		public string inSignal;

		// Token: 0x0400443D RID: 17469
		public Settlement settlement;

		// Token: 0x0400443E RID: 17470
		public ThingDef requestedThingDef;

		// Token: 0x0400443F RID: 17471
		public int requestedCount;

		// Token: 0x04004440 RID: 17472
		public int requestDuration;

		// Token: 0x04004441 RID: 17473
		public bool keepAfterQuestEnds;
	}
}
