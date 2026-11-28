using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C4 RID: 4292
	public class QuestPart_FactionRelationChange : QuestPart
	{
		// Token: 0x06006B49 RID: 27465 RVA: 0x0025A490 File Offset: 0x00258690
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				this.faction.SetRelationDirect(Faction.OfPlayer, this.relationKind, this.canSendHostilityLetter, null, null);
			}
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x0025A4F2 File Offset: 0x002586F2
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x0025A504 File Offset: 0x00258704
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", false, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04004433 RID: 17459
		public string inSignal;

		// Token: 0x04004434 RID: 17460
		public Faction faction;

		// Token: 0x04004435 RID: 17461
		public FactionRelationKind relationKind;

		// Token: 0x04004436 RID: 17462
		public bool canSendHostilityLetter;
	}
}
