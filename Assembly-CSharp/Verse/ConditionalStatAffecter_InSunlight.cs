using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DF RID: 223
	public class ConditionalStatAffecter_InSunlight : ConditionalStatAffecter
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00030033 File Offset: 0x0002E233
		public override string Label
		{
			get
			{
				return "StatsReport_InSunlight".Translate();
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00030044 File Offset: 0x0002E244
		public override bool Applies(StatRequest req)
		{
			return ModsConfig.BiotechActive && (req.HasThing && req.Thing.Spawned) && req.Thing.Position.InSunlight(req.Thing.Map);
		}
	}
}
