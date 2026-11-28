using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187C RID: 6268
	public class CompProperties_FacilityInUse : CompProperties
	{
		// Token: 0x06009CFD RID: 40189 RVA: 0x00387AC9 File Offset: 0x00385CC9
		public CompProperties_FacilityInUse()
		{
			this.compClass = typeof(CompFacilityInUse);
		}

		// Token: 0x06009CFE RID: 40190 RVA: 0x00387AE1 File Offset: 0x00385CE1
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_FacilityInUse.<ConfigErrors>d__3 <ConfigErrors>d__ = new CompProperties_FacilityInUse.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005D11 RID: 23825
		public float? inUsePowerConsumption;

		// Token: 0x04005D12 RID: 23826
		public EffecterDef effectInUse;
	}
}
