using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFA RID: 8186
	public class InfectionPathwayDef : Def
	{
		// Token: 0x17002559 RID: 9561
		// (get) Token: 0x0600CF65 RID: 53093 RVA: 0x004C60A6 File Offset: 0x004C42A6
		public float ExpiryDays
		{
			get
			{
				return this.expiryDays;
			}
		}

		// Token: 0x1700255A RID: 9562
		// (get) Token: 0x0600CF66 RID: 53094 RVA: 0x004C60AE File Offset: 0x004C42AE
		public int ExpiryTicks
		{
			get
			{
				return (int)this.expiryDays * 60000;
			}
		}

		// Token: 0x1700255B RID: 9563
		// (get) Token: 0x0600CF67 RID: 53095 RVA: 0x004C60BD File Offset: 0x004C42BD
		public bool PawnRequired
		{
			get
			{
				return this.pawnRequired;
			}
		}

		// Token: 0x040083EA RID: 33770
		private float expiryDays = 30f;

		// Token: 0x040083EB RID: 33771
		private bool pawnRequired;
	}
}
