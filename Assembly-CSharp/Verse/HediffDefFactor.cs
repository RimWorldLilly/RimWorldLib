using System;

namespace Verse
{
	// Token: 0x0200048C RID: 1164
	public class HediffDefFactor
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x000C266D File Offset: 0x000C086D
		public HediffDef HediffDef
		{
			get
			{
				return this.hediffDef;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x000C2675 File Offset: 0x000C0875
		public float Factor
		{
			get
			{
				return this.factor;
			}
		}

		// Token: 0x04001910 RID: 6416
		private HediffDef hediffDef;

		// Token: 0x04001911 RID: 6417
		private float factor = 1f;
	}
}
