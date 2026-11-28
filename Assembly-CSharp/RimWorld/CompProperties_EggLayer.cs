using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED3 RID: 3795
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x0600638E RID: 25486 RVA: 0x0023BBE4 File Offset: 0x00239DE4
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x0023BC36 File Offset: 0x00239E36
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_EggLayer.<ConfigErrors>d__8 <ConfigErrors>d__ = new CompProperties_EggLayer.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040037AD RID: 14253
		public float eggLayIntervalDays = 1f;

		// Token: 0x040037AE RID: 14254
		public IntRange eggCountRange = IntRange.One;

		// Token: 0x040037AF RID: 14255
		public ThingDef eggUnfertilizedDef;

		// Token: 0x040037B0 RID: 14256
		public ThingDef eggFertilizedDef;

		// Token: 0x040037B1 RID: 14257
		public int eggFertilizationCountMax = 1;

		// Token: 0x040037B2 RID: 14258
		public bool eggLayFemaleOnly = true;

		// Token: 0x040037B3 RID: 14259
		public float eggProgressUnfertilizedMax = 1f;
	}
}
