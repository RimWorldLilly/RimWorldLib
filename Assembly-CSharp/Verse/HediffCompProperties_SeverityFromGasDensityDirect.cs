using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	public class HediffCompProperties_SeverityFromGasDensityDirect : HediffCompProperties
	{
		// Token: 0x0600200C RID: 8204 RVA: 0x000C15E9 File Offset: 0x000BF7E9
		public HediffCompProperties_SeverityFromGasDensityDirect()
		{
			this.compClass = typeof(HediffComp_SeverityFromGasDensityDirect);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000C1614 File Offset: 0x000BF814
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_SeverityFromGasDensityDirect.<ConfigErrors>d__4 <ConfigErrors>d__ = new HediffCompProperties_SeverityFromGasDensityDirect.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040018EE RID: 6382
		public GasType gasType;

		// Token: 0x040018EF RID: 6383
		public int intervalTicks = 60;

		// Token: 0x040018F0 RID: 6384
		public List<float> densityStages = new List<float>();
	}
}
