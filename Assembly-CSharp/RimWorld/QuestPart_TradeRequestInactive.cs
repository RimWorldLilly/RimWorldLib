using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108E RID: 4238
	public class QuestPart_TradeRequestInactive : QuestPartActivable
	{
		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06006A2F RID: 27183 RVA: 0x00255EA2 File Offset: 0x002540A2
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_TradeRequestInactive.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_TradeRequestInactive.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x00255EB2 File Offset: 0x002540B2
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_TradeRequestInactive.<get_InvolvedFactions>d__4 <get_InvolvedFactions>d__ = new QuestPart_TradeRequestInactive.<get_InvolvedFactions>d__4(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x00255EC4 File Offset: 0x002540C4
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.settlement == null || !this.settlement.Spawned)
			{
				base.Complete();
				return;
			}
			TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
			if (component == null || !component.ActiveRequest)
			{
				base.Complete(this.settlement.Named("SUBJECT"));
			}
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x00255F20 File Offset: 0x00254120
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x00255F39 File Offset: 0x00254139
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.settlement = Find.WorldObjects.Settlements.FirstOrDefault<Settlement>();
		}

		// Token: 0x04004379 RID: 17273
		public Settlement settlement;
	}
}
