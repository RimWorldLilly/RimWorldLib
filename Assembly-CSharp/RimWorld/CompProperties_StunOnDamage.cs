using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A4 RID: 6564
	public class CompProperties_StunOnDamage : CompProperties
	{
		// Token: 0x0600A4D8 RID: 42200 RVA: 0x003AE372 File Offset: 0x003AC572
		public CompProperties_StunOnDamage()
		{
			this.compClass = typeof(CompStunOnDamage);
		}

		// Token: 0x0400616F RID: 24943
		public int delayTicks;

		// Token: 0x04006170 RID: 24944
		public DamageDef damage;
	}
}
