using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	public class HediffComp_GiveHemogen : HediffComp
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x000BEB50 File Offset: 0x000BCD50
		public HediffCompProperties_GiveHemogen Props
		{
			get
			{
				return (HediffCompProperties_GiveHemogen)this.props;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000BEB5D File Offset: 0x000BCD5D
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			GeneUtility.OffsetHemogen(base.Pawn, this.Props.amountPerDay / 60000f * (float)delta, true);
		}
	}
}
