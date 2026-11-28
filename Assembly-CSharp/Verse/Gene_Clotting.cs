using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003DF RID: 991
	public class Gene_Clotting : Gene
	{
		// Token: 0x06001D33 RID: 7475 RVA: 0x000B2588 File Offset: 0x000B0788
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick(360, delta))
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = hediffs.Count - 1; i >= 0; i--)
				{
					if (hediffs[i].Bleeding)
					{
						hediffs[i].Tended(Gene_Clotting.TendingQualityRange.RandomInRange, Gene_Clotting.TendingQualityRange.TrueMax, 1);
					}
				}
			}
		}

		// Token: 0x0400176E RID: 5998
		private const int ClotCheckInterval = 360;

		// Token: 0x0400176F RID: 5999
		private static readonly FloatRange TendingQualityRange = new FloatRange(0.2f, 0.7f);
	}
}
