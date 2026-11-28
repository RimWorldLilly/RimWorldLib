using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001917 RID: 6423
	public class CompNoiseSource : ThingComp
	{
		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x0600A10A RID: 41226 RVA: 0x003994C1 File Offset: 0x003976C1
		public CompProperties_NoiseSource Props
		{
			get
			{
				return (CompProperties_NoiseSource)this.props;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x0600A10B RID: 41227 RVA: 0x003994CE File Offset: 0x003976CE
		public bool Active
		{
			get
			{
				return this.PowerComp == null || (this.powerComp.PowerOn && this.PowerComp.PowerOutput < 0f) || this.powerComp.PowerOutput > 0f;
			}
		}

		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x0600A10C RID: 41228 RVA: 0x0039950C File Offset: 0x0039770C
		private CompPowerTrader PowerComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerComp) == null)
				{
					result = (this.powerComp = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x0600A10D RID: 41229 RVA: 0x00399537 File Offset: 0x00397737
		public override void PostDrawExtraSelectionOverlays()
		{
			GenDraw.DrawRadiusRing(this.parent.Position, this.Props.radius);
		}

		// Token: 0x04005F22 RID: 24354
		private CompPowerTrader powerComp;
	}
}
