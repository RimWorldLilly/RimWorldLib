using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE1 RID: 3809
	public class CompProperties_ProjectileEffecter : CompProperties
	{
		// Token: 0x060063B1 RID: 25521 RVA: 0x0023C28F File Offset: 0x0023A48F
		public CompProperties_ProjectileEffecter()
		{
			this.compClass = typeof(Comp_ProjectileEffecter);
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x0023C2A7 File Offset: 0x0023A4A7
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ProjectileEffecter.<ConfigErrors>d__2 <ConfigErrors>d__ = new CompProperties_ProjectileEffecter.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003812 RID: 14354
		public EffecterDef effecterDef;
	}
}
