using System;

namespace Verse
{
	// Token: 0x0200041D RID: 1053
	public class HediffComp_DisappearsPausable : HediffComp_Disappears
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x000BD2C7 File Offset: 0x000BB4C7
		protected virtual bool Paused
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000BD2CA File Offset: 0x000BB4CA
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Paused)
				{
					return base.CompLabelInBracketsExtra;
				}
				return null;
			}
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000BD2DC File Offset: 0x000BB4DC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.Paused)
			{
				this.ticksToDisappear -= this.TicksLostPerTick;
			}
		}
	}
}
