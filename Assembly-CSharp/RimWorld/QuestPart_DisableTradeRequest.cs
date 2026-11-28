using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BC RID: 4284
	public class QuestPart_DisableTradeRequest : QuestPart
	{
		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06006B22 RID: 27426 RVA: 0x00259AE3 File Offset: 0x00257CE3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DisableTradeRequest.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_DisableTradeRequest.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06006B23 RID: 27427 RVA: 0x00259AF3 File Offset: 0x00257CF3
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_DisableTradeRequest.<get_InvolvedFactions>d__5 <get_InvolvedFactions>d__ = new QuestPart_DisableTradeRequest.<get_InvolvedFactions>d__5(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x00259B04 File Offset: 0x00257D04
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x00259B48 File Offset: 0x00257D48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x00259B74 File Offset: 0x00257D74
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && component.ActiveRequest;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
		}

		// Token: 0x0400441B RID: 17435
		public string inSignal;

		// Token: 0x0400441C RID: 17436
		public Settlement settlement;
	}
}
