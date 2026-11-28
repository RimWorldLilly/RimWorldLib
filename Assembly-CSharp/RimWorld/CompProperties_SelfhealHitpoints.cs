using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE5 RID: 3813
	public class CompProperties_SelfhealHitpoints : CompProperties
	{
		// Token: 0x060063BF RID: 25535 RVA: 0x0023C465 File Offset: 0x0023A665
		public CompProperties_SelfhealHitpoints()
		{
			this.compClass = typeof(CompSelfhealHitpoints);
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x0023C47D File Offset: 0x0023A67D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_SelfhealHitpoints.<ConfigErrors>d__2 <ConfigErrors>d__ = new CompProperties_SelfhealHitpoints.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400381C RID: 14364
		public int ticksPerHeal;
	}
}
