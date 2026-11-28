using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000416 RID: 1046
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000BCAEB File Offset: 0x000BACEB
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000BCAF8 File Offset: 0x000BACF8
		private Need Need
		{
			get
			{
				if (this.needCached == null)
				{
					base.Pawn.needs.TryGetNeed<Need>(out this.needCached);
				}
				return this.needCached;
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000BCB1F File Offset: 0x000BAD1F
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f * (float)delta;
			}
		}

		// Token: 0x0400182A RID: 6186
		private Need needCached;
	}
}
