using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000458 RID: 1112
	public class HediffComp_MessageAboveSeverity : HediffComp
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000C02A0 File Offset: 0x000BE4A0
		protected HediffCompProperties_MessageAboveSeverity Props
		{
			get
			{
				return (HediffCompProperties_MessageAboveSeverity)this.props;
			}
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000C02B0 File Offset: 0x000BE4B0
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!this.messageSent && this.parent.Severity >= this.Props.severity)
			{
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn) && this.Props.messageType != null)
				{
					Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
				}
				this.messageSent = true;
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000C033A File Offset: 0x000BE53A
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.messageSent, "messageSent", false, false);
		}

		// Token: 0x040018AC RID: 6316
		private bool messageSent;
	}
}
