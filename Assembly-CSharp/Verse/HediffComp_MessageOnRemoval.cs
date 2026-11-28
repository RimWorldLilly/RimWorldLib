using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200045E RID: 1118
	public class HediffComp_MessageOnRemoval : HediffComp_MessageBase
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x000C063F File Offset: 0x000BE83F
		protected HediffCompProperties_MessageOnRemoval Props
		{
			get
			{
				return (HediffCompProperties_MessageOnRemoval)this.props;
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000C064C File Offset: 0x000BE84C
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (((this.Props.messageOnZeroSeverity && this.parent.Severity <= 0f) || (this.Props.messageOnNonZeroSeverity && this.parent.Severity > 0f)) && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				this.Message();
			}
		}
	}
}
