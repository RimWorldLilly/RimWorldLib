using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE3 RID: 3811
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x060063B6 RID: 25526 RVA: 0x0023C37E File Offset: 0x0023A57E
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x060063B7 RID: 25527 RVA: 0x0023C396 File Offset: 0x0023A596
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ProximityFuse.<ConfigErrors>d__3 <ConfigErrors>d__ = new CompProperties_ProximityFuse.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003814 RID: 14356
		public ThingDef target;

		// Token: 0x04003815 RID: 14357
		public float radius;
	}
}
