using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DD RID: 221
	public class ConditionalStatAffecter_NotInSpace : ConditionalStatAffecter_InSpace
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0002FFA3 File Offset: 0x0002E1A3
		public override string Label
		{
			get
			{
				return "StatsReport_NotInSpace".Translate();
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002FFB4 File Offset: 0x0002E1B4
		public override bool Applies(StatRequest req)
		{
			return !base.Applies(req);
		}
	}
}
