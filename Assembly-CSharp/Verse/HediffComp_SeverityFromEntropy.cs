using System;

namespace Verse
{
	// Token: 0x02000477 RID: 1143
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x000C1472 File Offset: 0x000BF672
		private float EntropyAmount
		{
			get
			{
				if (base.Pawn.psychicEntropy != null)
				{
					return base.Pawn.psychicEntropy.EntropyRelativeValue;
				}
				return 0f;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x000C1497 File Offset: 0x000BF697
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x000C14A6 File Offset: 0x000BF6A6
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
