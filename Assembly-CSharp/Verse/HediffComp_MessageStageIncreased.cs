using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000460 RID: 1120
	public class HediffComp_MessageStageIncreased : HediffComp
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x000C06D5 File Offset: 0x000BE8D5
		protected HediffCompProperties_MessageStageIncreased Props
		{
			get
			{
				return (HediffCompProperties_MessageStageIncreased)this.props;
			}
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000C06E4 File Offset: 0x000BE8E4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.CurStageIndex > this.lastStageMessaged)
			{
				this.lastStageMessaged = this.parent.CurStageIndex;
				if (this.sendMessages)
				{
					Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType ?? MessageTypeDefOf.NeutralEvent, true);
				}
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000C0762 File Offset: 0x000BE962
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.sendMessages, "sendMessages", false, false);
			Scribe_Values.Look<int>(ref this.lastStageMessaged, "lastStageMessaged", 0, false);
		}

		// Token: 0x040018BB RID: 6331
		private int lastStageMessaged;

		// Token: 0x040018BC RID: 6332
		public bool sendMessages = true;
	}
}
