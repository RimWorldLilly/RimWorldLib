using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001890 RID: 6288
	public class CompProperties_FleshmassNucleus : CompProperties
	{
		// Token: 0x06009D7C RID: 40316 RVA: 0x00389B78 File Offset: 0x00387D78
		public CompProperties_FleshmassNucleus()
		{
			this.compClass = typeof(CompFleshmassNucleus);
		}

		// Token: 0x04005D4A RID: 23882
		public float activityOnRoofCollapsed = 0.3f;

		// Token: 0x04005D4B RID: 23883
		public SimpleCurve activityMeatPerDayCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 20f),
				true
			},
			{
				new CurvePoint(0.9f, 120f),
				true
			}
		};
	}
}
