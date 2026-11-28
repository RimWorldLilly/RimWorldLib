using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200047F RID: 1151
	public class HediffComp_SeverityFromHemogen : HediffComp
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x000C17AC File Offset: 0x000BF9AC
		public HediffCompProperties_SeverityFromHemogen Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromHemogen)this.props;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x000C17B9 File Offset: 0x000BF9B9
		public override bool CompShouldRemove
		{
			get
			{
				Pawn_GeneTracker genes = base.Pawn.genes;
				return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) == null;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x000C17D8 File Offset: 0x000BF9D8
		private Gene_Hemogen Hemogen
		{
			get
			{
				Gene_Hemogen result;
				if ((result = this.cachedHemogenGene) == null)
				{
					result = (this.cachedHemogenGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Hemogen>());
				}
				return result;
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000C1808 File Offset: 0x000BFA08
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Hemogen != null)
			{
				severityAdjustment += ((this.Hemogen.Value > 0f) ? this.Props.severityPerHourHemogen : this.Props.severityPerHourEmpty) / 2500f;
			}
		}

		// Token: 0x040018F3 RID: 6387
		private Gene_Hemogen cachedHemogenGene;
	}
}
